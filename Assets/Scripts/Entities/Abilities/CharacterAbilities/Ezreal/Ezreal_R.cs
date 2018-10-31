using System.Collections;
using UnityEngine;

public class Ezreal_R : DirectionTargetedProjectile
{
    private const float DAMAGE_REDUCTION_PER_TARGET_HIT = 0.1f;
    private const float DAMAGE_REDUCTION_CAP = 0.3f;

    private float currentDamageMultiplier;

    protected Ezreal_R()
    {
        abilityName = "Trueshot Barrage";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.MAGIC;

        MaxLevel = 3;

        range = (float)Range.GLOBAL;
        speed = 2000;
        damage = 350;// 350/500/650
        damagePerLevel = 150;
        bonusADScaling = 1;// 100%
        totalAPScaling = 0.9f;// 90%
        resourceCost = 100;// 100
        baseCooldown = 120;// 120
        castTime = 1;
        delayCastTime = new WaitForSeconds(castTime);

        IsAnUltimateAbility = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealR";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Ezreal/EzrealR";
    }

    protected override void FinalAdjustments(Vector3 destination)
    {
        currentDamageMultiplier = 1f;
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        SetPositionAndRotationOnCast(transform.position);
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        SpawnProjectile(positionOnCast, rotationOnCast);

        FinishAbilityCast();
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        float damage = GetAbilityDamage(entityHit) * currentDamageMultiplier;
        entityHit.EntityStats.Health.Reduce(damage);
        if (currentDamageMultiplier > DAMAGE_REDUCTION_CAP)
        {
            currentDamageMultiplier -= DAMAGE_REDUCTION_PER_TARGET_HIT;
        }
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        AbilityHit(entityHit, damage);
    }
}
