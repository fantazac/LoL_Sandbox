using UnityEngine;

public abstract class UnitTargetedAoE : UnitTargeted
{
    protected string areaOfEffectPrefabPath;
    protected GameObject areaOfEffectPrefab;

    protected override void LoadPrefabs()
    {
        areaOfEffectPrefab = Resources.Load<GameObject>(areaOfEffectPrefabPath);
    }

    protected override void OnAbilityEffectHit(AbilityEffect areaOfEffect, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        float damage = GetAbilityDamage(entityHit);
        entityHit.EntityStatsManager.ReduceHealth(damageType, damage);
        AbilityHit(entityHit, damage);
    }
}
