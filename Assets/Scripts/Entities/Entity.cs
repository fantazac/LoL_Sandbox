using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityType EntityType { get; protected set; }
    public EntityTeam Team { get; protected set; }

    protected virtual void Start()
    {

    }
}
