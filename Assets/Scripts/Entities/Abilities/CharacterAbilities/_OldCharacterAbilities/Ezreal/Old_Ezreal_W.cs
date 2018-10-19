using System.Collections;
using UnityEngine;

public class Old_Ezreal_W : DirectionTargetedProjectile
{
    protected Old_Ezreal_W()
    {
        abilityName = "Essence Flux";

        abilityType = AbilityType.SKILLSHOT;
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
        castTime = 0.2f;//VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealW";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Ezreal/EzrealW";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Old_Ezreal_W_Buff>() };
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

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        float damage = 0;
        if (entityHit.Team == character.Team)
        {
            AbilityBuffs[0].AddNewBuffToAffectedEntity(entityHit);
        }
        else
        {
            damage = GetAbilityDamage(entityHit);
            entityHit.EntityStats.Health.Reduce(damage);
        }
        AbilityHit(entityHit, damage);
    }
}
