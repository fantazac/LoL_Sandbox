using System;
using System.Collections.Generic;

public class Lucian_P : PassiveTargeted
{
    private Ability lucianE;
    private float cooldownReducedOnPassiveHitOnCharacter;
    private float cooldownReducedOnPassiveHit;
    private float criticalStrikeMultiplierAgainstNonMinions;

    protected Lucian_P()
    {
        abilityName = "Lightslinger";

        abilityType = AbilityType.PASSIVE;
        affectedTeams = AffectedTeams.GetEnemyTeams(champion.Team);
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
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

        foreach (Ability ability in champion.AbilityManager.CharacterAbilities)
        {
            ability.OnAbilityFinished += PassiveEffect;
        }

        lucianE = GetComponent<Lucian_E>();

        champion.LevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override void OnCharacterLevelUp(int level)
    {
        if (level == 7 || level == 13)
        {
            LevelUp();
        }
    }

    public override void UseAbility(Unit unitHit)
    {
        if (lucianE)//This null check is only there if I decide to use Lucian_P without the character having Lucian_E
        {
            lucianE.ReduceCooldown(unitHit is Character ? cooldownReducedOnPassiveHitOnCharacter : cooldownReducedOnPassiveHit);
        }
    }

    public override void OnEmpoweredBasicAttackHit(Unit unitHit, bool isACriticalStrike)
    {
        float damage = GetAbilityDamage(unitHit, isACriticalStrike, champion.StatsManager.CriticalStrikeDamage.GetTotal());
        DamageUnit(unitHit, damage);
        UseAbility(unitHit);
        AbilityHit(unitHit, damage);
    }

    protected override float GetAbilityDamage(Unit unitHit, bool isACriticalStrike = false, float criticalStrikeDamage = 0)
    {
        float abilityDamage;
        //if(unitHit is Minion)
        //{
        //    abilityDamage = damage + character.StatsManager.AttackDamage.GetTotal();
        //}
        //else
        //{
        abilityDamage = damage + (totalADScaling * champion.StatsManager.AttackDamage.GetTotal());
        //Here, 0.5f is for "/2" because the base criticalStrikeDamage is 2f, which increases the criticalStrikeMultiplierAgainstNonMinions if criticalStrikeDamage is not 2f.
        criticalStrikeDamage *= criticalStrikeMultiplierAgainstNonMinions * 0.5f;
        //}
        return ApplyDamageModifiers(unitHit, abilityDamage, damageType) *
            ApplyAbilityDamageModifier(unitHit) *
            (isACriticalStrike ? criticalStrikeDamage * (1f - unitHit.StatsManager.CriticalStrikeDamageReduction.GetTotal()) : 1f);
    }
}
