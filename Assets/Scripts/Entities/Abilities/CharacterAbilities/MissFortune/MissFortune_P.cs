public class MissFortune_P : PassiveTargeted
{
    private Entity lastEntityHit;

    public delegate void OnPassiveHitHandler();
    public event OnPassiveHitHandler OnPassiveHit;

    protected MissFortune_P()
    {
        abilityName = "Love Tap";

        abilityType = AbilityType.Passive;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        damageType = DamageType.PHYSICAL;
        effectType = AbilityEffectType.SINGLE_TARGET;

        MaxLevel = 6;
        AbilityLevel = 1;

        totalADScaling = 0.5f;
        totalADScalingPerLevel = 0.1f;

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneP";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<MissFortune_P_Debuff>() };

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
        character.CharacterOnHitEffectsManager.OnApplyOnHitEffects += SetPassiveEffectOnEntityHit;

        lastEntityHit = character;
    }

    public override void OnCharacterLevelUp(int level)
    {
        if (level == 4 || level == 7 || level == 9 || level == 11 || level == 13)
        {
            LevelUp();
        }
    }

    private void SetPassiveEffectOnEntityHit(Entity entityHit, float damage)
    {
        if (entityHit != lastEntityHit)
        {
            AbilityBuffs[0].ConsumeBuff(lastEntityHit);
            AbilityBuffs[0].AddNewBuffToAffectedEntity(entityHit);

            lastEntityHit = entityHit;

            float passiveDamage = GetAbilityDamage(entityHit);
            //if (entityHit is Minion)
            //{
            //    entityHit.EntityStats.Health.Reduce(ApplyResistanceToDamage(entityHit, character.EntityStats.AttackDamage.GetTotal() * totalADScaling * 0.5f));
            //}
            //else
            //{
            entityHit.EntityStats.Health.Reduce(passiveDamage);
            //} 
            character.EntityStats.Health.Restore(passiveDamage * 0.01f * character.EntityStats.LifeSteal.GetTotal());

            if (OnPassiveHit != null)
            {
                OnPassiveHit();
            }
        }
    }

    private void OnDestroy()
    {
        if (lastEntityHit != null)
        {
            AbilityBuffs[0].ConsumeBuff(lastEntityHit);
        }
    }
}
