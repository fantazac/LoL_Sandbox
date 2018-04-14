using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_P : PassiveTargeted, PassiveCharacterAbility
{
    protected Ezreal_P()
    {
        abilityName = "Rising Spell Force";

        abilityType = AbilityType.Passive;

        MaxLevel = 3;
        AbilityLevel = 1;

        buffDuration = 6;
        buffMaximumStacks = 5;
        buffPercentBonus = 10;
        buffPercentBonusPerLevel = 2;

        IsEnabled = true;
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

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override void OnCharacterLevelUp(int level)
    {
        if (level == 7 || level == 13)
        {
            float oldValue = buffPercentBonus;
            LevelUp();
            UpdateBuffOnAffectedEntities(oldValue, buffPercentBonus);
        }
    }

    protected override void UpdateBuffOnAffectedEntities(float oldValue, float newValue)
    {
        foreach (Entity affectedEntity in EntitiesAffectedByBuff)
        {
            Buff buff = affectedEntity.EntityBuffManager.GetBuff(this);
            if (buff != null)
            {
                int currentStacks = buff.CurrentStacks;
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
