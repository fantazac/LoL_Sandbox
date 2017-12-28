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

    private Vector3 characterHeightOffset;

    private TerrainCollider terrain;

    public delegate void PlayerMovedHandler();
    public event PlayerMovedHandler CharacterMoved;

    protected override void Start()
    {
        CharacterInput.OnRightClick += PressedRightClick;
        CharacterInput.OnPressedS += StopMovement;

        terrain = StaticObjects.Terrain;
        characterHeightOffset = Vector3.up * Character.transform.position.y;

        base.Start();
    }

    private void PressedRightClick(Vector3 mousePosition)
    {
        if (terrain.Raycast(GetRay(mousePosition), out hit, Mathf.Infinity))
        {
            Instantiate(movementCapsule, hit.point, new Quaternion());

            if (StaticObjects.OnlineMode)
            {
                SendToServer_Movement(hit.point + characterHeightOffset);
            }
            else
            {
                SetMoveTowardsPoint(hit.point + characterHeightOffset);
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

    private void SetMoveTowardsPoint(Vector3 destination)
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

    private Ray GetRay(Vector3 mousePosition)
    {
        return StaticObjects.CharacterCamera.ScreenPointToRay(mousePosition);
    }

    private void NotifyCharacterMoved()
    {
        if ((StaticObjects.OnlineMode && PhotonView.isMine) || !StaticObjects.OnlineMode)
        {
            CharacterMoved();
        }
    }
}
