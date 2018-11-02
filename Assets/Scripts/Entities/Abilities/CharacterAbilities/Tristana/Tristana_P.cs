using System.Collections.Generic;
using UnityEngine;

public class Tristana_P : PassiveTargeted
{
    private float rangePerLevel;
    private float currentRangeBonus;
    private List<Ability> abilitiesToIncreaseCastRange;

    protected Tristana_P()
    {
        abilityName = "Draw a Bead";

        abilityType = AbilityType.PASSIVE;

        AbilityLevel = 1;

        rangePerLevel = 8;

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaP";
    }

    protected override void Start()
    {
        base.Start();

        abilitiesToIncreaseCastRange = new List<Ability>();
        foreach (Ability ability in character.CharacterAbilityManager.CharacterAbilities)
        {
            if (ability is UnitTargeted)
            {
                abilitiesToIncreaseCastRange.Add(ability);
            }
        }

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override void OnCharacterLevelUp(int level)
    {
        character.EntityStatsManager.AttackRange.RemoveFlatBonus(currentRangeBonus);
        currentRangeBonus = rangePerLevel * (level - 1);
        character.EntityStatsManager.AttackRange.AddFlatBonus(currentRangeBonus);

        foreach (Ability ability in abilitiesToIncreaseCastRange)
        {
            ability.SetRange(currentRangeBonus * StaticObjects.MultiplyingFactor + character.EntityStatsManager.AttackRange.GetCurrentBaseValue());
        }
    }
}
