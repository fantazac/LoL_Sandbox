using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityType EntityType { get; protected set; }
    public EntityTeam Team { get; protected set; }

    public int EntityId { get; protected set; }

    public EntityBuffManager EntityBuffManager { get; private set; }
    public EntityStats EntityStats { get; private set; }

    protected virtual void Awake()
    {
        InitEntityProperties();
    }

    protected virtual void Start() { }

    protected virtual void InitEntityProperties()
    {
        EntityBuffManager = GetComponent<EntityBuffManager>();
        EntityStats = GetComponent<EntityStats>();
}
}
