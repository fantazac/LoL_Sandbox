using System.Collections;
using UnityEngine;

public class Teleport : GroundTargetedBlink//TODO: UnitTargeted
{
    protected Teleport()
    {
        abilityName = "Teleport";

        abilityType = AbilityType.BLINK;

        baseCooldown = 360;
        channelTime = 4f;
        delayChannelTime = new WaitForSeconds(channelTime);

        CannotCastAnyAbilityWhileActive = true;
        IsAMovementAbility = true;

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Teleport";
        abilityRecastSpritePath = "Sprites/Characters/SummonerAbilities/Teleport";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetAllyTeam(allyTeam);
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        champion.BasicAttack.CancelCurrentBasicAttackToCastAbility();

        FinalAdjustments(destination);

        StartCorrectCoroutine();
    }

    protected override IEnumerator AbilityWithChannelTime()
    {
        UseResource();
        champion.AbilityManager.BlockAllMovementAbilities();
        IsBeingChanneled = true;

        yield return delayChannelTime;

        IsBeingChanneled = false;
        champion.AbilityManager.UnblockAllMovementAbilities();
        champion.OrientationManager.RotateCharacterInstantly(destinationOnCast);
        transform.position = destinationOnCast;
        champion.ChampionMovementManager.NotifyChampionMoved();

        FinishAbilityCast();
    }
}
