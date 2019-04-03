using UnityEngine;

public class BoxAreaOfEffectCollider : MonoBehaviour, AreaOfEffectCollider
{
    public Collider[] GetCollidersInAreaOfEffect()
    {
        return Physics.OverlapBox(transform.position, transform.localScale * 0.5f, transform.rotation);
    }
}
