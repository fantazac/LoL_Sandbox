using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject movementCapsule;

    private Vector3 destination;
    private Entity target;
    private RaycastHit hit;

    private Character character;

    [HideInInspector]
    public Vector3 spawnPoint;

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
            character.CharacterInput.OnRightClick += PressedRightClick;
            character.CharacterInput.OnPressedS += StopMovement;
        }

        CharacterHeightOffset = Vector3.up * transform.position.y;
    }

    public void UnsubscribeCameraEvent()
    {
        CharacterMoved = null;
    }

    private void PressedRightClick(Vector3 mousePosition)
    {
        if (MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit))
        {
            Instantiate(movementCapsule, hit.point, Quaternion.identity);

            if (StaticObjects.OnlineMode)
            {
                SendToServer_Movement(hit.point + CharacterHeightOffset);
            }
            else if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement())
            {
                SetMoveTowardsPoint(hit.point + CharacterHeightOffset);
            }
            else
            {
                character.CharacterActionManager.SetPositionMovementInQueue(hit.point + CharacterHeightOffset);
            }
        }
    }

    private void SendToServer_Movement(Vector3 destination)
    {
        PhotonNetwork.RemoveRPCs(character.PhotonView);//if using AllBufferedViaServer somewhere else, this needs to change
        character.PhotonView.RPC("ReceiveFromServer_Movement", PhotonTargets.AllBufferedViaServer, destination);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement(Vector3 destination)
    {
        if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement())
        {
            SetMoveTowardsPoint(destination);
        }
        else
        {
            character.CharacterActionManager.SetPositionMovementInQueue(destination);
        }
    }

    public void SetMoveTowardsPoint(Vector3 destination)
    {
        StopAllCoroutines();
        CharacterIsInRange = null;
        this.destination = destination;
        target = null;
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
        this.destination = Vector3.zero;
    }

    public void SetMoveTowardsPointIfMovingTowardsTarget()
    {
        if (target != null)
        {
            SetMoveTowardsPointWithRange(target.transform.position, character.CharacterStatsController.GetCurrentAttackRange());// TODO : Maybe its 125 range (base value for all units)?
        }
    }

    private void SetMoveTowardsPointWithRange(Vector3 destination, float range)
    {
        StopAllCoroutines();
        CharacterIsInRange = null;
        this.destination = Vector3.zero; // Should not resume movement after an ability cast (ex. Lucian Q -> Lucian R)
        target = null;
        StartCoroutine(MoveTowardsPointWithRange(destination, range));
        //character.CharacterOrientation.RotateCharacter(destination); //Should not have to rotate
    }

    private IEnumerator MoveTowardsPointWithRange(Vector3 destination, float range)
    {
        while (Vector3.Distance(destination, transform.position) > range)
        {
            if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement())
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * character.CharacterStatsController.GetCurrentMovementSpeed());

                NotifyCharacterMoved();
            }

            yield return null;
        }
    }

    public void SetMoveTowardsTarget(Entity target, float range)
    {
        StopAllCoroutines();
        CharacterIsInRange = null;
        destination = Vector3.zero;
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
        if (CharacterIsInRange != null)
        {
            CharacterIsInRange(target);
        }
        this.target = null;
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
            character.CharacterActionManager.ResetBufferedAction();
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
        character.CharacterActionManager.ResetBufferedAction();
    }

    private void StopAllMovement()
    {
        destination = Vector3.zero;
        target = null;
        StopAllCoroutines();
        character.CharacterOrientation.StopAllCoroutines();
    }

    public void StopAllMovement(Ability ability)
    {
        if (ability.CanStopMovement)
        {
            if (!(ability is UnitTargeted))
            {
                if (destination != Vector3.zero)
                {
                    character.CharacterActionManager.SetPositionMovementInQueue(destination);
                }
                else if (target != null)
                {
                    character.CharacterActionManager.SetUnitMovementInQueue(target);
                }
            }
            StopAllMovement();
        }
    }

    public void NotifyCharacterMoved()
    {
        if (CharacterMoved != null && (!StaticObjects.OnlineMode || character.PhotonView.isMine))
        {
            CharacterMoved();
        }
    }
}
