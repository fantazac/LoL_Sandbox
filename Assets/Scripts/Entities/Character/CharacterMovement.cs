using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject movementCapsule;

    private Vector3 destination;
    private Entity target;

    private Character character;

    public Vector3 CharacterHeightOffset { get; private set; }

    public delegate void PlayerMovedHandler();
    public event PlayerMovedHandler CharacterMoved;

    public delegate void PlayerIsInRangeHandler(Entity target);
    public event PlayerIsInRangeHandler CharacterIsInRange;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        if (!StaticObjects.OnlineMode || character.PhotonView.isMine)
        {
            character.CharacterInput.OnPressedS += StopMovement;
        }

        CharacterHeightOffset = Vector3.up * transform.position.y;
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
        Instantiate(movementCapsule, pointOnMap, Quaternion.identity);
    }

    public void PrepareMovementTowardsTarget(Entity target)
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Movement_Target(target, character.CharacterStatsController.GetCurrentAttackRange());
        }
        else
        {
            SetMoveTowardsTarget(target, character.CharacterStatsController.GetCurrentAttackRange());
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

    private void SendToServer_Movement_Target(Entity target, float range)
    {
        PhotonNetwork.RemoveRPCs(character.PhotonView);//if using AllBufferedViaServer somewhere else, this needs to change
        character.PhotonView.RPC("ReceiveFromServer_Movement_Target", PhotonTargets.AllBufferedViaServer, target.EntityId, target.EntityType, range);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement_Target(int entityId, EntityType entityType, float range)
    {
        SetMoveTowardsTarget(character.CharacterAbilityManager.FindTarget(entityId, entityType), range);
    }

    public void SetMoveTowardsPoint(Vector3 destination)
    {
        StopAllMovement();
        this.destination = destination;
        StartCoroutine(MoveTowardsPoint(destination));
        character.CharacterOrientation.RotateCharacter(destination);
    }

    private IEnumerator MoveTowardsPoint(Vector3 destination)
    {
        while (transform.position != destination)
        {
            if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement())
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * character.CharacterStatsController.GetCurrentMovementSpeed());

                NotifyCharacterMoved();
            }

            yield return null;
        }
        this.destination = Vector3.down;
    }

    public void SetCharacterIsInRangeEventForBasicAttack()
    {
        if (target != null)
        {
            CharacterIsInRange = null;
            //CharacterIsInRange += character.BasicAttack.InRange;
        }
    }

    public void SetMoveTowardsTarget(Entity target, float range)
    {
        StopAllMovement();
        this.target = target;
        StartCoroutine(MoveTowardsTarget(target, range));
        character.CharacterOrientation.RotateCharacterUntilReachedTarget(target.transform);
    }

    private IEnumerator MoveTowardsTarget(Entity target, float range)
    {
        Transform targetTransform = target.transform;
        while (Vector3.Distance(targetTransform.position, transform.position) > range)
        {
            if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement())
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * character.CharacterStatsController.GetCurrentMovementSpeed());

                NotifyCharacterMoved();
            }

            yield return null;
        }

        while (character.CharacterAbilityManager.IsUsingAbilityPreventingMovement())
        {
            yield return null;
        }

        if (CharacterIsInRange != null)
        {
            CharacterIsInRange(target);
        }
        this.target = null;
    }

    public void RotateCharacterIfMoving()
    {
        if (destination != Vector3.down)
        {
            character.CharacterOrientation.RotateCharacter(destination);
        }
        else if (target != null)
        {
            character.CharacterOrientation.RotateCharacterUntilReachedTarget(target.transform);
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
        StopAllMovement();
    }

    public void StopAllMovement()
    {
        character.CharacterActionManager.ResetBufferedAction();
        StopAllCoroutines();
        character.CharacterOrientation.StopAllRotation();
        CharacterIsInRange = null;
        destination = Vector3.down;
        target = null;
    }

    public void StopMovementTowardsPoint()
    {
        if(destination != Vector3.down)
        {
            StopAllMovement();
        }
    }

    public Entity GetTarget()
    {
        return target;
    }

    public void NotifyCharacterMoved()
    {
        if (CharacterMoved != null && (!StaticObjects.OnlineMode || character.PhotonView.isMine))
        {
            CharacterMoved();
        }
    }
}
