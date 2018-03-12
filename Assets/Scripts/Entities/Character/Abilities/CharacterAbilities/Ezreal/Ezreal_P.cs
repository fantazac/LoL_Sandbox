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
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealP";
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealP_Buff";
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

        character.CharacterLevelManager.OnLevelUp += OnLevelUp;
    }

    public override void OnLevelUp(int level)
    {
        if (level == 7)
        {
            UpdateBuffOnAffectedEntities(buffPercentBonus, 12);
            buffPercentBonus = 12;
        }
        else if (level == 13)
        {
            UpdateBuffOnAffectedEntities(buffPercentBonus, 14);
            buffPercentBonus = 14;
        }
    }

    protected override void UpdateBuffOnAffectedEntities(float oldValue, float newValue)
    {
        foreach(Entity affectedEntity in EntitiesAffectedByBuff)
        {
            Buff buff = affectedEntity.EntityBuffManager.GetBuff(this);
            int currentStacks = 0;
            if (buff != null)
            {
                currentStacks = buff.CurrentStacks;
                affectedEntity.EntityStats.AttackSpeed.RemovePercentBonus(oldValue * currentStacks);
                affectedEntity.EntityStats.AttackSpeed.AddPercentBonus(newValue * currentStacks);
            }
        }
    }

    protected override void AddNewBuffToEntityHit(Entity entityHit)
    {
        Buff buff = entityHit.EntityBuffManager.GetBuff(this);
        if (buff == null)
        {
            buff = new Buff(this, entityHit, false, buffDuration, buffMaximumStacks);
            entityHit.EntityBuffManager.ApplyBuff(buff, buffSprite);
        }
        else
        {
            buff.IncreaseCurrentStacks();
            buff.ResetDurationRemaining();
        }
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.AttackSpeed.AddPercentBonus(buffPercentBonus * currentStacks);
        EntitiesAffectedByBuff.Add(entityHit);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.AttackSpeed.RemovePercentBonus(buffPercentBonus * currentStacks);
        EntitiesAffectedByBuff.Remove(entityHit);
    }
}
