using System;
using UnityEngine;

public class Teleport : Ability, OtherAbility {

    protected RaycastHit hit;

    protected Teleport()
    {
        CanStopMovement = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void OnPressedInput()
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Ability_Teleport();
        }
        else
        {
            UseAbility();
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability_Teleport()
    {
        UseAbility();
    }

    protected void SendToServer_Ability_Teleport()
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Teleport", PhotonTargets.AllViaServer);
    }

    protected override void UseAbility()
    {
        character.CharacterMovement.StopAllMovement(this);
        // TODO: add cast delay
        if (MousePositionOnTerrain.GetRaycastHit(Input.mousePosition, out hit))
        {
            character.transform.position = hit.point + character.CharacterMovement.CharacterHeightOffset;
            character.CharacterMovement.NotifyCharacterMoved();
        }
    }
}
