using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityType EntityType { get; protected set; }
    public EntityTeam Team { get; protected set; }

    public int EntityId { get; protected set; }

    public BasicAttackManager BasicAttackManager { get; protected set; }
    public BuffManager BuffManager { get; private set; }
    public DisplacementManager DisplacementManager { get; private set; }
    public EffectSourceManager EffectSourceManager { get; private set; }
    public ForcedActionManager ForcedActionManager { get; private set; }
    public GameObject HitboxObject { get; private set; }
    public GameObject ModelObject { get; private set; }
    public ShieldManager ShieldManager { get; private set; }
    public StatsManager StatsManager { get; protected set; }
    public StatusManager StatusManager { get; private set; }

    public PhotonView PhotonView { get; private set; }

    protected virtual void Awake()
    {
        InitEntityProperties();
    }

    protected virtual void Start() { }

    protected virtual void InitEntityProperties()
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
}
