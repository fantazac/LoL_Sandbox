using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundTargetedAoE : GroundTargeted
{
    protected string areaOfEffectPrefabPath;
    protected GameObject areaOfEffectPrefab;

    protected override void LoadPrefabs()
    {
        areaOfEffectPrefab = Resources.Load<GameObject>(areaOfEffectPrefabPath);
    }

    /*public override void UseAbility(Vector3 destination)
    {
        if (Vector3.Distance(destination, transform.position) <= range)
        {
            base.UseAbility(destination);
        }
        else
        {
            character.CharacterMovement.SetMoveTowardsPoint(destination, range);
            character.CharacterMovement.CharacterIsInRange += base.UseAbility;//TODO: MAKE SURE IT'S THE RIGHT USEABILITY
        }
    }*/
}
