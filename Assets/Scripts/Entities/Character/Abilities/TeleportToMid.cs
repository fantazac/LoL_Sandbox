using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToMid : Ability
{
    protected Vector3 midPosition;

    protected TeleportToMid()
    {
        CanStopMovement = true;
    }

    protected override void Start()
    {
        base.Start();

        midPosition = Vector3.up * character.transform.position.y;
        character.CharacterInput.OnPressedM += PressedInput;
    }

    protected override void PressedInput()
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Ability_TeleportToMid();
        }
        else
        {
            UseAbility();
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability_TeleportToMid()
    {
        UseAbility();
    }

    protected void SendToServer_Ability_TeleportToMid()
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_TeleportToMid", PhotonTargets.AllViaServer);
    }

    protected override void UseAbility()
    {
        character.CharacterMovement.StopAllMovement(this);
        character.transform.position = midPosition;
        character.CharacterMovement.NotifyCharacterMoved();
    }
}
