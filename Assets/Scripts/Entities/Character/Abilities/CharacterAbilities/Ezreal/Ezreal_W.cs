using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_W : DirectionTargetedProjectile, CharacterAbility
{
    protected Ezreal_W()
    {
        abilityName = "Essence Flux";

        abilityType = AbilityType.Skillshot;
        affectedUnitType = AbilityAffectedUnitType.CHARACTERS;
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.MAGIC;

        MaxLevel = 5;

        range = 1000;
        speed = 1550;
        damage = 70;// 70/115/160/205/250
        damagePerLevel = 45;
        totalAPScaling = 0.8f;// 80%
        resourceCost = 50;// 50/60/70/80/90
        resourceCostPerLevel = 10;
        cooldown = 9;// 9
        castTime = 0.2f;//TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        startCooldownOnAbilityCast = true;

        buffDuration = 5;
        buffPercentBonus = 20;// 20/25/30/35/40
        buffPercentBonusPerLevel = 5;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealW";
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealW_Buff";
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        character.CharacterOrientation.RotateCharacterInstantly(destinationOnCast);

        SpawnProjectile(transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.65f), transform.rotation);

        FinishAbilityCast();
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        if (entityHit.Team == character.Team)
        {
            AddNewBuffToEntityHit(entityHit);
        }
        else
        {
            entityHit.EntityStats.Health.Reduce(GetAbilityDamage());
        }
        AbilityHit();
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.AttackSpeed.AddPercentBonus(buffPercentBonus);
        EntitiesAffectedByBuff.Add(entityHit);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.AttackSpeed.RemovePercentBonus(buffPercentBonus);
        EntitiesAffectedByBuff.Remove(entityHit);
    }
}
