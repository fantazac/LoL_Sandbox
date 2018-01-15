using UnityEngine;
using System.Collections;

public abstract class SkillShot : Ability
{
    [SerializeField]
    protected GameObject projectilePrefab;

    protected float range;
    protected float speed;
    protected float damage;

    protected override void ModifyValues()
    {
        range /= StaticObjects.DivisionFactor;
        speed /= StaticObjects.DivisionFactor;
    }

    public override bool CanBeCast(Vector3 mousePosition)
    {
        return !character.CharacterAbilityManager.IsUsingAbilityPreventingAbilityCasts() && MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
    }

    public override Vector3 GetDestination()
    {
        return hit.point + character.CharacterMovement.CharacterHeightOffset;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.CharacterMovement.StopAllMovement(this);
        character.CharacterOrientation.RotateCharacterInstantly(destination);

        if (delayCastTime == null)
        {
            StartCoroutine(AbilityWithoutCastTime());
        }
        else
        {
            StartCoroutine(AbilityWithCastTime());
        }
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        Projectile projectile = ((GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation)).GetComponent<Projectile>();
        projectile.ShootProjectile(GetComponent<Character>().team, speed, range, damage);
        projectile.OnProjectileHit += OnSkillShotHit;
        projectile.OnProjectileReachedEnd += OnSkillShotReachedEnd;

        FinishAbilityCast();
    }

    protected virtual void OnSkillShotHit(Projectile projectile) { }

    protected virtual void OnSkillShotReachedEnd(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
