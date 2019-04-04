using System.Collections.Generic;

public class Tristana_P : PassiveTargeted
{
    private readonly float rangePerLevel;
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

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = new List<Team>();
    }

    protected override void Start()
    {
        base.Start();

        abilitiesToIncreaseCastRange = new List<Ability>();
        foreach (Ability ability in champion.AbilityManager.CharacterAbilities)
        {
            if (ability is UnitTargeted)
            {
                abilitiesToIncreaseCastRange.Add(ability);
            }
        }

        champion.LevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override void OnCharacterLevelUp(int level)
    {
        champion.StatsManager.AttackRange.RemoveFlatBonus(currentRangeBonus);
        currentRangeBonus = rangePerLevel * (level - 1);
        champion.StatsManager.AttackRange.AddFlatBonus(currentRangeBonus);

        foreach (Ability ability in abilitiesToIncreaseCastRange)
        {
            ability.SetRange(currentRangeBonus + champion.StatsManager.AttackRange.GetCurrentBaseValue());
        }
    }
}
