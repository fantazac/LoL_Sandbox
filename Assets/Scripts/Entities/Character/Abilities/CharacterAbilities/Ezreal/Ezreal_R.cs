using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_R : DirectionTargetedProjectile, CharacterAbility
{
    private const float DAMAGE_REDUCTION_PER_TARGET_HIT = 0.1f;
    private const float DAMAGE_REDUCTION_CAP = 0.3f;

    private float currentDamageMultiplier;

    protected Ezreal_R()
    {
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.MAGIC;

        range = (float)Range.GLOBAL;
        speed = 2000;
        damage = 350;// 350/500/650 + BONUS AD % 100 + TOTAL AP % 90
        resourceCost = 100;
        cooldown = 120;
        castTime = 1;
        delayCastTime = new WaitForSeconds(castTime);

        startCooldownOnAbilityCast = true;

        HasCastTime = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealR";
    }

    protected override void FinalAdjustments(Vector3 destination)
    {
        currentDamageMultiplier = 1f;
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        SetPositionAndRotationOnCast(transform.position);

        yield return delayCastTime;

        Projectile projectile = ((GameObject)Instantiate(projectilePrefab, positionOnCast, rotationOnCast)).GetComponent<Projectile>();
        projectile.ShootProjectile(character.Team, affectedUnitType, speed, range);
        projectile.OnAbilityEffectHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;

        FinishAbilityCast();
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(damage * currentDamageMultiplier);
        if (currentDamageMultiplier > DAMAGE_REDUCTION_CAP)
        {
            currentDamageMultiplier -= DAMAGE_REDUCTION_PER_TARGET_HIT;
        }
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        AbilityHit();
    }
}
