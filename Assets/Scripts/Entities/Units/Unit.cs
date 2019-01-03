using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Entity
{
    public Team Team { get; private set; }

    public int ID { get; private set; }

    public BuffManager BuffManager { get; private set; }
    public DisplacementManager DisplacementManager { get; private set; }
    public EffectSourceManager EffectSourceManager { get; private set; }
    public ForcedActionManager ForcedActionManager { get; private set; }
    public GameObject HitboxObject { get; private set; }
    public GameObject ModelObject { get; private set; }
    public ShieldManager ShieldManager { get; private set; }

    public BasicAttack BasicAttack { get; protected set; }
    public MovementManager MovementManager { get { return GetMovementManager(); } }
    public StatsManager StatsManager { get; protected set; }
    public StatusManager StatusManager { get; protected set; }

    public PhotonView PhotonView { get; private set; }

    protected virtual void Awake()
    {
        InitUnitProperties();
    }

    protected virtual void Start() { }

    protected virtual void InitUnitProperties()
    {
        BuffManager = gameObject.AddComponent<BuffManager>();
        DisplacementManager = gameObject.AddComponent<DisplacementManager>();
        EffectSourceManager = gameObject.AddComponent<EffectSourceManager>();
        ForcedActionManager = gameObject.AddComponent<ForcedActionManager>();
        HitboxObject = gameObject.GetComponentInChildren<Collider>().gameObject;
        ModelObject = gameObject.GetComponentInChildren<MeshRenderer>().gameObject;
        ShieldManager = gameObject.AddComponent<ShieldManager>();

        PhotonView = GetComponent<PhotonView>();
        HitboxObject.AddComponent<MouseEvent>();
    }

    protected abstract MovementManager GetMovementManager();

    public bool IsTargetable(List<Type> affectedUnitTypes, List<Team> affectedTeams)
    {
        foreach (Team affectedTeam in affectedTeams)
        {
            if (affectedTeam == Team)
            {
                foreach (Type affectedUnitType in affectedUnitTypes)
                {
                    if (IsSameTypeOrSubtype(GetType(), affectedUnitType))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool IsSameTypeOrSubtype(Type unitTypeToVerify, Type affectedUnitType)
    {
        return unitTypeToVerify.IsSubclassOf(affectedUnitType) || unitTypeToVerify == affectedUnitType;
    }

    protected virtual void SetTeamAndID(Team team, int id)
    {
        Team = team;
        ID = id;
        StaticObjects.Units.Add(id, this);

        if (BasicAttack)
        {
            BasicAttack.SetAffectedTeams(team);
        }
    }
}
