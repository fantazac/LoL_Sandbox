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

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealP";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Ezreal_P_Buff>() };

        foreach (Ability ability in character.CharacterAbilityManager.CharacterAbilities)
        {
            ability.OnAbilityHit += PassiveEffect;
        }

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override void OnCharacterLevelUp(int level)
    {
        if (level == 7 || level == 13)
        {
            LevelUp();
        }
    }
}
