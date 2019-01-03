using System.Collections.Generic;

public class Ezreal_P : PassiveTargeted
{
    protected Ezreal_P()
    {
        abilityName = "Rising Spell Force";

        abilityType = AbilityType.PASSIVE;

        AbilityLevel = 1;

        IsEnabled = true;
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
    }
}
