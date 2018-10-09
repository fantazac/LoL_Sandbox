using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityType EntityType { get; protected set; }
    public EntityTeam Team { get; protected set; }

    public int EntityId { get; protected set; }

    public EntityBasicAttack EntityBasicAttack { get; protected set; }
    public EntityBasicAttackCycle EntityBasicAttackCycle { get; private set; }
    public EntityBuffManager EntityBuffManager { get; private set; }
    public EntityStats EntityStats { get; protected set; }
    public EntityStatusManager EntityStatusManager { get; private set; }
    public EntityStatusMovementManager EntityStatusMovementManager { get; private set; }

    public PhotonView PhotonView { get; private set; }

    protected virtual void Awake()
    {
        InitEntityProperties();
    }

    protected virtual void Start() { }

    protected virtual void InitEntityProperties()
    {
        if (!(this is Dummy))
        {
            EntityBasicAttackCycle = gameObject.AddComponent<EntityBasicAttackCycle>();
        }
        EntityBuffManager = gameObject.AddComponent<EntityBuffManager>();
        EntityStatusManager = gameObject.AddComponent<EntityStatusManager>();
        EntityStatusMovementManager = gameObject.AddComponent<EntityStatusMovementManager>();

        PhotonView = GetComponent<PhotonView>();
        gameObject.AddComponent<MouseEvent>();
    }
}
