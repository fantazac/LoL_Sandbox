using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToMid : Ability, OtherAbility
{
    protected TeleportToMid()
    {
        CanStopMovement = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void OnPressedInput(Vector3 mousePosition)
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

    protected override void UseAbility(Vector3 destination = default(Vector3))
    {
        character.CharacterMovement.StopAllMovement(this);
        character.transform.position = character.CharacterMovement.CharacterHeightOffset;
        character.CharacterMovement.NotifyCharacterMoved();
    }
}
