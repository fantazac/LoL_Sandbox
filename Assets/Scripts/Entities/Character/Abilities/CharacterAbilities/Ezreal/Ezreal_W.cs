using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_W : DirectionTargetedProjectile, CharacterAbility
{
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

        buffDuration = 5;
        buffPercentBonus = 20;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Ezreal/EzrealW";
        buffSpritePath = "Sprites/CharacterAbilities/Ezreal/EzrealW_Buff";
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        if (entityHit.Team == character.Team)
        {
            AddNewBuffToEntityHit(entityHit);
        }
        else
        {
            entityHit.EntityStats.Health.Reduce(damage);
        }
        AbilityHit();
    }

    public override void ApplyBuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.AttackSpeed.AddPercentBonus(buffPercentBonus);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.AttackSpeed.RemovePercentBonus(buffPercentBonus);
    }
}
