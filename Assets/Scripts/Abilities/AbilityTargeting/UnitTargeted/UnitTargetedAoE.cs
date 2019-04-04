using UnityEngine;

public abstract class UnitTargetedAoE : UnitTargeted
{
    protected string areaOfEffectPrefabPath;
    protected GameObject areaOfEffectPrefab;

    protected override void LoadPrefabs()
    {
        areaOfEffectPrefab = Resources.Load<GameObject>(areaOfEffectPrefabPath);
    }

    protected void OnAreaOfEffectHit(AreaOfEffect areaOfEffect, Unit unitHit)
    {
        float damage = GetAbilityDamage(unitHit);
        DamageUnit(unitHit, damage);
        AbilityHit(unitHit, damage);
    }
}
