using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_P : PassiveTargeted
{
    private Ability lucianE;
    private float cooldownReducedOnPassiveHitOnCharacter;
    private float cooldownReducedOnPassiveHit;
    private float criticalStrikeMultiplier;
    //private float criticalStrikeMultiplierAgainstMinions;

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
        criticalStrikeMultiplier = 1.75f;
        //criticalStrikeMultiplierAgainstMinions = 2;

        AppliesOnHitEffects = true;

        IsEnabled = true;
    }

    protected override void Awake()
    {
        base.Awake();

        GetComponent<LucianBasicAttack>().SetPassive(this);
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

    public void OnPassiveHit(Entity entityHit, bool isACriticalAttack)
    {
        float damage = GetAbilityDamage(entityHit);

        //if (entityHit is Minion)
        //{
        //    entityHit.EntityStats.Health.Reduce(ApplyResistanceToDamage(entityHit, character.EntityStats.AttackDamage.GetTotal()));
        //}
        //else
        //{
        //    entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        //} 

        if (isACriticalAttack)//TODO: This should be in GetAbilityDamage(entityHit, isACriticalStrike)
        {
            //damage *= entityHit is Minion ? criticalStrikeMultiplierAgainstMinions : criticalStrikeMultiplier;
            damage *= criticalStrikeMultiplier;//TODO: Crit reduction (randuins)? Crit multiplier different than +100% (Jhin, IE)?
        }
        entityHit.EntityStats.Health.Reduce(damage);

        UseAbility(entityHit);

        AbilityHit(entityHit, damage);
    }
}
