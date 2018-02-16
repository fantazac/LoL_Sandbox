using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : Entity
{
    private bool sentConnectionInfoRequest = false;

    public CharacterAbilityManager CharacterAbilityManager { get; private set; }
    public CharacterActionManager CharacterActionManager { get; private set; }
    public CharacterInput CharacterInput { get; private set; }
    public CharacterMouseManager CharacterMouseManager { get; private set; }
    public CharacterMovement CharacterMovement { get; private set; }
    public CharacterOrientation CharacterOrientation { get; private set; }

    public AbilityUIManager AbilityUIManager { get; private set; }
    public BuffUIManager BuffUIManager { get; private set; }
    public BuffUIManager DebuffUIManager { get; private set; }

    public delegate void OnConnectionInfoReceivedHandler(Character character);
    public event OnConnectionInfoReceivedHandler OnConnectionInfoReceived;

    protected override void Start()
    {
        if ((!StaticObjects.OnlineMode && EntityId == 0) || PhotonView.isMine)
        {
            AbilityUIManager = transform.parent.GetComponentInChildren<AbilityUIManager>();
            BuffUIManager[] buffUIManagers = transform.parent.GetComponentsInChildren<BuffUIManager>();
            BuffUIManager = buffUIManagers[0];
            DebuffUIManager = buffUIManagers[1];
            EntityBuffManager.SetUIManagers(BuffUIManager, DebuffUIManager);
        }
        if (StaticObjects.OnlineMode && PhotonView.isMine)
        {
            if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
            {
                Team = EntityTeam.BLUE;
            }
            else
            {
                Team = EntityTeam.RED;
            }
            EntityId = PhotonNetwork.player.ID;
            SendToServer_TeamAndID();
        }

        EntityType = EntityType.CHARACTER;

        base.Start();
    }

    protected override void InitEntityProperties()
    {
        base.InitEntityProperties();

        CharacterAbilityManager = GetComponent<CharacterAbilityManager>();
        CharacterActionManager = GetComponent<CharacterActionManager>();
        CharacterInput = GetComponent<CharacterInput>();
        CharacterMouseManager = GetComponent<CharacterMouseManager>();
        CharacterMovement = GetComponent<CharacterMovement>();
        CharacterOrientation = GetComponent<CharacterOrientation>();
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfo(Vector3 position, Quaternion rotation, EntityTeam team, int characterId)
    {
        if (sentConnectionInfoRequest)
        {
            sentConnectionInfoRequest = false;
            transform.position = position;
            transform.rotation = rotation;
            Team = team;
            EntityId = characterId;
            OnConnectionInfoReceived(this);
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfoRequest()
    {
        if (PhotonView.isMine)
        {
            PhotonView.RPC("ReceiveFromServer_ConnectionInfo", PhotonTargets.Others, transform.position, transform.rotation, Team, EntityId);
        }
    }

    public void SendToServer_ConnectionInfoRequest()
    {
        sentConnectionInfoRequest = true;
        PhotonView.RPC("ReceiveFromServer_ConnectionInfoRequest", PhotonTargets.Others);
    }

    public void SendToServer_TeamAndID()
    {
        PhotonView.RPC("ReceiveFromServer_TeamAndID", PhotonTargets.Others, Team, EntityId);
    }

    [PunRPC]
    protected void ReceiveFromServer_TeamAndID(EntityTeam team, int characterId)
    {
        Team = team;
        EntityId = characterId;
    }

    //This was in CharacterBase, no idea if useful, keeping it here in case it is.
    public virtual void SerializeState(PhotonStream stream, PhotonMessageInfo info) { }
}
