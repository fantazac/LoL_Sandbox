﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : Entity
{
    protected bool sentConnectionInfoRequest = false;

    protected string characterPortraitPath;

    public CharacterAbilityEffectsManager CharacterAbilityEffectsManager { get; private set; }
    public CharacterAbilityManager CharacterAbilityManager { get; protected set; }
    public CharacterBufferedAbilityManager CharacterBufferedAbilityManager { get; private set; }
    public CharacterInput CharacterInput { get; private set; }
    public CharacterLevelManager CharacterLevelManager { get; private set; }
    public CharacterMouseManager CharacterMouseManager { get; private set; }
    public CharacterMovement CharacterMovement { get; private set; }
    public CharacterOnHitEffectsManager CharacterOnHitEffectsManager { get; private set; }
    public CharacterOrientation CharacterOrientation { get; private set; }
    public CharacterStatsManager CharacterStatsManager { get; private set; }

    public AbilityLevelUpUIManager AbilityLevelUpUIManager { get; private set; }
    public AbilityTimeBarUIManager AbilityTimeBarUIManager { get; private set; }
    public AbilityUIManager AbilityUIManager { get; private set; }
    public BuffUIManager BuffUIManager { get; private set; }
    public BuffUIManager DebuffUIManager { get; private set; }
    public HealthBarManager HealthBarManager { get; private set; }
    public LevelUIManager LevelUIManager { get; private set; }

    protected override void Start()
    {
        if ((!StaticObjects.OnlineMode && EntityId == 0) || PhotonView.isMine)
        {
            AbilityLevelUpUIManager = transform.parent.GetComponentInChildren<AbilityLevelUpUIManager>();
            AbilityTimeBarUIManager = transform.parent.GetComponentInChildren<AbilityTimeBarUIManager>();
            AbilityUIManager = transform.parent.GetComponentInChildren<AbilityUIManager>();
            BuffUIManager[] buffUIManagers = transform.parent.GetComponentsInChildren<BuffUIManager>();
            BuffUIManager = buffUIManagers[0];
            DebuffUIManager = buffUIManagers[1];
            EntityBuffManager.SetUIManagers(BuffUIManager, DebuffUIManager);
            HealthBarManager = transform.parent.GetComponentInChildren<HealthBarManager>();
            LevelUIManager = transform.parent.GetComponentInChildren<LevelUIManager>();
            LevelUIManager.SetPortraitSprite(Resources.Load<Sprite>(characterPortraitPath));
            LevelUIManager.SetLevel(CharacterLevelManager.Level);
        }

        if (StaticObjects.Character && StaticObjects.Character.HealthBarManager)
        {
            StaticObjects.Character.HealthBarManager.SetupHealthBarForCharacter(this);
        }

        EntityType = EntityType.CHARACTER;

        base.Start();
    }

    protected override void InitEntityProperties()
    {
        base.InitEntityProperties();

        SetCharacterSpecificScripts();

        if (!(this is Dummy))
        {
            CharacterAbilityEffectsManager = gameObject.AddComponent<CharacterAbilityEffectsManager>();
            CharacterBufferedAbilityManager = gameObject.AddComponent<CharacterBufferedAbilityManager>();
            CharacterLevelManager = gameObject.AddComponent<CharacterLevelManager>();
            CharacterMovement = gameObject.AddComponent<CharacterMovement>();
            CharacterOnHitEffectsManager = gameObject.AddComponent<CharacterOnHitEffectsManager>();
            CharacterOrientation = gameObject.AddComponent<CharacterOrientation>();
            CharacterStatsManager = gameObject.AddComponent<CharacterStatsManager>();
        }
        if ((!StaticObjects.OnlineMode && !(this is Dummy)) || PhotonView.isMine)
        {
            CharacterInput = gameObject.AddComponent<CharacterInput>();
            CharacterMouseManager = gameObject.AddComponent<CharacterMouseManager>();
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
    }

    protected virtual void SetCharacterSpecificScripts() { }

    protected void OnDestroy()
    {
        RemoveHealthBar();
    }

    public void RemoveHealthBar()
    {
        if (StaticObjects.Character && StaticObjects.Character.HealthBarManager)
        {
            StaticObjects.Character.HealthBarManager.RemoveHealthBarOfDeletedCharacter(this);
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfo(Vector3 position, Quaternion rotation, EntityTeam team, int characterId, int characterLevel, int[] characterAbilityLevels)
    {
        if (sentConnectionInfoRequest)
        {
            sentConnectionInfoRequest = false;
            transform.position = position;
            transform.rotation = rotation;
            Team = team;
            EntityId = characterId;
            CharacterLevelManager.SetLevelFromLoad(characterLevel);
            CharacterAbilityManager.SetAbilityLevelsFromLoad(characterAbilityLevels);
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfoRequest()
    {
        if (PhotonView.isMine)
        {
            PhotonView.RPC("ReceiveFromServer_ConnectionInfo", PhotonTargets.Others, transform.position, transform.rotation, Team, EntityId, CharacterLevelManager.Level, CharacterAbilityManager.GetCharacterAbilityLevels());
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
