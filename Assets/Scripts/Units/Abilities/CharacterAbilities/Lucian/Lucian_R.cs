﻿using UnityEngine;
using System.Collections;

public class Lucian_R : DirectionTargetedProjectile
{
    private float durationOfActive;
    private int amountOfProjectilesToShoot;
    private int amountOfProjectilesToShootPerLevel;
    //private float damageMultiplierAgainstMinions;

    private float projectileOffset;

    private WaitForSeconds delayBetweenBullets;

    private Projectile projectile;

    protected Lucian_R()
    {
        abilityName = "The Culling";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 3;

        range = 1200;
        speed = 2000;
        damage = 20;// 20/35/50
        damagePerLevel = 15;
        totalADScaling = 0.25f;// 25%
        totalAPScaling = 0.1f;// 10%
        resourceCost = 100;// 100
        baseCooldown = 110;// 110/100/90
        baseCooldownPerLevel = -10;
        cooldownBeforeRecast = 0.75f;
        castTime = 0.01f;
        delayCastTime = new WaitForSeconds(castTime);

        amountOfProjectilesToShoot = 20;// 20/25/30
        amountOfProjectilesToShootPerLevel = 5;
        //damageMultiplierAgainstMinions = 2f;
        durationOfActive = 3;
        projectileOffset = 0.2f;
        delayBetweenBullets = new WaitForSeconds(durationOfActive / amountOfProjectilesToShoot);

        CanBeRecasted = true;
        CannotRotateWhileActive = true;
        CanMoveWhileActive = true;

        IsAnUltimateAbility = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianR";
        abilityRecastSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianR_Active";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Lucian/LucianR";
    }

    protected override void Start()
    {
        foreach (Ability ability in champion.AbilityManager.CharacterAbilities)
        {
            if (!(ability is DirectionTargetedDash || ability == this))
            {
                AbilitiesToDisableWhileActive.Add(ability);
            }
        }
        AbilitiesToDisableWhileActive.Add(champion.AbilityManager.OtherCharacterAbilities[0]);

        CastableAbilitiesWhileActive.Add(GetComponent<Lucian_E>());

        base.Start();
    }

    public override void LevelUpExtraStats()
    {
        amountOfProjectilesToShoot += amountOfProjectilesToShootPerLevel;

        delayBetweenBullets = new WaitForSeconds(durationOfActive / amountOfProjectilesToShoot);
    }

    protected override void RotationOnAbilityCast(Vector3 destination)
    {
        champion.OrientationManager.RotateCharacterInstantly(destination);
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();

        ShootProjectile(0);

        for (int i = 1; i < amountOfProjectilesToShoot; i++)
        {
            yield return delayBetweenBullets;

            ShootProjectile(i);
        }

        FinishAbilityCast();
    }

    private void ShootProjectile(int projectileId)
    {
        SpawnProjectile(transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.5f) + (transform.right * (projectileId % 2 == 0 ? projectileOffset : -projectileOffset)), transform.rotation);
    }

    protected override float ApplyAbilityDamageModifier(Unit unitHit)
    {
        //TODO when Minion exists: return unitHit is Minion ? damageMultiplierAgainstMinions : 1f ;
        return 1f;
    }
}
