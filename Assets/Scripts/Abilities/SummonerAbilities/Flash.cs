using System.Collections.Generic;
using UnityEngine;

public class Flash : GroundTargetedBlink
{
    protected Flash()
    {
        abilityName = "Flash";

        abilityType = AbilityType.BLINK;

        range = 400;
        baseCooldown = 300;

        CanBeCastDuringOtherAbilityCastTimes = true;
        IsAMovementAbility = true;

        AbilityLevel = 1;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Flash";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = new List<Team>();
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        champion.ChampionMovementManager.StopMovementTowardsPoint();

        Vector3 newDestination = FindGroundPoint(destination, transform.position);
        Quaternion newRotation = Quaternion.LookRotation((destination - transform.position).normalized);

        transform.position = newDestination;
        champion.DisplacementManager.StopCurrentDisplacement();

        if (champion.AbilityManager.CanRotate())
        {
            transform.rotation = newRotation;
        }

        champion.ChampionMovementManager.NotifyChampionMoved();

        FinishAbilityCast();
    }
}
