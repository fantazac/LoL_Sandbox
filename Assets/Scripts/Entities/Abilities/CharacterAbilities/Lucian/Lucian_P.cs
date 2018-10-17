public class Lucian_P : PassiveTargeted
{
    private Ability lucianE;
    private float cooldownReducedOnPassiveHitOnCharacter;
    private float cooldownReducedOnPassiveHit;
    private float criticalStrikeMultiplierAgainstNonMinions;

    protected Lucian_P()
    {
        abilityName = "Lightslinger";

        abilityType = AbilityType.Passive;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        damageType = DamageType.PHYSICAL;
        effectType = AbilityEffectType.BASIC_ATTACK;

        MaxLevel = 3;
        AbilityLevel = 1;

        totalADScaling = 0.5f;// 50/55/60
        totalADScalingPerLevel = 0.05f;

        cooldownReducedOnPassiveHitOnCharacter = 2;
        cooldownReducedOnPassiveHit = 1;
        criticalStrikeMultiplierAgainstNonMinions = 1.75f;

        AppliesOnHitEffects = true;

        IsEnabled = true;
    }

    protected override void Awake()
    {
        base.Awake();

        GetComponent<LucianBasicAttack>().SetBasicAttackEmpoweringAbility(this);
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianP";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Lucian_P_Buff>() };

        foreach (Ability ability in character.CharacterAbilityManager.CharacterAbilities)
        {
            ability.OnAbilityFinished += PassiveEffect;
        }

        lucianE = GetComponent<Lucian_E>();

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override void OnCharacterLevelUp(int level)
    {
        if (level == 7 || level == 13)
        {
            LevelUp();
        }
    }

    public override void UseAbility(Entity entityHit)
    {
        if (lucianE)
        {
            lucianE.ReduceCooldown(entityHit is Character ? cooldownReducedOnPassiveHitOnCharacter : cooldownReducedOnPassiveHit);
        }
    }

    public override void OnEmpoweredBasicAttackHit(Entity entityHit, bool isACriticalStrike)
    {
        float damage = GetAbilityDamage(entityHit, isACriticalStrike, character.EntityStats.CriticalStrikeDamage.GetTotal());
        entityHit.EntityStats.Health.Reduce(damage);
        UseAbility(entityHit);
        AbilityHit(entityHit, damage);
    }

    protected override float GetAbilityDamage(Entity entityHit, bool isACriticalStrike = false, float criticalStrikeDamage = 0)
    {
        float abilityDamage;
        //if(entityHit is Minion)
        //{
        //    abilityDamage = damage + character.EntityStats.AttackDamage.GetTotal();
        //}
        //else
        //{
        abilityDamage = damage + (totalADScaling * character.EntityStats.AttackDamage.GetTotal());
        //Here, 0.5f is for "/2" because the base criticalStrikeDamage is 2f, which increases the criticalStrikeMultiplierAgainstNonMinions if criticalStrikeDamage is not 2f.
        criticalStrikeDamage *= criticalStrikeMultiplierAgainstNonMinions * 0.5f;
        //}
        return ApplyDamageModifiers(entityHit, abilityDamage) *
            ApplyAbilityDamageModifier(entityHit) *
            (isACriticalStrike ? criticalStrikeDamage * (1f - entityHit.EntityStats.CriticalStrikeDamageReduction.GetTotal()) : 1f);
    }
}
