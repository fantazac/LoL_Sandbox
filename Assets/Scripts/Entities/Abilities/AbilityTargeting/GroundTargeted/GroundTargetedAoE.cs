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

    public override void UseAbility(Vector3 destination)
    {
        if (Vector3.Distance(destination, transform.position) <= range)
        {
            base.UseAbility(destination);
        }
        else if (!character.EntityStatusManager.CrowdControlEffectsOnCharacter.Contains(CrowdControlEffects.ROOT))
        {
            character.CharacterMovement.SetMoveTowardsPoint(destination, range);
            character.CharacterMovement.CharacterIsInDestinationRange += base.UseAbility;
        }
    }
}
