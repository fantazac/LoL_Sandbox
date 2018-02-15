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
        if (buff == null || buff.HasExpired())
        {
            buff = new Buff(this, character, false, buffDuration, buffMaximumStacks);
            entityHit.EntityBuffManager.ApplyBuff(buff, buffSprite);
        }
        else
        {
            buff.IncreaseCurrentStacks();
            buff.ResetDurationRemaining();
        }
    }

    public override void ApplyBuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.AttackSpeed.AddPercentBonus(buffPercentBonus * buff.CurrentStacks);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.AttackSpeed.RemovePercentBonus(buffPercentBonus * buff.CurrentStacks);
    }
}
