using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour
{
    private string movementCapsulePrefabPath;
    private GameObject movementCapsulePrefab;

    private Vector3 currentlySelectedDestination;
    private float destinationRange;

    private Unit currentlySelectedTarget;
    private Unit currentlySelectedBasicAttackTarget;
    private float targetRange;

    private IEnumerator currentMovementCoroutine;

    private Character character;

    public Vector3 CharacterHeightOffset { get; private set; }

    public delegate void PlayerMovedHandler();
    public event PlayerMovedHandler CharacterMoved;

    public delegate void CharacterIsInDestinationRangeHandler(Vector3 destination);
    public event CharacterIsInDestinationRangeHandler CharacterIsInDestinationRange;

    public delegate void CharacterIsInTargetRangeHandler(Unit target);
    public event CharacterIsInTargetRangeHandler CharacterIsInTargetRange;

    private MovementManager()
    {
        currentlySelectedDestination = Vector3.down;

        movementCapsulePrefabPath = "MovementCapsulePrefab/MovementCapsule";
    }

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        if (character.IsLocalCharacter())
        {
            character.InputManager.OnPressedS += StopMovement;

            LoadPrefabs();
        }

        CharacterHeightOffset = Vector3.up * transform.position.y;
    }

    private void LoadPrefabs()
    {
        movementCapsulePrefab = Resources.Load<GameObject>(movementCapsulePrefabPath);
    }

    public void UnsubscribeCameraEvent()
    {
        CharacterMoved = null;
    }

    public void PrepareMovementTowardsPoint(Vector3 pointOnMap)
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Movement_Point(pointOnMap + CharacterHeightOffset);
        }
        else
        {
            SetMoveTowardsPoint(pointOnMap + CharacterHeightOffset);
        }
        Instantiate(movementCapsulePrefab, pointOnMap, Quaternion.identity);
    }

    public void PrepareMovementTowardsTarget(Unit target)
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Movement_Target(target, character.StatsManager.AttackRange.GetTotal(), true);
        }
        else
        {
            SetMoveTowardsTarget(target, character.StatsManager.AttackRange.GetTotal(), true);
        }
    }

    private void SendToServer_Movement_Point(Vector3 destination)
    {
        PhotonNetwork.RemoveRPCs(character.PhotonView);//if using AllBufferedViaServer somewhere else, this needs to change
        character.PhotonView.RPC("ReceiveFromServer_Movement_Point", PhotonTargets.AllBufferedViaServer, destination);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement_Point(Vector3 destination)
    {
        SetMoveTowardsPoint(destination);
    }

    private void SendToServer_Movement_Target(Unit target, float range, bool isBasicAttack)
    {
        PhotonNetwork.RemoveRPCs(character.PhotonView);//if using AllBufferedViaServer somewhere else, this needs to change
        character.PhotonView.RPC("ReceiveFromServer_Movement_Target", PhotonTargets.AllBufferedViaServer, target.ID, target.UnitType, range, isBasicAttack);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement_Target(int unitId, UnitType unitType, float range, bool isBasicAttack)
    {
        SetMoveTowardsTarget(character.AbilityManager.FindTarget(unitId, unitType), range, isBasicAttack);
    }

    public void SetMoveTowardsPoint(Vector3 destination, float range = 0)
    {
        if (currentlySelectedDestination != destination)
        {
            if (currentlySelectedDestination != Vector3.down && (CharacterIsInDestinationRange == null || range > 0) && (CharacterIsInDestinationRange != null || range == 0))
            {
                currentlySelectedDestination = destination;
                character.OrientationManager.RotateCharacter(destination);
            }
            else
            {
                StopAllMovement();
                currentlySelectedDestination = destination;
                destinationRange = range;
                currentMovementCoroutine = range > 0 ? MoveTowardsPointWithRange() : MoveTowardsPoint();
                StartCoroutine(currentMovementCoroutine);
                character.OrientationManager.RotateCharacter(destination);
            }
        }
    }

    private IEnumerator MoveTowardsPoint()
    {
        character.AutoAttackManager.EnableAutoAttack();

        while (transform.position != currentlySelectedDestination)
        {
            if (character.AbilityManager.CanUseMovement() && character.StatusManager.CanUseMovement() && !character.DisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentlySelectedDestination, Time.deltaTime * character.StatsManager.MovementSpeed.GetTotal());

                NotifyCharacterMoved();
            }

            yield return null;
        }

        currentlySelectedDestination = Vector3.down;
        currentMovementCoroutine = null;
    }

    private IEnumerator MoveTowardsPointWithRange()
    {
        character.AutoAttackManager.StopAutoAttack();

        float distance = Vector3.Distance(currentlySelectedDestination, transform.position);
        while (distance > destinationRange || (distance <= destinationRange && character.DisplacementManager.IsBeingDisplaced))
        {
            if (character.AbilityManager.CanUseMovement() && character.StatusManager.CanUseMovement() && !character.DisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentlySelectedDestination, Time.deltaTime * character.StatsManager.MovementSpeed.GetTotal());

                NotifyCharacterMoved();
            }

            yield return null;

            distance = Vector3.Distance(currentlySelectedDestination, transform.position);
        }

        if (CharacterIsInDestinationRange != null)
        {
            CharacterIsInDestinationRange(currentlySelectedDestination);
            CharacterIsInDestinationRange = null;
        }

        character.AutoAttackManager.EnableAutoAttack();
        currentlySelectedDestination = Vector3.down;
        currentMovementCoroutine = null;
    }

    public void SetMoveTowardsHalfDistanceOfAbilityCastRange()
    {
        if (IsMovingTowardsPositionForAnEvent())
        {
            CharacterIsInDestinationRange = null;
            destinationRange *= 0.5f;
        }
    }

    public void SetCharacterIsInTargetRangeEventForBasicAttack()//example: Lucian Q out of range -> Lucian W while walking: This cancels the Q cast but continues movement post-W to auto the same target
    {
        if (currentlySelectedTarget != null)
        {
            CharacterIsInDestinationRange = null;
            CharacterIsInTargetRange = null;
            character.BasicAttack.SetupBasicAttack(currentlySelectedTarget, false);
        }
    }

    public void SetMoveTowardsTarget(Unit target, float range, bool isBasicAttack, bool forceNewCoroutine = false)
    {
        if (CharacterIsInTargetRange == null || forceNewCoroutine)
        {
            StopAllMovement();
            SetupCorrectTarget(target, isBasicAttack);
            targetRange = isBasicAttack ? character.StatsManager.AttackRange.GetTotal() : range;
            currentMovementCoroutine = MoveTowardsTarget(isBasicAttack);
            StartCoroutine(currentMovementCoroutine);
        }
        else
        {
            CharacterIsInTargetRange = null;
            SetupCorrectTarget(target, isBasicAttack);
            if (Vector3.Distance(target.transform.position, transform.position) > range)
            {
                character.OrientationManager.RotateCharacterUntilReachedTarget(target.transform, isBasicAttack);
            }
        }
    }

    private void SetupCorrectTarget(Unit target, bool isBasicAttack)
    {
        currentlySelectedTarget = !isBasicAttack ? target : null;
        currentlySelectedBasicAttackTarget = isBasicAttack ? target : null;
        if (isBasicAttack)
        {
            character.BasicAttack.SetupBasicAttack(target, true);
        }
    }

    private IEnumerator MoveTowardsTarget(bool isBasicAttack)
    {
        Unit target = currentlySelectedBasicAttackTarget != null ? currentlySelectedBasicAttackTarget : currentlySelectedTarget;

        if (target != null && Vector3.Distance(target.transform.position, transform.position) > targetRange)
        {
            character.AutoAttackManager.StopAutoAttack();

            Transform targetTransform = target.transform;

            character.OrientationManager.RotateCharacterUntilReachedTarget(targetTransform, currentlySelectedBasicAttackTarget != null);

            float distance = Vector3.Distance(targetTransform.position, transform.position);
            while (distance > targetRange || (distance <= targetRange && character.DisplacementManager.IsBeingDisplaced))
            {
                if (character.AbilityManager.CanUseMovement() && character.StatusManager.CanUseMovement() && !character.DisplacementManager.IsBeingDisplaced)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * character.StatsManager.MovementSpeed.GetTotal());

                    NotifyCharacterMoved();
                }

                yield return null;

                target = currentlySelectedBasicAttackTarget != null ? currentlySelectedBasicAttackTarget : currentlySelectedTarget;
                targetTransform = target ? target.transform : null;

                if (targetTransform == null)
                {
                    break;
                }
                distance = Vector3.Distance(targetTransform.position, transform.position);

                if (isBasicAttack)
                {
                    targetRange = character.StatsManager.AttackRange.GetTotal();
                }
            }

            character.AutoAttackManager.EnableAutoAttack();
            character.OrientationManager.StopRotation();
        }

        if (target != null)
        {
            if (target == currentlySelectedBasicAttackTarget && (!character.AbilityManager.CanUseBasicAttacks() || !character.StatusManager.CanUseBasicAttacks() || character.DisplacementManager.IsBeingDisplaced))//Checks if disarmed
            {
                while (!character.AbilityManager.CanUseBasicAttacks() || !character.StatusManager.CanUseBasicAttacks() || character.DisplacementManager.IsBeingDisplaced)
                {
                    yield return null;
                }
                SetMoveTowardsTarget(currentlySelectedBasicAttackTarget, targetRange, true);
            }

            currentlySelectedTarget = null;
            currentlySelectedBasicAttackTarget = null;

            if (CharacterIsInTargetRange != null)
            {
                CharacterIsInTargetRange(target);
                CharacterIsInTargetRange = null;
            }
        }
        else if (!IsMoving())
        {
            StopAllMovement();
            character.AutoAttackManager.EnableAutoAttack();
        }

        currentMovementCoroutine = null;
    }

    public void RotateCharacterIfMoving()
    {
        if (currentlySelectedDestination != Vector3.down)
        {
            character.OrientationManager.RotateCharacter(currentlySelectedDestination);
        }
        else if (currentlySelectedTarget != null)
        {
            character.OrientationManager.RotateCharacterUntilReachedTarget(currentlySelectedTarget.transform, true);
        }
        else if (currentlySelectedBasicAttackTarget != null)
        {
            character.OrientationManager.RotateCharacterUntilReachedTarget(currentlySelectedBasicAttackTarget.transform, true);
        }
    }

    private void StopMovement()
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_StopMovement();
        }
        else
        {
            character.AutoAttackManager.StopAutoAttack();
            StopAllMovement();
        }
    }

    private void SendToServer_StopMovement()
    {
        character.PhotonView.RPC("ReceiveFromServer_StopMovement", PhotonTargets.AllViaServer);
    }

    [PunRPC]
    private void ReceiveFromServer_StopMovement()
    {
        character.AutoAttackManager.StopAutoAttack();
        StopAllMovement();
    }

    public void StopAllMovement(bool resetBufferedAbility = true)
    {
        character.BasicAttack.StopBasicAttack();
        if (resetBufferedAbility)
        {
            character.BufferedAbilityManager.ResetBufferedAbility();
        }

        //TODO: Sometimes, it does not enter the if even when there is a coroutine running, so 2+ are alive at once. Try to find why and remove StopAllCoroutines() if possible!
        //if (currentMovementCoroutine != null)
        //{
        //    StopCoroutine(currentMovementCoroutine);
        //    currentMovementCoroutine = null;
        //}
        StopAllCoroutines();
        currentMovementCoroutine = null;

        character.OrientationManager.StopRotation();
        CharacterIsInTargetRange = null;
        CharacterIsInDestinationRange = null;
        currentlySelectedDestination = Vector3.down;
        destinationRange = 0;
        currentlySelectedTarget = null;
        currentlySelectedBasicAttackTarget = null;
        targetRange = 0;
    }

    public void StopMovementTowardsPoint()
    {
        if (currentlySelectedDestination != Vector3.down)
        {
            StopAllMovement();
        }
    }

    public void StopMovementTowardsTarget()
    {
        if (IsMovingTowardsTarget())
        {
            StopAllMovement();
        }
    }

    public void StopMovementTowardsPointIfHasEvent()
    {
        if (IsMovingTowardsPositionForAnEvent())
        {
            StopAllMovement();
        }
    }

    public void StopMovementTowardsTargetIfHasEvent(bool stopBasicAttack = false)
    {
        if ((currentlySelectedTarget != null || (stopBasicAttack && currentlySelectedBasicAttackTarget != null)) && CharacterIsInTargetRange != null)
        {
            StopAllMovement();
        }
    }

    public bool IsMoving()
    {
        return IsMovingTowardsPosition() || IsMovingTowardsTarget();
    }

    public bool IsMovingTowardsPosition()
    {
        return currentlySelectedDestination != Vector3.down;
    }

    public bool IsMovingTowardsPositionForAnEvent()
    {
        return IsMovingTowardsPosition() && CharacterIsInDestinationRange != null;
    }

    public bool IsMovingTowardsTarget()
    {
        return currentlySelectedTarget != null || currentlySelectedBasicAttackTarget != null;
    }

    public Unit GetBasicAttackTarget()
    {
        return currentlySelectedBasicAttackTarget;
    }

    public void NotifyCharacterMoved()
    {
        if (CharacterMoved != null)// && (character.IsLocalCharacter()))
        {
            CharacterMoved();
        }
    }
}
