using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    private string movementCapsulePrefabPath;
    private GameObject movementCapsulePrefab;

    private Vector3 currentlySelectedDestination;
    private Entity currentlySelectedTarget;
    private Entity currentlySelectedBasicAttackTarget;

    private IEnumerator currentMovementCoroutine;

    private Character character;

    public Vector3 CharacterHeightOffset { get; private set; }

    public delegate void PlayerMovedHandler();
    public event PlayerMovedHandler CharacterMoved;

    public delegate void CharacterIsInDestinationRangeHandler(Vector3 destination);
    public event CharacterIsInDestinationRangeHandler CharacterIsInDestinationRange;

    public delegate void CharacterIsInTargetRangeHandler(Entity target);
    public event CharacterIsInTargetRangeHandler CharacterIsInTargetRange;

    private CharacterMovement()
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
        if (!StaticObjects.OnlineMode || character.PhotonView.isMine)
        {
            character.CharacterInput.OnPressedS += StopMovement;

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
            SendToServer_Movement_Target(target, character.EntityStats.AttackRange.GetTotal(), true);
        }
        else
        {
            SetMoveTowardsTarget(target, character.EntityStats.AttackRange.GetTotal(), true);
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
                character.CharacterOrientation.RotateCharacter(destination);
            }
            else
            {
                StopAllMovement();
                currentlySelectedDestination = destination;
                currentMovementCoroutine = range > 0 ? MoveTowardsPoint(range) : MoveTowardsPoint();
                StartCoroutine(currentMovementCoroutine);
                character.CharacterOrientation.RotateCharacter(destination);
            }
        }
    }

    private IEnumerator MoveTowardsPoint()
    {
        character.CharacterAutoAttack.EnableAutoAttack();

        while (transform.position != currentlySelectedDestination)
        {
            if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement() && character.EntityStatusManager.CanUseMovement() && !character.EntityDisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentlySelectedDestination, Time.deltaTime * character.EntityStats.MovementSpeed.GetTotal());

                NotifyCharacterMoved();
            }

            yield return null;
        }

        currentlySelectedDestination = Vector3.down;
        currentMovementCoroutine = null;
    }

    private IEnumerator MoveTowardsPoint(float range)
    {
        character.CharacterAutoAttack.StopAutoAttack();

        while (Vector3.Distance(currentlySelectedDestination, transform.position) > range)
        {
            if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement() && character.EntityStatusManager.CanUseMovement() && !character.EntityDisplacementManager.IsBeingDisplaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentlySelectedDestination, Time.deltaTime * character.EntityStats.MovementSpeed.GetTotal());

                NotifyCharacterMoved();
            }

            yield return null;
        }

        if (CharacterIsInDestinationRange != null)
        {
            CharacterIsInDestinationRange(currentlySelectedDestination);
            CharacterIsInDestinationRange = null;
        }

        character.CharacterAutoAttack.EnableAutoAttack();
        currentlySelectedDestination = Vector3.down;
        currentMovementCoroutine = null;
    }

    public void SetCharacterIsInRangeEventForBasicAttack()//example: Lucian Q out of range -> Lucian W while walking: This cancels the Q cast but continues movement post-W to auto the same target
    {
        if (currentlySelectedTarget != null)
        {
            CharacterIsInDestinationRange = null;
            CharacterIsInTargetRange = null;
            character.EntityBasicAttack.SetupBasicAttack(currentlySelectedTarget, false);
        }
    }

    public void RestartMovementTowardsTargetAfterAbility()
    {
        if (character.EntityBasicAttack.AttackIsInQueue)
        {
            character.EntityBasicAttack.StopBasicAttack();//This is so CharacterAutoAttack doesn't shoot while an ability is active
            if (character.EntityBasicAttack.CurrentTarget() != null)
            {
                SetMoveTowardsTarget(character.EntityBasicAttack.CurrentTarget(), character.EntityStats.AttackRange.GetTotal(), true);
            }
        }
    }

    public void SetMoveTowardsTarget(Entity target, float range, bool isBasicAttack, bool forceNewCoroutine = false)
    {
        if (CharacterIsInTargetRange == null || forceNewCoroutine)
        {
            StopAllMovement();
            SetupCorrectTarget(target, isBasicAttack);
            currentMovementCoroutine = MoveTowardsTarget(range);
            StartCoroutine(currentMovementCoroutine);
        }
        else
        {
            CharacterIsInTargetRange = null;
            SetupCorrectTarget(target, isBasicAttack);
            if (Vector3.Distance(target.transform.position, transform.position) > range)
            {
                character.CharacterOrientation.RotateCharacterUntilReachedTarget(target.transform, isBasicAttack);
            }
        }

        //TODO: Remove this after multiple tests, keeping it as a comment in case the newer version breaks something else
        /*StopAllMovement();
        if (isBasicAttack)
        {
            currentlySelectedBasicAttackTarget = target;
            character.EntityBasicAttack.SetupBasicAttack(target, true);
        }
        else
        {
            currentlySelectedTarget = target;
        }
        currentMovementCoroutine = MoveTowardsTarget(range);
        StartCoroutine(currentMovementCoroutine);*/
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

    private IEnumerator MoveTowardsTarget(float range)
    {
        Entity target = currentlySelectedBasicAttackTarget != null ? currentlySelectedBasicAttackTarget : currentlySelectedTarget;

        if (target != null && Vector3.Distance(target.transform.position, transform.position) > range)
        {
            character.CharacterAutoAttack.StopAutoAttack();

            Transform targetTransform = target.transform;

            character.CharacterOrientation.RotateCharacterUntilReachedTarget(targetTransform, currentlySelectedBasicAttackTarget != null);

            while (targetTransform != null && Vector3.Distance(targetTransform.position, transform.position) > range)
            {
                if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement() && character.EntityStatusManager.CanUseMovement() && !character.EntityDisplacementManager.IsBeingDisplaced)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * character.EntityStats.MovementSpeed.GetTotal());

                    NotifyCharacterMoved();
                }

                yield return null;

                target = currentlySelectedBasicAttackTarget != null ? currentlySelectedBasicAttackTarget : currentlySelectedTarget;
                targetTransform = target ? target.transform : null;
            }

            character.CharacterAutoAttack.EnableAutoAttack();
            character.CharacterOrientation.StopRotation();
        }

        if (target != null)
        {
            if (target == currentlySelectedBasicAttackTarget && (character.CharacterAbilityManager.IsUsingAbilityPreventingBasicAttacks() || !character.EntityStatusManager.CanUseBasicAttacks() || character.EntityDisplacementManager.IsBeingDisplaced))//checks is disarmed
            {
                while (character.CharacterAbilityManager.IsUsingAbilityPreventingBasicAttacks() || !character.EntityStatusManager.CanUseBasicAttacks() || character.EntityDisplacementManager.IsBeingDisplaced)
                {
                    yield return null;
                }

                SetMoveTowardsTarget(currentlySelectedBasicAttackTarget, range, true);
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
            character.CharacterAutoAttack.EnableAutoAttack();
        }

        currentMovementCoroutine = null;
    }

    public void RotateCharacterIfMoving()
    {
        if (currentlySelectedDestination != Vector3.down)
        {
            character.CharacterOrientation.RotateCharacter(currentlySelectedDestination);
        }
        else if (currentlySelectedTarget != null)
        {
            character.CharacterOrientation.RotateCharacterUntilReachedTarget(currentlySelectedTarget.transform, true);
        }
        else if (currentlySelectedBasicAttackTarget != null)
        {
            character.CharacterOrientation.RotateCharacterUntilReachedTarget(currentlySelectedBasicAttackTarget.transform, true);
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
            character.CharacterAutoAttack.StopAutoAttack();
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
        character.CharacterAutoAttack.StopAutoAttack();
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

        character.CharacterOrientation.StopRotation();
        CharacterIsInTargetRange = null;
        CharacterIsInDestinationRange = null;
        currentlySelectedDestination = Vector3.down;
        currentlySelectedTarget = null;
        currentlySelectedBasicAttackTarget = null;
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
        if (currentlySelectedTarget != null || currentlySelectedBasicAttackTarget != null)
        {
            StopAllMovement();
        }
    }

    public void StopMovementTowardsPointIfHasEvent()
    {
        if (currentlySelectedDestination != Vector3.down && CharacterIsInDestinationRange != null)
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

    public void UnsubscribeMovementTowardsPointEvents()
    {
        CharacterIsInDestinationRange = null;
    }

    public void UnsubscribeMovementTowardsTargetEvents()
    {
        CharacterIsInTargetRange = null;
    }

    public bool IsMoving()
    {
        return currentlySelectedDestination != Vector3.down || currentlySelectedTarget != null || currentlySelectedBasicAttackTarget != null;
    }

    public bool IsMovingTowardsPosition()
    {
        return currentlySelectedDestination != Vector3.down && CharacterIsInDestinationRange == null;
    }

    public bool IsMovingTowardsPositionForAnEvent()
    {
        return currentlySelectedDestination != Vector3.down && CharacterIsInDestinationRange != null;
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
        if (CharacterMoved != null)// && (!StaticObjects.OnlineMode || character.PhotonView.isMine))
        {
            CharacterMoved();
        }
    }
}
