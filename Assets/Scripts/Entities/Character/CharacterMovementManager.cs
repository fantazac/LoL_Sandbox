using UnityEngine;
using System.Collections;

public class CharacterMovementManager : MonoBehaviour
{
    private string movementCapsulePrefabPath;
    private GameObject movementCapsulePrefab;

    private Vector3 currentlySelectedDestination;
    private float destinationRange;

    private Entity currentlySelectedTarget;
    private Entity currentlySelectedBasicAttackTarget;
    private float targetRange;

    private IEnumerator currentMovementCoroutine;

    private Character character;

    public Vector3 CharacterHeightOffset { get; private set; }

    public delegate void PlayerMovedHandler();
    public event PlayerMovedHandler CharacterMoved;

    public delegate void CharacterIsInDestinationRangeHandler(Vector3 destination);
    public event CharacterIsInDestinationRangeHandler CharacterIsInDestinationRange;

    public delegate void CharacterIsInTargetRangeHandler(Entity target);
    public event CharacterIsInTargetRangeHandler CharacterIsInTargetRange;

    private CharacterMovementManager()
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
            character.CharacterInputManager.OnPressedS += StopMovement;

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

    public void PrepareMovementTowardsTarget(Entity target)
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Movement_Target(target, character.EntityStatsManager.AttackRange.GetTotal(), true);
        }
        else
        {
            SetMoveTowardsTarget(target, character.EntityStatsManager.AttackRange.GetTotal(), true);
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

    private void SendToServer_Movement_Target(Entity target, float range, bool isBasicAttack)
    {
        PhotonNetwork.RemoveRPCs(character.PhotonView);//if using AllBufferedViaServer somewhere else, this needs to change
        character.PhotonView.RPC("ReceiveFromServer_Movement_Target", PhotonTargets.AllBufferedViaServer, target.EntityId, target.EntityType, range, isBasicAttack);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement_Target(int entityId, EntityType entityType, float range, bool isBasicAttack)
    {
        SetMoveTowardsTarget(character.CharacterAbilityManager.FindTarget(entityId, entityType), range, isBasicAttack);
    }

    public void SetMoveTowardsPoint(Vector3 destination, float range = 0)
    {
        if (currentlySelectedDestination != destination)
        {
            if (currentlySelectedDestination != Vector3.down && (CharacterIsInDestinationRange == null || range > 0) && (CharacterIsInDestinationRange != null || range == 0))
            {
                currentlySelectedDestination = destination;
                character.CharacterOrientationManager.RotateCharacter(destination);
            }
            else
            {
                StopAllMovement();
                currentlySelectedDestination = destination;
                destinationRange = range;
                currentMovementCoroutine = range > 0 ? MoveTowardsPointWithRange() : MoveTowardsPoint();
                StartCoroutine(currentMovementCoroutine);
                character.CharacterOrientationManager.RotateCharacter(destination);
            }
        }
    }

    private IEnumerator MoveTowardsPoint()
    {
        character.CharacterAutoAttackManager.EnableAutoAttack();

        while (transform.position != currentlySelectedDestination)
        {
            if (character.CharacterAbilityManager.CanUseMovement() && character.EntityStatusManager.CanUseMovement() && !character.EntityDisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentlySelectedDestination, Time.deltaTime * character.EntityStatsManager.MovementSpeed.GetTotal());

                NotifyCharacterMoved();
            }

            yield return null;
        }

        currentlySelectedDestination = Vector3.down;
        currentMovementCoroutine = null;
    }

    private IEnumerator MoveTowardsPointWithRange()
    {
        character.CharacterAutoAttackManager.StopAutoAttack();

        float distance = Vector3.Distance(currentlySelectedDestination, transform.position);
        while (distance > destinationRange || (distance <= destinationRange && character.EntityDisplacementManager.IsBeingDisplaced))
        {
            if (character.CharacterAbilityManager.CanUseMovement() && character.EntityStatusManager.CanUseMovement() && !character.EntityDisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentlySelectedDestination, Time.deltaTime * character.EntityStatsManager.MovementSpeed.GetTotal());

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

        character.CharacterAutoAttackManager.EnableAutoAttack();
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
            character.EntityBasicAttack.SetupBasicAttack(currentlySelectedTarget, false);
        }
    }

    public void SetMoveTowardsTarget(Entity target, float range, bool isBasicAttack, bool forceNewCoroutine = false)
    {
        if (CharacterIsInTargetRange == null || forceNewCoroutine)
        {
            StopAllMovement();
            SetupCorrectTarget(target, isBasicAttack);
            targetRange = isBasicAttack ? character.EntityStatsManager.AttackRange.GetTotal() : range;
            currentMovementCoroutine = MoveTowardsTarget(isBasicAttack);
            StartCoroutine(currentMovementCoroutine);
        }
        else
        {
            CharacterIsInTargetRange = null;
            SetupCorrectTarget(target, isBasicAttack);
            if (Vector3.Distance(target.transform.position, transform.position) > range)
            {
                character.CharacterOrientationManager.RotateCharacterUntilReachedTarget(target.transform, isBasicAttack);
            }
        }
    }

    private void SetupCorrectTarget(Entity target, bool isBasicAttack)
    {
        currentlySelectedTarget = !isBasicAttack ? target : null;
        currentlySelectedBasicAttackTarget = isBasicAttack ? target : null;
        if (isBasicAttack)
        {
            character.EntityBasicAttack.SetupBasicAttack(target, true);
        }
    }

    private IEnumerator MoveTowardsTarget(bool isBasicAttack)
    {
        Entity target = currentlySelectedBasicAttackTarget != null ? currentlySelectedBasicAttackTarget : currentlySelectedTarget;

        if (target != null && Vector3.Distance(target.transform.position, transform.position) > targetRange)
        {
            character.CharacterAutoAttackManager.StopAutoAttack();

            Transform targetTransform = target.transform;

            character.CharacterOrientationManager.RotateCharacterUntilReachedTarget(targetTransform, currentlySelectedBasicAttackTarget != null);

            float distance = Vector3.Distance(targetTransform.position, transform.position);
            while (distance > targetRange || (distance <= targetRange && character.EntityDisplacementManager.IsBeingDisplaced))
            {
                if (character.CharacterAbilityManager.CanUseMovement() && character.EntityStatusManager.CanUseMovement() && !character.EntityDisplacementManager.IsBeingDisplaced)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * character.EntityStatsManager.MovementSpeed.GetTotal());

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
                    targetRange = character.EntityStatsManager.AttackRange.GetTotal();
                }
            }

            character.CharacterAutoAttackManager.EnableAutoAttack();
            character.CharacterOrientationManager.StopRotation();
        }

        if (target != null)
        {
            if (target == currentlySelectedBasicAttackTarget && (!character.CharacterAbilityManager.CanUseBasicAttacks() || !character.EntityStatusManager.CanUseBasicAttacks() || character.EntityDisplacementManager.IsBeingDisplaced))//Checks if disarmed
            {
                while (!character.CharacterAbilityManager.CanUseBasicAttacks() || !character.EntityStatusManager.CanUseBasicAttacks() || character.EntityDisplacementManager.IsBeingDisplaced)
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
            character.CharacterAutoAttackManager.EnableAutoAttack();
        }

        currentMovementCoroutine = null;
    }

    public void RotateCharacterIfMoving()
    {
        if (currentlySelectedDestination != Vector3.down)
        {
            character.CharacterOrientationManager.RotateCharacter(currentlySelectedDestination);
        }
        else if (currentlySelectedTarget != null)
        {
            character.CharacterOrientationManager.RotateCharacterUntilReachedTarget(currentlySelectedTarget.transform, true);
        }
        else if (currentlySelectedBasicAttackTarget != null)
        {
            character.CharacterOrientationManager.RotateCharacterUntilReachedTarget(currentlySelectedBasicAttackTarget.transform, true);
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
            character.CharacterAutoAttackManager.StopAutoAttack();
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
        character.CharacterAutoAttackManager.StopAutoAttack();
        StopAllMovement();
    }

    public void StopAllMovement(bool resetBufferedAbility = true)
    {
        character.EntityBasicAttack.StopBasicAttack();
        if (resetBufferedAbility)
        {
            character.CharacterBufferedAbilityManager.ResetBufferedAbility();
        }

        //TODO: Sometimes, it does not enter the if even when there is a coroutine running, so 2+ are alive at once. Try to find why and remove StopAllCoroutines() if possible!
        //if (currentMovementCoroutine != null)
        //{
        //    StopCoroutine(currentMovementCoroutine);
        //    currentMovementCoroutine = null;
        //}
        StopAllCoroutines();
        currentMovementCoroutine = null;

        character.CharacterOrientationManager.StopRotation();
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

    public Entity GetBasicAttackTarget()
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
