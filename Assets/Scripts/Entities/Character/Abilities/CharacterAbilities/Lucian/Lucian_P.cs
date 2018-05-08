using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_P : PassiveTargeted
{
    private Ability lucianE;
    private float cooldownReducedOnPassiveHitOnCharacter;
    private float cooldownReducedOnPassiveHit;

    protected Lucian_P()
    {
        abilityName = "Lightslinger";

        abilityType = AbilityType.Passive;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        damageType = DamageType.PHYSICAL;
        effectType = AbilityEffectType.BASIC_ATTACK;

        MaxLevel = 3;
        AbilityLevel = 1;

        totalADScaling = 0.5f;
        totalADScalingPerLevel = 0.05f;

        cooldownReducedOnPassiveHitOnCharacter = 2;
        cooldownReducedOnPassiveHit = 1;

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

        lucianE = character.CharacterAbilityManager.CharacterAbilities[2];

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override void OnCharacterLevelUp(int level)
    {
        if (level == 7 || level == 13)
        {
            LevelUp();
        }
    }

    public void OnPassiveHit(Entity entityHit, bool isACriticalAttack)
    {
        //if (entityHit is Minion)
        //{
        //    entityHit.EntityStats.Health.Reduce(ApplyResistanceToDamage(entityHit, character.EntityStats.AttackDamage.GetTotal()));
        //}
        //else
        //{
        //    entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        //} 

        if (isACriticalAttack)
        {
            entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit) * 1.75f);//TODO: Crit reduction (randuins)? Crit multiplier different than +100% (Jhin, IE)?
        }
        else
        {
            entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        }

        lucianE.ReduceCooldown(entityHit is Character ? cooldownReducedOnPassiveHitOnCharacter : cooldownReducedOnPassiveHit);
    }
}
