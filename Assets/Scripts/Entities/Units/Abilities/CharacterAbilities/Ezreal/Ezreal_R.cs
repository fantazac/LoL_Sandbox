using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_R : DirectionTargetedProjectile
{
    //private float damageMultiplierAgainstMinionsAndNonEpicMonsters;

    protected Ezreal_R()
    {
        abilityName = "Trueshot Barrage";

        abilityType = AbilityType.SKILLSHOT;
        affectedTeams = AffectedTeams.GetEnemyTeams(champion.Team);
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

        //damageMultiplierAgainstMinionsAndNonEpicMonsters = 0.5f;

        IsAnUltimateAbility = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealR";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Ezreal/EzrealR";
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

    protected override float ApplyAbilityDamageModifier(Unit unitHit)
    {
        //TODO when Minion and Non-Epic Monsters exists, return damageMultiplierAgainstMinionsAndNonEpicMonsters
        return 1f;
    }
}
