using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToMid : Ability
{
    protected Vector3 midPosition;

    protected override void Start()
    {
        base.Start();

        midPosition = Vector3.up * character.transform.position.y;
        character.CharacterInput.OnPressedM += PressedInput;
        CanStopMovement = true;
    }

    protected override void UseAbility()
    {
        character.CharacterMovement.StopAllMovement(this);
        character.transform.position = midPosition;
        character.CharacterMovement.NotifyCharacterMoved();
    }
}
