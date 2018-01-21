using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject movementCapsule;

    private RaycastHit hit;

    private Character character;

    [HideInInspector]
    public Vector3 spawnPoint;

    public Vector3 CharacterHeightOffset { get; private set; }

    public delegate void PlayerMovedHandler();
    public event PlayerMovedHandler CharacterMoved;

    public delegate void PlayerIsInRangeHandler(Vector3 targetPosition);
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
        if (!character.CharacterAbilityManager.IsUsingAbilityPreventingMovement() && MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit))
        {
            Instantiate(movementCapsule, hit.point, Quaternion.identity);

            if (StaticObjects.OnlineMode)
            {
                SendToServer_Movement(hit.point + CharacterHeightOffset);
            }
            else
            {
                SetMoveTowardsPoint(hit.point + CharacterHeightOffset);
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
        SetMoveTowardsPoint(destination);
    }

    public void SetMoveTowardsPoint(Vector3 destination)
    {
        StopAllCoroutines();
        CharacterIsInRange = null;
        StartCoroutine(MoveTowardsPoint(destination));
        character.CharacterOrientation.RotateCharacter(destination);
    }

    private IEnumerator MoveTowardsPoint(Vector3 wherePlayerClickedToMove)
    {
        while (transform.position != wherePlayerClickedToMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, wherePlayerClickedToMove, Time.deltaTime * character.CharacterStatsController.GetCurrentMovementSpeed());

            NotifyCharacterMoved();

            yield return null;
        }
    }

    public void SetMoveTowardsTarget(Transform target, float range)
    {
        StopAllCoroutines();
        CharacterIsInRange = null;
        StartCoroutine(MoveTowardsTarget(target, range));
        character.CharacterOrientation.RotateCharacterUntilReachedTarget(target);
    }

    private IEnumerator MoveTowardsTarget(Transform target, float range)
    {
        while (Vector3.Distance(target.position, transform.position) > range)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * character.CharacterStatsController.GetCurrentMovementSpeed());

            NotifyCharacterMoved();

            yield return null;
        }
        if (CharacterIsInRange != null)
        {
            CharacterIsInRange(target.position);
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

    private void StopAllMovement()
    {
        StopAllCoroutines();
        character.CharacterOrientation.StopAllCoroutines();
    }

    public void StopAllMovement(Ability ability)
    {
        if (ability.CanStopMovement)
        {
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
