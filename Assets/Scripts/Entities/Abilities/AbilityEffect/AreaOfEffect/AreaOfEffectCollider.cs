using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectCollider : MonoBehaviour
{
    public delegate void OnTriggerEnterInChildHandler(Collider collider);
    public event OnTriggerEnterInChildHandler OnTriggerEnterInChild;

    protected void OnTriggerEnter(Collider collider)
    {
        OnTriggerEnterInChild(collider);
    }
}
