using System.Collections.Generic;

public class Lucian_E : DirectionTargetedDash
{
    protected Lucian_E()
    {
        abilityName = "Relentless Pursuit";

        abilityType = AbilityType.DASH;

        MaxLevel = 5;

        range = 425;
        resourceCost = 40; // 40/30/20/10/0
        resourceCostPerLevel = -10;
        baseCooldown = 22; // 22/20/18/16/14
        baseCooldownPerLevel = -2;
        minimumDistanceTravelled = 200;
        dashSpeed = 32;

        IsAMovementAbility = true;
        resetBasicAttackCycleOnAbilityFinished = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianE";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = new List<Team>();
    }
}
