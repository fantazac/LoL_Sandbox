using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_W : DirectionTargetedProjectile, CharacterAbility
{
    private float attackSpeedBuffDuration;
    private float attackSpeedBuffPercentBonus;

    protected Ezreal_W()
    {
        affectedUnitType = AbilityAffectedUnitType.CHARACTERS;
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.MAGIC;

        range = 1000;
        speed = 1550;
        damage = 80;
        cooldown = 7;
        castTime = 0.2f;
        delayCastTime = new WaitForSeconds(castTime);

        startCooldownOnAbilityCast = true;

        HasCastTime = true;

        attackSpeedBuffDuration = 5;
        attackSpeedBuffPercentBonus = 20;
    }

    protected override void SetAbilitySpritePath()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Ezreal/EzrealW";
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        if (entityHit.Team == character.Team)
        {
            ApplyBuffToTarget(entityHit);
        }
        else
        {
            entityHit.GetComponent<Health>().Reduce(damage);
        }
    }

    protected void ApplyBuffToTarget(Entity entityHit)
    {
        entityHit.EntityBuffManager.ApplyBuff(new Buff(this, entityHit, attackSpeedBuffDuration));
    }

    public override void ApplyBuffToEntityHit(Entity entityHit)
    {
        entityHit.GetComponent<AttackSpeed>().AddPercentBonus(attackSpeedBuffPercentBonus);
        Debug.Log("Increased " + entityHit.gameObject + "'s Attack Speed");
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit)
    {
        entityHit.GetComponent<AttackSpeed>().AddPercentBonus(-attackSpeedBuffPercentBonus);
        Debug.Log("Decreased " + entityHit.gameObject + "'s Attack Speed");
    }
}
