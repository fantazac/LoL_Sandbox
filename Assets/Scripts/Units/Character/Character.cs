using UnityEngine;

public abstract class Character : Unit
{
    protected bool sentConnectionInfoRequest = false;

    protected string characterPortraitPath;

    public AbilityEffectsManager AbilityEffectsManager { get; private set; }
    public AbilityManager AbilityManager { get; protected set; }
    public AutoAttackManager AutoAttackManager { get; private set; }
    public BufferedAbilityManager BufferedAbilityManager { get; private set; }
    public InputManager InputManager { get; private set; }
    public LevelManager LevelManager { get; private set; }
    public MouseManager MouseManager { get; private set; }
    public MovementManager MovementManager { get; private set; }
    public OnAttackEffectsManager OnAttackEffectsManager { get; private set; }
    public OnHitEffectsManager OnHitEffectsManager { get; private set; }
    public OrientationManager OrientationManager { get; private set; }

    public AbilityLevelUpUIManager AbilityLevelUpUIManager { get; private set; }
    public AbilityTimeBarUIManager AbilityTimeBarUIManager { get; private set; }
    public AbilityUIManager AbilityUIManager { get; private set; }
    public BuffUIManager BuffUIManager { get; private set; }
    public BuffUIManager DebuffUIManager { get; private set; }
    public HealthBarManager HealthBarManager { get; private set; }
    public LevelUIManager LevelUIManager { get; private set; }
    public StatsUIManager StatsUIManager { get; private set; }

    protected override void Start()
    {
        if (IsLocalCharacter())
        {
            AbilityLevelUpUIManager = transform.parent.GetComponentInChildren<AbilityLevelUpUIManager>();
            AbilityTimeBarUIManager = transform.parent.GetComponentInChildren<AbilityTimeBarUIManager>();
            AbilityUIManager = transform.parent.GetComponentInChildren<AbilityUIManager>();
            BuffUIManager[] buffUIManagers = transform.parent.GetComponentsInChildren<BuffUIManager>();
            BuffUIManager = buffUIManagers[0];
            DebuffUIManager = buffUIManagers[1];
            BuffManager.SetUIManagers(BuffUIManager, DebuffUIManager);
            HealthBarManager = transform.parent.GetComponentInChildren<HealthBarManager>();
            LevelUIManager = transform.parent.GetComponentInChildren<LevelUIManager>();
            LevelUIManager.SetPortraitSprite(Resources.Load<Sprite>(characterPortraitPath));
            LevelUIManager.SetLevel(LevelManager.Level);
            StatsUIManager = gameObject.AddComponent<StatsUIManager>();
        }

        if (StaticObjects.Character && StaticObjects.Character.HealthBarManager)
        {
            StaticObjects.Character.HealthBarManager.SetupHealthBarForCharacter(this);
        }

        UnitType = UnitType.CHARACTER;

        base.Start();
    }

    protected override void InitUnitProperties()
    {
        base.InitUnitProperties();

        SetCharacterSpecificScripts();

        if (!(this is Dummy))
        {
            AbilityEffectsManager = gameObject.AddComponent<AbilityEffectsManager>();
            AutoAttackManager = gameObject.AddComponent<AutoAttackManager>();
            BufferedAbilityManager = gameObject.AddComponent<BufferedAbilityManager>();
            LevelManager = gameObject.AddComponent<LevelManager>();
            MovementManager = gameObject.AddComponent<MovementManager>();
            OnAttackEffectsManager = gameObject.AddComponent<OnAttackEffectsManager>();
            OnHitEffectsManager = gameObject.AddComponent<OnHitEffectsManager>();
            OrientationManager = gameObject.AddComponent<OrientationManager>();
        }
        if (IsLocalCharacter())
        {
            InputManager = gameObject.AddComponent<InputManager>();
            MouseManager = gameObject.AddComponent<MouseManager>();
        }

        if (StaticObjects.OnlineMode && PhotonView.isMine)
        {
            if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
            {
                Team = Team.BLUE;
            }
            else
            {
                Team = Team.RED;
            }
            ID = PhotonNetwork.player.ID;
            SendToServer_TeamAndID();
        }
    }

    public bool IsLocalCharacter()
    {
        return (!StaticObjects.OnlineMode && !(this is Dummy)) || PhotonView.isMine;
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
    protected void ReceiveFromServer_ConnectionInfo(Vector3 position, Quaternion rotation, Team team, int characterId, int characterLevel, int[] characterAbilityLevels)
    {
        if (sentConnectionInfoRequest)
        {
            sentConnectionInfoRequest = false;
            transform.position = position;
            transform.rotation = rotation;
            Team = team;
            ID = characterId;
            LevelManager.SetLevelFromLoad(characterLevel);
            AbilityManager.SetAbilityLevelsFromLoad(characterAbilityLevels);
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_ConnectionInfoRequest()
    {
        if (PhotonView.isMine)
        {
            PhotonView.RPC("ReceiveFromServer_ConnectionInfo", PhotonTargets.Others, transform.position, transform.rotation, Team, ID, LevelManager.Level, AbilityManager.GetCharacterAbilityLevels());
        }
    }

    public void SendToServer_ConnectionInfoRequest()
    {
        sentConnectionInfoRequest = true;
        PhotonView.RPC("ReceiveFromServer_ConnectionInfoRequest", PhotonTargets.Others);
    }

    public void SendToServer_TeamAndID()
    {
        PhotonView.RPC("ReceiveFromServer_TeamAndID", PhotonTargets.Others, Team, ID);
    }

    [PunRPC]
    protected void ReceiveFromServer_TeamAndID(Team team, int characterId)
    {
        Team = team;
        ID = characterId;
    }

    //This was in CharacterBase, no idea if useful, keeping it here in case it is.
    public virtual void SerializeState(PhotonStream stream, PhotonMessageInfo info) { }
}
