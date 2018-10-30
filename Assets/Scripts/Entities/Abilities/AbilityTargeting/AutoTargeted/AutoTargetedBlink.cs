using UnityEngine;

public abstract class AutoTargetedBlink : AutoTargeted
{
    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        character.CharacterMovementManager.StopAllMovement();

        StartCorrectCoroutine();
    }
}
