using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lucian_R : DirectionTargetedProjectile, CharacterAbility
{
    private int amountOfProjectilesToShoot;
    private float offset;

    private WaitForSeconds delayBetweenBullets;

    private Projectile projectile;

    protected Lucian_R()
    {
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        range = 1200;
        speed = 2000;
        damage = 40;
        cooldown = 13;

        amountOfProjectilesToShoot = 20;
        durationOfActive = 3;
        offset = 0.2f;
        delayBetweenBullets = new WaitForSeconds(durationOfActive / (float)amountOfProjectilesToShoot);

        CanBeCancelled = true;
        CannotRotateWhileCasting = true;
        CanMoveWhileCasting = true;
    }

    protected override void SetAbilitySpritePath()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Lucian/LucianR";
    }

    protected override void Start()
    {
        CastableAbilitiesWhileActive.Add(GetComponent<Lucian_E>());
        //CastableAbilitiesWhileActive.Add(this); Useful once cancel is coded?

        base.Start();
    }

    protected override void RotationOnAbilityCast(Vector3 destination)
    {
        character.CharacterOrientation.RotateCharacterInstantly(destination);
    }

    protected override IEnumerator AbilityWithoutCastTime()
    {
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
                transform.position + (transform.forward * 0.6f) + (transform.right * (projectileId % 2 == 0 ? offset : -offset)),
                transform.rotation)).GetComponent<Projectile>();
        projectile.ShootProjectile(character.Team, affectedUnitType, speed, range);
        projectile.OnAbilityEffectHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
    }
}
