using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_Ezreal_R : DirectionTargetedProjectile
{
    private const float DAMAGE_REDUCTION_PER_TARGET_HIT = 0.1f;
    private const float DAMAGE_REDUCTION_CAP = 0.3f;

    private float currentDamageMultiplier;

    protected Old_Ezreal_R()
    {
        abilityName = "Trueshot Barrage";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
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

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
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

    protected override void OnProjectileHit(Projectile projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        float damage = GetAbilityDamage(unitHit) * currentDamageMultiplier;
        DamageUnit(unitHit, damage);
        if (currentDamageMultiplier > DAMAGE_REDUCTION_CAP)
        {
            currentDamageMultiplier -= DAMAGE_REDUCTION_PER_TARGET_HIT;
        }
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        AbilityHit(unitHit, damage);
    }
}
