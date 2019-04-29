using System.Collections.Generic;

public class Old_Ezreal_P : PassiveTargeted
{
    protected Old_Ezreal_P()
    {
        abilityName = "Rising Spell Force";

        abilityType = AbilityType.PASSIVE;

        MaxLevel = 3;
        AbilityLevel = 1;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealP";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = new List<Team>();
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Ezreal_P_Buff>() };

        foreach (Ability ability in champion.AbilityManager.CharacterAbilities)
        {
            ability.OnAbilityHit += PassiveEffect;
        }

        champion.LevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    private void OnCharacterLevelUp(int level)
    {
        if (level == 7 || level == 13)
        {
            LevelUp();
        }
    }
}
