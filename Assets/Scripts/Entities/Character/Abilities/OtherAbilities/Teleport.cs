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

    public override void OnPressedInput(Vector3 mousePosition)
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Ability_Teleport(mousePosition);
        }
        else
        {
            UseAbility(mousePosition);
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability_Teleport(Vector3 mousePosition)
    {
        UseAbility(mousePosition);
    }

    protected void SendToServer_Ability_Teleport(Vector3 mousePosition)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Teleport", PhotonTargets.AllViaServer, mousePosition);
    }

    protected override void UseAbility(Vector3 mousePosition)
    {
        character.CharacterMovement.StopAllMovement(this);
        // TODO: add cast delay
        if (MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit))
        {
            character.transform.position = hit.point + character.CharacterMovement.CharacterHeightOffset;
            character.CharacterMovement.NotifyCharacterMoved();
        }
    }
}
