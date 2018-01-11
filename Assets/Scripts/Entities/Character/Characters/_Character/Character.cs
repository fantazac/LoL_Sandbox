using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : CharacterBase
{
    public EntityTeam Team { get; set; }

    private bool sentConnectionInfoRequest = false;

    public delegate void OnConnectionInfoReceivedHandler(Character character);
    public event OnConnectionInfoReceivedHandler OnConnectionInfoReceived;

    protected Character()
    {
        if (StaticObjects.OnlineMode && PhotonView.isMine)
        {
            if(PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
            {
                Team = EntityTeam.BLUE;
            }
            else
            {
                Team = EntityTeam.RED;
            }
            SendToServer_Team();
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfo(Vector3 position, Quaternion rotation, EntityTeam team)
    {
        if (sentConnectionInfoRequest)
        {
            sentConnectionInfoRequest = false;
            transform.position = position;
            transform.rotation = rotation;
            Team = team;
            OnConnectionInfoReceived(this);
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfoRequest()
    {
        if (PhotonView.isMine)
        {
            PhotonView.RPC("ReceiveFromServer_ConnectionInfo", PhotonTargets.Others, transform.position, transform.rotation, Team);
        }
    }

    public void SendToServer_ConnectionInfoRequest()
    {
        sentConnectionInfoRequest = true;
        PhotonView.RPC("ReceiveFromServer_ConnectionInfoRequest", PhotonTargets.Others);
    }

    public void SendToServer_Team()
    {
        PhotonView.RPC("ReceiveFromServer_Team", PhotonTargets.Others, Team);
    }

    [PunRPC]
    protected void ReceiveFromServer_Team(EntityTeam team)
    {
        Team = team;
    }
}
