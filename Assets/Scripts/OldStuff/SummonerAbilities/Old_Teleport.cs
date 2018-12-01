using System.Collections;
using UnityEngine;

public class Old_Teleport : GroundTargetedBlink//TODO: UnitTargeted
{
    protected Old_Teleport()
    {
        abilityName = "Teleport";

        abilityType = AbilityType.BLINK;

        baseCooldown = 360;
        cooldownBeforeRecast = 0.75f;
        channelTime = 4.5f;
        delayChannelTime = new WaitForSeconds(channelTime);

        baseCooldownOnCancel = 240;

        CanBeRecasted = true;
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

        character.EntityBasicAttack.CancelCurrentBasicAttackToCastAbility();

        FinalAdjustments(destination);

        StartCorrectCoroutine();
    }

    protected override IEnumerator AbilityWithChannelTime()
    {
        UseResource();
        character.CharacterAbilityManager.BlockAllMovementAbilities(); //(this); -> so it does not block Teleport so you could cancel it, changed method since then
        IsBeingChanneled = true;

        yield return delayChannelTime;

        IsBeingChanneled = false;
        character.CharacterAbilityManager.UnblockAllMovementAbilities();
        character.CharacterOrientationManager.RotateCharacterInstantly(destinationOnCast);
        transform.position = destinationOnCast;
        character.CharacterMovementManager.NotifyCharacterMoved();

        FinishAbilityCast();
    }

    protected override void ExtraActionsOnCancel()
    {
        character.CharacterAbilityManager.UnblockAllMovementAbilities();
    }
}
