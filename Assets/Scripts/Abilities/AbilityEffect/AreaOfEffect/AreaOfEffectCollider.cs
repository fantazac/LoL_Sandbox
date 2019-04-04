using System.Collections.Generic;
using UnityEngine;

public interface AreaOfEffectCollider
{
    IEnumerable<Collider> GetCollidersInAreaOfEffect();
}
