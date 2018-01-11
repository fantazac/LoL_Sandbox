using UnityEngine;

public class Teleport : Ability, OtherAbility
{
    protected Teleport()
    {
        CanStopMovement = true;
    }

    public override void OnPressedInput(Vector3 mousePosition)
    {
        if (CanUseSkill(mousePosition))
        {
            if (StaticObjects.OnlineMode)
            {
                SendToServer(hit.point + character.CharacterMovement.CharacterHeightOffset);
            }
            else
            {
                UseAbility(hit.point + character.CharacterMovement.CharacterHeightOffset);
            }
        }
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
