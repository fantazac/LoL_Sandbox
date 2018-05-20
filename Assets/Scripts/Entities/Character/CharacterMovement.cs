using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    private string movementCapsulePrefabPath;
    private GameObject movementCapsulePrefab;

    private Vector3 currentlySelectedDestination;
    private Entity currentlySelectedTarget;
    private Entity currentlySelectedBasicAttackTarget;

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
        StopAllMovement();//TODO: If you are rooted and you spam click, it cancels your auto-attack but instantly sets one up again, is it intended in LoL?
        currentlySelectedDestination = destination;
        if (range > 0)
        {
            StartCoroutine(MoveTowardsPoint(destination, range));
        }
        else
        {
            StartCoroutine(MoveTowardsPoint(destination));
        }
        character.CharacterOrientation.RotateCharacter(destination);
    }

    private IEnumerator MoveTowardsPoint(Vector3 destination)
    {
        character.CharacterAutoAttack.EnableAutoAttack();

        while (transform.position != destination)
        {
            if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement() && character.EntityStatusManager.CanUseMovement())
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * character.EntityStats.MovementSpeed.GetTotal());

                NotifyCharacterMoved();
            }

            yield return null;
        }

        currentlySelectedDestination = Vector3.down;
    }

    private IEnumerator MoveTowardsPoint(Vector3 destination, float range)
    {
        character.CharacterAutoAttack.StopAutoAttack();

        while (Vector3.Distance(destination, transform.position) > range)
        {
            if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement() && character.EntityStatusManager.CanUseMovement())
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * character.EntityStats.MovementSpeed.GetTotal());

                NotifyCharacterMoved();
            }

            yield return null;
        }

        if (CharacterIsInDestinationRange != null)
        {
            CharacterIsInDestinationRange(destination);
        }

        character.CharacterAutoAttack.EnableAutoAttack();
        currentlySelectedDestination = Vector3.down;
    }

    public void SetCharacterIsInRangeEventForBasicAttack()//Lucian Q -> Lucian W: This cancels the Q cast but continues movement post-W to auto the same target
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

    public void SetMoveTowardsTarget(Entity target, float range, bool isBasicAttack)
    {
        StopAllMovement();
        if (isBasicAttack)
        {
            currentlySelectedBasicAttackTarget = target;
            character.EntityBasicAttack.SetupBasicAttack(target, true);
        }
        else
        {
            currentlySelectedTarget = target;
        }
        StartCoroutine(MoveTowardsTarget(target, range, isBasicAttack));
    }

    private IEnumerator MoveTowardsTarget(Entity target, float range, bool isBasicAttack)
    {
        if (target != null && Vector3.Distance(target.transform.position, transform.position) > range)
        {
            character.CharacterAutoAttack.StopAutoAttack();

            Transform targetTransform = target.transform;

            character.CharacterOrientation.RotateCharacterUntilReachedTarget(targetTransform, isBasicAttack);

            while (targetTransform != null && Vector3.Distance(targetTransform.position, transform.position) > range)
            {
                if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement())
                {
                    if (character.EntityStatusManager.CanUseMovement())
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * character.EntityStats.MovementSpeed.GetTotal());

                        NotifyCharacterMoved();
                    }
                }

                yield return null;
            }

            character.CharacterAutoAttack.EnableAutoAttack();
            character.CharacterOrientation.StopRotation();
        }

        if (target != null)
        {
            if (isBasicAttack && (character.CharacterAbilityManager.IsUsingAbilityPreventingBasicAttacks() || !character.EntityStatusManager.CanUseBasicAttacks()))//checks is disarmed
            {
                while (character.CharacterAbilityManager.IsUsingAbilityPreventingBasicAttacks() || !character.EntityStatusManager.CanUseBasicAttacks())
                {
                    yield return null;
                }

                SetMoveTowardsTarget(target, range, true);
            }

            if (CharacterIsInTargetRange != null)
            {
                CharacterIsInTargetRange(target);
            }

            currentlySelectedTarget = null;
            currentlySelectedBasicAttackTarget = null;
        }
        else
        {
            StopAllMovement();
            character.CharacterAutoAttack.EnableAutoAttack();
        }
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
        StopAllCoroutines();
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

    public bool IsWalkingTowardsPosition()
    {
        return currentlySelectedDestination != Vector3.down && CharacterIsInDestinationRange == null;
    }

    public void NotifyCharacterMoved()
    {
        if (CharacterMoved != null)// && (!StaticObjects.OnlineMode || character.PhotonView.isMine))
        {
            CharacterMoved();
        }
    }
}
