using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lucian_R : DirectionTargetedProjectile, CharacterAbility
{
    private int amountOfProjectilesToShoot;
    private int amountOfProjectilesToShootPerLevel;
    private float offset;

    private WaitForSeconds delayBetweenBullets;

    private Projectile projectile;

    protected Lucian_R()
    {
        abilityName = "The Culling";

        abilityType = AbilityType.Skillshot;
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
        cooldown = 110;// 110/100/90
        cooldownPerLevel = -10;
        cooldownBeforeRecast = 0.75f;
        castTime = 0.01f;
        delayCastTime = new WaitForSeconds(castTime);

        amountOfProjectilesToShoot = 20;// 20/25/30
        amountOfProjectilesToShootPerLevel = 5;
        durationOfActive = 3;
        offset = 0.2f;
        delayBetweenBullets = new WaitForSeconds(durationOfActive / amountOfProjectilesToShoot);

        CanBeRecasted = true;
        CannotRotateWhileCasting = true;
        CanMoveWhileActive = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianR";
    }

    protected override void Start()
    {
        AbilitiesToDisableWhileActive.Add(GetComponent<Lucian_Q>());
        AbilitiesToDisableWhileActive.Add(GetComponent<Lucian_W>());

        CastableAbilitiesWhileActive.Add(GetComponent<Lucian_E>());
        CastableAbilitiesWhileActive.Add(GetComponent<Teleport>());

        base.Start();
    }

    public override void LevelUpExtraStats()
    {
        amountOfProjectilesToShoot += amountOfProjectilesToShootPerLevel;

        delayBetweenBullets = new WaitForSeconds(durationOfActive / amountOfProjectilesToShoot);
    }

    protected override void RotationOnAbilityCast(Vector3 destination)
    {
        character.CharacterOrientation.RotateCharacterInstantly(destination);
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

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
        projectile = ((GameObject)Instantiate(projectilePrefab,
                transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.5f) + (transform.right * (projectileId % 2 == 0 ? offset : -offset)),
                transform.rotation)).GetComponent<Projectile>();
        projectile.ShootProjectile(character.Team, affectedUnitType, speed, range);
        projectile.OnAbilityEffectHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
    }
}
