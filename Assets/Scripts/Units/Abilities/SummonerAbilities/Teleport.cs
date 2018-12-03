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

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.BasicAttack.CancelCurrentBasicAttackToCastAbility();

        FinalAdjustments(destination);

        StartCorrectCoroutine();
    }

    protected override IEnumerator AbilityWithChannelTime()
    {
        UseResource();
        character.CharacterAbilityManager.BlockAllMovementAbilities();
        IsBeingChanneled = true;

        yield return delayChannelTime;

        IsBeingChanneled = false;
        character.CharacterAbilityManager.UnblockAllMovementAbilities();
        character.CharacterOrientationManager.RotateCharacterInstantly(destinationOnCast);
        transform.position = destinationOnCast;
        character.CharacterMovementManager.NotifyCharacterMoved();

        FinishAbilityCast();
    }
}
