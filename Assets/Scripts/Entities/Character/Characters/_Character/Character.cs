using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : CharacterBase
{
    public EntityTeam team;

    private bool sentConnectionInfoRequest = false;

    public delegate void OnConnectionInfoReceivedHandler(Character character);
    public event OnConnectionInfoReceivedHandler OnConnectionInfoReceived;

    protected override void Start()
    {
        if (StaticObjects.OnlineMode && PhotonView.isMine)
        {
            if(PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
            {
                team = EntityTeam.BLUE;
            }
            else
            {
                team = EntityTeam.RED;
            }
            SendToServer_Team();
        }
        base.Start();
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfo(Vector3 position, Quaternion rotation, EntityTeam team)
    {
        if (sentConnectionInfoRequest)
        {
            sentConnectionInfoRequest = false;
            transform.position = position;
            transform.rotation = rotation;
            this.team = team;
            OnConnectionInfoReceived(this);
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfoRequest()
    {
        if (PhotonView.isMine)
        {
            PhotonView.RPC("ReceiveFromServer_ConnectionInfo", PhotonTargets.Others, transform.position, transform.rotation, team);
        }
    }

    public void SendToServer_ConnectionInfoRequest()
    {
        sentConnectionInfoRequest = true;
        PhotonView.RPC("ReceiveFromServer_ConnectionInfoRequest", PhotonTargets.Others);
    }

    public void SendToServer_Team()
    {
        PhotonView.RPC("ReceiveFromServer_Team", PhotonTargets.Others, team);
    }

    [PunRPC]
    protected void ReceiveFromServer_Team(EntityTeam team)
    {
        this.team = team;
    }
}
