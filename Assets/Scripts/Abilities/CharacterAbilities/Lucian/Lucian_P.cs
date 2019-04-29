using System;
using System.Collections.Generic;

public class Lucian_P : PassiveTargeted, IBasicAttackEmpoweringAbility, IBasicAttackEmpoweringAbilityWithSelfEffect
{
    private Ability lucianE;
    private readonly float cooldownReducedOnPassiveHitOnCharacter;
    private readonly float cooldownReducedOnPassiveHit;
    private readonly float criticalStrikeMultiplierAgainstNonMinions;

    protected Lucian_P()
    {
        abilityName = "Lightslinger";

        abilityType = AbilityType.PASSIVE;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        damageType = DamageType.PHYSICAL;
        effectType = AbilityEffectType.BASIC_ATTACK;

        MaxLevel = 3;
        AbilityLevel = 1;

        totalADScaling = 0.5f; // 50/55/60
        totalADScalingPerLevel = 0.05f;

        cooldownReducedOnPassiveHitOnCharacter = 2;
        cooldownReducedOnPassiveHit = 1;
        criticalStrikeMultiplierAgainstNonMinions = 1.75f;

        appliesOnHitEffects = true;
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

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
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

    private void OnCharacterLevelUp(int level)
    {
        if (level == 7 || level == 13)
        {
            LevelUp();
        }
    }

    public void ApplySelfEffect(Unit unitHit)
    {
        if (lucianE) //This null check is only there if I decide to use Lucian_P without the character having Lucian_E
        {
            lucianE.ReduceCooldown(unitHit is Character ? cooldownReducedOnPassiveHitOnCharacter : cooldownReducedOnPassiveHit);
        }
    }

    public void OnEmpoweredBasicAttackHit(Unit unitHit, bool isACriticalStrike = false)
    {
        float abilityDamage = GetAbilityDamage(unitHit, isACriticalStrike, champion.StatsManager.CriticalStrikeDamage.GetTotal());
        DamageUnit(unitHit, abilityDamage);
        AbilityHit(unitHit, abilityDamage);
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
               (isACriticalStrike ? criticalStrikeDamage * (1 - unitHit.StatsManager.CriticalStrikeDamageReduction.GetTotal()) : 1);
    }
}
