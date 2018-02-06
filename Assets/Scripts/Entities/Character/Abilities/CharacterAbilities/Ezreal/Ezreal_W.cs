using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_W : DirectionTargetedProjectile, CharacterAbility
{
    private float attackSpeedBuffDurationInSeconds;
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

        attackSpeedBuffDurationInSeconds = 5;
        attackSpeedBuffPercentBonus = 20;   //TODO: make the bonus scale with level
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
        Buff attackSpeedBuff = new Buff(attackSpeedBuffDurationInSeconds);
        attackSpeedBuff.OnApply += OnApplyAttackSpeedBuff;
        attackSpeedBuff.OnRemove += OnRemoveAttackSpeedBuff;

        entityHit.ApplyBuff(attackSpeedBuff);
    }

    private void OnApplyAttackSpeedBuff(Entity entity)
    {
        entity.GetComponent<AttackSpeed>().AddPercentBonus(20); //FIXME: the stats should be properties of Entity.
        Debug.Log("Increased " + entity.gameObject + "'s Attack Speed");
    }

    private void OnRemoveAttackSpeedBuff(Entity entity)
    {
        entity.GetComponent<AttackSpeed>().AddPercentBonus(-20);
        Debug.Log("Decreased " + entity.gameObject + "'s Attack Speed");
    }
}
