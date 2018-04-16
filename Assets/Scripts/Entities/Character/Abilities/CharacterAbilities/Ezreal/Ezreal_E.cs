using UnityEngine;
using System.Collections;

public class Ezreal_E : GroundTargetedBlink, CharacterAbility
{
    private string projectilePrefabPath;
    private GameObject projectilePrefab;

    private float effectRadius;

    protected Ezreal_E()
    {
        abilityName = "Arcane Shift";

        abilityType = AbilityType.Blink;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 5;

        range = 475;
        speed = 1500;
        damage = 80;// 80/130/180/230/280
        damagePerLevel = 50;
        bonusADScaling = 0.5f;// 50%
        totalAPScaling = 0.75f;// 75%
        resourceCost = 90;// 90
        baseCooldown = 19;// 19/17.5f/16/14.5f/13
        baseCooldownPerLevel = -1.5f;
        castTime = 0.15f;//TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        affectedByCooldownReduction = true;
        startCooldownOnAbilityCast = true;

        effectRadius = 600;// Says 750 on wiki, is more like 600 when I tested
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealE";

        projectilePrefabPath = "CharacterAbilities/Ezreal/EzrealE";
    }

    protected override void ModifyValues()
    {
        effectRadius *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }

    protected override void LoadPrefabs()
    {
        projectilePrefab = Resources.Load<GameObject>(projectilePrefabPath);
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        destinationOnCast = FindPointToMoveTo(destinationOnCast, transform.position);
        character.CharacterOrientation.RotateCharacterInstantly(destinationOnCast);
        transform.position = destinationOnCast;
        character.CharacterMovement.NotifyCharacterMoved();

        FinishAbilityCast();

        ShootHomingProjectile();
    }

    private void ShootHomingProjectile()
    {
        Entity closestEntity = null;
        Entity tempEntity;
        float distance = float.MaxValue;
        float tempDistance;

        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            tempEntity = collider.GetComponent<Entity>();
            if (tempEntity != null && TargetIsValid.CheckIfTargetIsValid(tempEntity, affectedUnitType, character.Team))
            {
                tempDistance = Vector3.Distance(transform.position, tempEntity.transform.position);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    closestEntity = tempEntity;
                }
            }
        }

        if (closestEntity != null)
        {
            ProjectileUnitTargeted projectile = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<ProjectileUnitTargeted>();
            projectile.ShootProjectile(character.Team, closestEntity, speed);
            projectile.OnAbilityEffectHit += OnProjectileHit;
        }
    }

    private void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        AbilityHit();
        Destroy(projectile.gameObject);
    }
}
