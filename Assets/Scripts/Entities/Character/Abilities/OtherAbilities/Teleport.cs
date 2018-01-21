using UnityEngine;

public class Teleport : GroundTargetedBlink, OtherAbility
{
    protected Teleport()
    {
        CanStopMovement = true;
    }

    public override void UseAbility(Vector3 destination)
    {
        character.CharacterMovement.StopAllMovement(this);
        character.CharacterOrientation.RotateCharacterInstantly(destination);

        transform.position = destination;
        character.CharacterMovement.NotifyCharacterMoved();
        // TODO: add cast delay (channel time)
    }
}
