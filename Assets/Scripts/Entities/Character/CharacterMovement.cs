using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement : CharacterBase
{
    [SerializeField]
    private GameObject movementCapsule;

    private RaycastHit hit;

    [HideInInspector]
    public Vector3 spawnPoint;

    public Vector3 CharacterHeightOffset { get; private set; }

    public delegate void PlayerMovedHandler();
    public event PlayerMovedHandler CharacterMoved;

    public delegate void PlayerIsInRangeHandler(Vector3 targetPosition);
    public event PlayerIsInRangeHandler CharacterIsInRange;

    protected override void Start()
    {
        CharacterInput.OnRightClick += PressedRightClick;
        CharacterInput.OnPressedS += StopMovement;

        CharacterHeightOffset = Vector3.up * transform.position.y;

        base.Start();
    }

    private void PressedRightClick(Vector3 mousePosition)
    {
        if (!CharacterAbilityManager.IsUsingAbilityPreventingMovement() && MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit))
        {
            CharacterIsInRange = null;
            Instantiate(movementCapsule, hit.point, new Quaternion());

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
        PhotonNetwork.RemoveRPCs(PhotonView);//if using AllBufferedViaServer somewhere else, this needs to change
        PhotonView.RPC("ReceiveFromServer_Movement", PhotonTargets.AllBufferedViaServer, destination);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement(Vector3 destination)
    {
        SetMoveTowardsPoint(destination);
    }

    public void SetMoveTowardsPoint(Vector3 destination)
    {
        StopAllCoroutines();
        StartCoroutine(MoveTowardsPoint(destination));
        CharacterOrientation.RotateCharacter(destination);
    }

    private IEnumerator MoveTowardsPoint(Vector3 wherePlayerClickedToMove)
    {
        while (transform.position != wherePlayerClickedToMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, wherePlayerClickedToMove, Time.deltaTime * CharacterStatsController.GetCurrentMovementSpeed());

            NotifyCharacterMoved();

            yield return null;
        }
    }

    public void SetMoveTowardsTarget(Transform target, float range)
    {
        StopAllCoroutines();
        StartCoroutine(MoveTowardsTarget(target, range));
        CharacterOrientation.RotateCharacterUntilReachedTarget(target);
    }

    private IEnumerator MoveTowardsTarget(Transform target, float range)
    {
        while (Vector3.Distance(target.position, transform.position) > range)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * CharacterStatsController.GetCurrentMovementSpeed());

            NotifyCharacterMoved();

            yield return null;
        }
        if(CharacterIsInRange != null)
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
        PhotonView.RPC("ReceiveFromServer_StopMovement", PhotonTargets.AllViaServer);
    }

    [PunRPC]
    private void ReceiveFromServer_StopMovement()
    {
        StopAllMovement();
    }

    private void StopAllMovement()
    {
        StopAllCoroutines();
        CharacterOrientation.StopAllCoroutines();
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
        if (!StaticObjects.OnlineMode || PhotonView.isMine)
        {
            CharacterMoved();
        }
    }
}
