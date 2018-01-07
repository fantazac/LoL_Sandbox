using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDummy : Ability
{
    protected override void Start()
    {
        if (!StaticObjects.OnlineMode)
        {
            base.Start();

            character.CharacterInput.OnPressedN += PressedInput;
        }
    }

    protected override void PressedInput()
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Ability_SpawnDummy();
        }
        else
        {
            UseAbility();
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability_SpawnDummy()
    {
        UseAbility();
    }

    protected void SendToServer_Ability_SpawnDummy()
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_SpawnDummy", PhotonTargets.AllViaServer);
    }

    protected override void UseAbility()
    {
        GameObject dummy = (GameObject)Instantiate(Resources.Load("Dummy"), transform.position, transform.rotation);
    }
}
