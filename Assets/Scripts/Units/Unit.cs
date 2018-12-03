using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public UnitType UnitType { get; protected set; }
    public Team Team { get; protected set; }//monsters do not have teams, maybe minions/characters/pets inherit from a class and monsters from another?

    public int ID { get; protected set; }

    public BuffManager BuffManager { get; private set; }
    public DisplacementManager DisplacementManager { get; private set; }
    public EffectSourceManager EffectSourceManager { get; private set; }
    public ForcedActionManager ForcedActionManager { get; private set; }
    public GameObject HitboxObject { get; private set; }
    public GameObject ModelObject { get; private set; }
    public ShieldManager ShieldManager { get; private set; }
    public StatusManager StatusManager { get; private set; }

    public BasicAttack BasicAttack { get; protected set; }
    public MovementManager MovementManager { get { return GetMovementManager(); } }
    public StatsManager StatsManager { get; protected set; }

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
        StatusManager = gameObject.AddComponent<StatusManager>();

        PhotonView = GetComponent<PhotonView>();
        HitboxObject.AddComponent<MouseEvent>();
    }

    protected abstract MovementManager GetMovementManager();
}
