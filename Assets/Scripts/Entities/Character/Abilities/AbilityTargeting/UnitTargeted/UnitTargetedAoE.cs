using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitTargetedAoE : UnitTargeted
{
    protected string areaOfEffectPrefabPath;
    protected GameObject areaOfEffectPrefab;

    protected override void LoadPrefabs()
    {
        areaOfEffectPrefab = Resources.Load<GameObject>(areaOfEffectPrefabPath);
    }

    protected override void OnAbilityEffectHit(AbilityEffect areaOfEffect, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        AbilityHit();
    }
}
