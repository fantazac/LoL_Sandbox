using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_P : PassiveTargeted, CharacterAbility, PassiveCharacterAbility
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

        totalADScaling = 0.5f;
        totalADScalingPerLevel = 0.05f;

        buffDuration = 3;

        cooldownReducedOnPassiveHitOnCharacter = 2;
        cooldownReducedOnPassiveHit = 1;

        IsEnabled = true;
    }

    protected override void Awake()
    {
        base.Awake();

        lucianE = GetComponent<Lucian_E>();
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianP";
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianP_Buff";
    }

    protected override void Start()
    {
        base.Start();

        foreach (Ability ability in GetComponents<CharacterAbility>())
        {
            if (!(ability is PassiveCharacterAbility))
            {
                ability.OnAbilityFinished += PassiveEffect;
            }
        }

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override void UseAbility(Entity target)
    {
        if (target is Character)
        {
            lucianE.ReduceCooldown(cooldownReducedOnPassiveHitOnCharacter);
        }
        else
        {
            lucianE.ReduceCooldown(cooldownReducedOnPassiveHit);
        }
    }

    public override void OnCharacterLevelUp(int level)
    {
        if (level == 7 || level == 13)
        {
            LevelUpAbilityStats();
        }
    }

    public void OnPassiveHit(Entity entityHit)
    {
        //if (entityHit is Minion)
        //{
        //    entityHit.EntityStats.Health.Reduce(ApplyResistanceToDamage(entityHit, character.EntityStats.AttackDamage.GetTotal()));
        //}
        //else
        //{
        entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        //} 
        UseAbility(entityHit);
    }
}
