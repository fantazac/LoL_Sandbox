using UnityEngine;
using System.Collections;

public class Ezreal_E : GroundTargetedBlink, CharacterAbility
{
    [SerializeField]
    private GameObject projectilePrefab;

    protected Ezreal_E()
    {
        effectType = AbilityEffectType.SINGLE_TARGET;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;

        range = 475;
        speed = 1500;
        damage = 50;
        castTime = 0.15f;
        delayCastTime = new WaitForSeconds(castTime);

        CanStopMovement = true;
        HasCastTime = true;
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        transform.position = destination;
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

        foreach (Collider collider in Physics.OverlapSphere(transform.position, range))
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
            ProjectileUnitTargeted projectile = ((GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation)).GetComponent<ProjectileUnitTargeted>();
            projectile.ShootProjectile(character.Team, closestEntity, speed);
            projectile.OnAbilityEffectHit += OnProjectileHit;
        }
    }

    private void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.GetComponent<Health>().Hit(damage);
        Destroy(projectile.gameObject);
    }
}
