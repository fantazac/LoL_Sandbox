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

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Flash";
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.CharacterMovement.StopMovementTowardsPoint();

        Vector3 newDestination = FindPointToMoveTo(destination, transform.position);
        Quaternion newRotation = Quaternion.LookRotation((destination - transform.position).normalized);

        transform.position = newDestination;
        character.EntityDisplacementManager.StopCurrentDisplacement();

        if (character.CharacterAbilityManager.CanRotate())
        {
            transform.rotation = newRotation;
        }

        character.CharacterMovement.NotifyCharacterMoved();

        FinishAbilityCast();
    }
}
