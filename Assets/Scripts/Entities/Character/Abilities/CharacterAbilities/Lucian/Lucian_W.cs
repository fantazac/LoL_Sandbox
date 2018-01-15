using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_W : SkillShot, CharacterAbility
{
    [SerializeField]
    private GameObject explosionAreaOfEffectPrefab;

    private float durationAoE;

    protected Lucian_W()
    {
        range = 900;
        speed = 1550;
        damage = 60;
        castTime = 0.2f;
        delayCastTime = new WaitForSeconds(castTime);

        durationAoE = 0.2f;

        CanStopMovement = true;
        HasCastTime = true;
    }

    protected override void OnSkillShotHit(Projectile projectile)
    {
        OnSkillShotReachedEnd(projectile);
    }

    protected override void OnSkillShotReachedEnd(Projectile projectile)
    {
        AreaOfEffect aoe = ((GameObject)Instantiate(explosionAreaOfEffectPrefab, projectile.transform.position, projectile.transform.rotation)).GetComponent<AreaOfEffect>();
        aoe.ActivateAreaOfEffect(projectile.HealthOfUnitsAlreadyHit, character.team, false, damage, durationAoE, true);
        Destroy(projectile.gameObject);
    }
}
