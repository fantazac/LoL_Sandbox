using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_W : DirectionTargetedProjectile
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
        baseCooldown = 9;// 9
        castTime = 0.2f;//TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        affectedByCooldownReduction = true;
        startCooldownOnAbilityCast = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealW";

        projectilePrefabPath = "CharacterAbilities/Ezreal/EzrealW";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Ezreal_W_Buff>() };
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        character.CharacterOrientation.RotateCharacterInstantly(destinationOnCast);

        SpawnProjectile(transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.65f), transform.rotation);

        FinishAbilityCast();
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        if (entityHit.Team == character.Team)
        {
            AbilityBuffs[0].AddNewBuffToAffectedEntity(entityHit);
        }
        else
        {
            entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        }
        AbilityHit();
    }
}
