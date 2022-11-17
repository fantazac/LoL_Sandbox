using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public abstract class Champion : Character
{
    private bool sentConnectionInfoRequest;

    public AbilityEffectsManager AbilityEffectsManager { get; private set; }
    public AbilityManager AbilityManager { get; protected set; }
    public AutoAttackManager AutoAttackManager { get; private set; }
    public BufferedAbilityManager BufferedAbilityManager { get; private set; }
    public ChampionMovementManager ChampionMovementManager { get; private set; }
    public InputManager InputManager { get; private set; }
    public LevelManager LevelManager { get; private set; }
    public MouseManager MouseManager { get; private set; }
    public OnAttackEffectsManager OnAttackEffectsManager { get; private set; }
    public OnHitEffectsManager OnHitEffectsManager { get; private set; }
    public OrientationManager OrientationManager { get; private set; }

    public AbilityLevelUpUIManager AbilityLevelUpUIManager { get; private set; }
    public AbilityTimeBarUIManager AbilityTimeBarUIManager { get; private set; }
    public AbilityUIManager AbilityUIManager { get; private set; }
    public HealthBarManager HealthBarManager { get; private set; }
    public HealthUIManager HealthUIManager { get; private set; }
    public InfoUIManager InfoUIManager { get; private set; }
    public LevelUIManager LevelUIManager { get; private set; }
    public StatsUIManager StatsUIManager { get; private set; }

    protected override void InitUnitProperties()
    {
        base.InitUnitProperties();

        AbilityEffectsManager = gameObject.AddComponent<AbilityEffectsManager>();
        AutoAttackManager = gameObject.AddComponent<AutoAttackManager>();
        BufferedAbilityManager = gameObject.AddComponent<BufferedAbilityManager>();
        ChampionMovementManager = gameObject.AddComponent<ChampionMovementManager>();
        LevelManager = gameObject.AddComponent<LevelManager>();
        OnAttackEffectsManager = gameObject.AddComponent<OnAttackEffectsManager>();
        OnHitEffectsManager = gameObject.AddComponent<OnHitEffectsManager>();
        OrientationManager = gameObject.AddComponent<OrientationManager>();
        StatusManager = gameObject.AddComponent<ChampionStatusManager>();

        if (IsLocalChampion())
        {
            InputManager = gameObject.AddComponent<InputManager>();
            MouseManager = gameObject.AddComponent<MouseManager>();
        }

        if (StaticObjects.OnlineMode && PhotonView.IsMine)
        {
            SetTeamAndID(PhotonNetwork.LocalPlayer.GetPhotonTeam().Code == 1 ? Team.BLUE : Team.RED, PhotonNetwork.LocalPlayer.UserId);
            SendToServer_TeamAndID();
        }
        else if (!StaticObjects.OnlineMode)
        {
            SetTeamAndID(Team.BLUE, "Local");
        }
    }

    protected override void Start()
    {
        if (IsLocalChampion())
        {
            AbilityLevelUpUIManager = transform.parent.GetComponentInChildren<AbilityLevelUpUIManager>();
            AbilityTimeBarUIManager = transform.parent.GetComponentInChildren<AbilityTimeBarUIManager>();
            AbilityUIManager = transform.parent.GetComponentInChildren<AbilityUIManager>();
            BuffUIManager[] buffUIManagers = transform.parent.GetComponentsInChildren<BuffUIManager>();
            BuffManager.SetUIManagers(buffUIManagers[0], buffUIManagers[1]);
            HealthBarManager = transform.parent.GetComponentInChildren<HealthBarManager>();
            HealthUIManager = transform.parent.GetComponentInChildren<HealthUIManager>();
            HealthUIManager.SetHealthAndResource(this);
            InfoUIManager = transform.parent.GetComponentInChildren<InfoUIManager>();
            InfoUIManager.gameObject.SetActive(false);
            LevelUIManager = transform.parent.GetComponentInChildren<LevelUIManager>();
            LevelUIManager.SetPortraitSprite(PortraitSprite);
            LevelUIManager.SetLevel(LevelManager.Level);
            StatsUIManager = gameObject.AddComponent<StatsUIManager>();
        }

        base.Start();
    }

    public bool IsLocalChampion()
    {
        return !StaticObjects.OnlineMode || PhotonView.IsMine;
    }

    public void SendToServer_ConnectionInfoRequest()
    {
        sentConnectionInfoRequest = true;
        PhotonView.RPC(nameof(ReceiveFromServer_ConnectionInfoRequest), RpcTarget.Others);
    }

    [PunRPC]
    private void ReceiveFromServer_ConnectionInfoRequest()
    {
        if (PhotonView.IsMine)
        {
            PhotonView.RPC(nameof(ReceiveFromServer_ConnectionInfo), RpcTarget.Others, transform.position, transform.rotation, Team, ID, LevelManager.Level,
                AbilityManager.GetCharacterAbilityLevels());
        }
    }

    [PunRPC]
    private void ReceiveFromServer_ConnectionInfo(Vector3 position, Quaternion rotation, Team team, string characterId, int characterLevel, int[] characterAbilityLevels)
    {
        if (!sentConnectionInfoRequest) return;

        sentConnectionInfoRequest = false;
        transform.position = position;
        transform.rotation = rotation;
        SetTeamAndID(team, characterId);
        LevelManager.SetLevelFromLoad(characterLevel);
        AbilityManager.SetAbilityLevelsFromLoad(characterAbilityLevels);
    }

    private void SendToServer_TeamAndID()
    {
        PhotonView.RPC(nameof(ReceiveFromServer_TeamAndID), RpcTarget.Others, Team, ID);
    }

    [PunRPC]
    private void ReceiveFromServer_TeamAndID(Team team, string characterId)
    {
        SetTeamAndID(team, characterId);
    }

    protected override MovementManager GetMovementManager()
    {
        return ChampionMovementManager;
    }

    protected override void SetTeamAndID(Team team, string id)
    {
        base.SetTeamAndID(team, id);

        AbilityManager.SeAffectedTeamsForAbilities(team);
        AutoAttackManager.SetAffectedTeams(team);
    }
}
