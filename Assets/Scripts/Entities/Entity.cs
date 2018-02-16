using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityType EntityType { get; protected set; }
    public EntityTeam Team { get; protected set; }

    public int EntityId { get; protected set; }

    public EntityBasicAttack EntityBasicAttack { get; private set; }
    public EntityBasicAttackCycle EntityBasicAttackCycle { get; private set; }
    public EntityBuffManager EntityBuffManager { get; private set; }
    public EntityStats EntityStats { get; private set; }

    public PhotonView PhotonView { get; private set; }

    protected virtual void Awake()
    {
        InitEntityProperties();
    }

    protected virtual void Start() { }

    protected virtual void InitEntityProperties()
    {
        EntityBasicAttack = GetComponent<EntityBasicAttack>();
        EntityBasicAttackCycle = GetComponent<EntityBasicAttackCycle>();
        EntityBuffManager = GetComponent<EntityBuffManager>();
        EntityStats = GetComponent<EntityStats>();

        PhotonView = GetComponent<PhotonView>();
    }
}
