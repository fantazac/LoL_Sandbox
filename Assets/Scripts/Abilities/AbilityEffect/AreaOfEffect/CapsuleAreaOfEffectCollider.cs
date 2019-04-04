using System.Collections.Generic;
using UnityEngine;

public class CapsuleAreaOfEffectCollider : MonoBehaviour, AreaOfEffectCollider
{
    private float radius;

    private void Awake()
    {
        radius = transform.localScale.x * 0.5f;
    }
    
    public IEnumerable<Collider> GetCollidersInAreaOfEffect()
    {
        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        Vector3 highestPosition = groundPosition + Vector3.up * 5;

        return Physics.OverlapCapsule(groundPosition, highestPosition, radius);
    }
}
