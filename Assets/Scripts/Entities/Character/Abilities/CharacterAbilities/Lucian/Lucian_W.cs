using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_W : DirectionTargetedProjectile, CharacterAbility
{
    [SerializeField]
    private GameObject explosionAreaOfEffectPrefab;

    private float durationAoE;

    protected Lucian_W()
    {
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        range = 900;
        speed = 1550;
        damage = 60;// 60/100/140/180/220 + TOTAL AP % 90
        resourceCost = 50;
        cooldown = 14;// 14/13/12/11/10
        castTime = 0.25f;
        delayCastTime = new WaitForSeconds(castTime);

        buffDuration = 1;
        buffFlatBonus = 60;// 60/65/70/75/80
        debuffDuration = 6;

        startCooldownOnAbilityCast = true;

        durationAoE = 0.2f;

        HasCastTime = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianW";
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianW_Buff";
        debuffSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianW_Debuff";
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        OnAreaOfEffectHit(projectile, entityHit);
        AbilityHit();
        OnProjectileReachedEnd((Projectile)projectile);
    }

    protected override void OnProjectileReachedEnd(Projectile projectile)
    {
        AreaOfEffect aoe = ((GameObject)Instantiate(explosionAreaOfEffectPrefab, projectile.transform.position, projectile.transform.rotation)).GetComponent<AreaOfEffect>();
        aoe.ActivateAreaOfEffect(projectile.UnitsAlreadyHit, character.Team, affectedUnitType, durationAoE, true);
        aoe.OnAbilityEffectHit += OnAreaOfEffectHit;
        Destroy(projectile.gameObject);
    }

    private void OnAreaOfEffectHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(damage);
        AddNewDebuffToEntityHit(entityHit);
        AbilityHit();
    }

    private void OnEntityDamaged()
    {
        AddNewBuffToEntityHit(character);
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.MovementSpeed.AddFlatBonus(buffFlatBonus);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.MovementSpeed.RemoveFlatBonus(buffFlatBonus);
    }

    public override void ApplyDebuffToEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.Health.OnHealthReduced += OnEntityDamaged;
    }

    public override void RemoveDebuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.Health.OnHealthReduced -= OnEntityDamaged;
    }
}
