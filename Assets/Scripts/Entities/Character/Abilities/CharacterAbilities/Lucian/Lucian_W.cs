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
        abilityName = "Ardent Blaze";

        abilityType = AbilityType.Skillshot;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        range = 900;
        speed = 1550;
        damage = 85;// 85/125/165/205/245
        damagePerLevel = 40;
        totalAPScaling = 0.9f;// 90%
        resourceCost = 50;// 50
        cooldown = 14;// 14/13/12/11/10
        cooldownPerLevel = -1;
        castTime = 0.25f;
        delayCastTime = new WaitForSeconds(castTime);

        buffDuration = 1;
        buffFlatBonus = 60;// 60/65/70/75/80
        buffFlatBonusPerLevel = 5;
        debuffDuration = 6;

        startCooldownOnAbilityCast = true;

        durationAoE = 0.2f;
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
        entityHit.EntityStats.Health.Reduce(GetAbilityDamage());
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
        EntitiesAffectedByBuff.Add(entityHit);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.MovementSpeed.RemoveFlatBonus(buffFlatBonus);
        EntitiesAffectedByBuff.Remove(entityHit);
    }

    public override void ApplyDebuffToEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.Health.OnHealthReduced += OnEntityDamaged;
        EntitiesAffectedByDebuff.Add(entityHit);
    }

    public override void RemoveDebuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.Health.OnHealthReduced -= OnEntityDamaged;
        EntitiesAffectedByDebuff.Remove(entityHit);
    }
}
