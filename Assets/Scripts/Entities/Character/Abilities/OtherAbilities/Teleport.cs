using UnityEngine;

public class Teleport : GroundTargetedBlink, OtherAbility
{
    public override void UseAbility(Vector3 destination)
    {
        character.CharacterOrientation.RotateCharacterInstantly(destination);

        transform.position = destination;
        character.CharacterMovement.NotifyCharacterMoved();
        // TODO: add cast delay (channel time)
    }
}
