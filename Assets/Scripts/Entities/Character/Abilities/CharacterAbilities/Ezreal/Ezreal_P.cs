using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_P : PassiveTargeted, CharacterAbility, PassiveCharacterAbility
{
    protected Ezreal_P()
    {
        buffDuration = 6;
        buffMaximumStacks = 5;
        buffPercentBonus = 10;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Ezreal/EzrealP";
        buffSpritePath = "Sprites/CharacterAbilities/Ezreal/EzrealP_Buff";
    }

    protected override void Start()
    {
        base.Start();

        foreach (Ability ability in GetComponents<CharacterAbility>())
        {
            if (!(ability is PassiveCharacterAbility))
            {
                ability.OnAbilityHit += PassiveEffect;
            }
        }
    }

    protected override void AddNewBuffToEntityHit(Entity entityHit)
    {
        if (passive == null || passive.HasExpired())
        {
            passive = new Buff(this, character, buffDuration, buffMaximumStacks);
            entityHit.EntityBuffManager.ApplyBuff(passive, buffSprite);
        }
        else
        {
            passive.IncreaseCurrentStacks();
            passive.ResetDurationRemaining();
        }
    }

    public override void ApplyBuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.AttackSpeed.AddPercentBonus(buffPercentBonus * passive.CurrentStacks);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.AttackSpeed.RemovePercentBonus(buffPercentBonus * passive.CurrentStacks);
    }
}
