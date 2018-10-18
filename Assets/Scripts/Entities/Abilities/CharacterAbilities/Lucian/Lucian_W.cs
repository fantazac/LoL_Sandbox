using UnityEngine;

public class Lucian_W : DirectionTargetedProjectile
{
    private string explosionAreaOfEffectPrefabPath;
    private GameObject explosionAreaOfEffectPrefab;

    private float durationAoE;

    protected Lucian_W()
    {
        abilityName = "Ardent Blaze";

        abilityType = AbilityType.Skillshot;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 5;

        range = 900;
        speed = 1550;
        damage = 85;// 85/125/165/205/245
        damagePerLevel = 40;
        totalAPScaling = 0.9f;// 90%
        resourceCost = 50;// 50
        baseCooldown = 14;// 14/13/12/11/10
        baseCooldownPerLevel = -1;
        castTime = 0.25f;
        delayCastTime = new WaitForSeconds(castTime);

        affectedByCooldownReduction = true;

        durationAoE = 0.2f;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianW";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Lucian/LucianW1";
        explosionAreaOfEffectPrefabPath = "CharacterAbilitiesPrefabs/Lucian/LucianW2";
    }

    protected override void LoadPrefabs()
    {
        base.LoadPrefabs();

        explosionAreaOfEffectPrefab = Resources.Load<GameObject>(explosionAreaOfEffectPrefabPath);
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Lucian_W_Buff>() };
        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Lucian_W_Debuff>() };

        AbilityDebuffs[0].OnAbilityBuffRemoved += RemoveDebuffFromEntityHit;
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        OnAreaOfEffectHit(projectile, entityHit, isACriticalStrike, willMiss);
        OnProjectileReachedEnd((Projectile)projectile);
    }

    protected override void OnProjectileReachedEnd(Projectile projectile)
    {
        AreaOfEffect aoe = Instantiate(explosionAreaOfEffectPrefab, projectile.transform.position, projectile.transform.rotation).GetComponent<AreaOfEffect>();
        aoe.CreateAreaOfEffect(projectile.UnitsAlreadyHit, character.Team, affectedUnitType, durationAoE, true);
        aoe.ActivateAreaOfEffect();
        aoe.OnAbilityEffectHit += OnAreaOfEffectHit;
        Destroy(projectile.gameObject);
    }

    private void OnAreaOfEffectHit(AbilityEffect projectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        float damage = GetAbilityDamage(entityHit);
        entityHit.EntityStats.Health.Reduce(damage);
        AddNewDebuffToEntityHit(entityHit);
        AbilityHit(entityHit, damage);
    }

    private void OnEntityDamaged()
    {
        AbilityBuffs[0].AddNewBuffToAffectedEntity(character);
    }

    private void AddNewDebuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.Health.OnHealthReduced += OnEntityDamaged;
        AbilityDebuffs[0].AddNewBuffToAffectedEntity(entityHit);
    }

    private void RemoveDebuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.Health.OnHealthReduced -= OnEntityDamaged;
    }
}
