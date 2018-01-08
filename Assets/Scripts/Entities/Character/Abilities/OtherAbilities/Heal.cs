using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Ability, OtherAbility {

    protected override void Start()
    {
        base.Start();
    }

    public override void OnPressedInput()
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Ability_Heal();
        }
        else
        {
            UseAbility();
        }
    }

    protected void SendToServer_Ability_Heal()
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Heal", PhotonTargets.AllViaServer);
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability_Heal()
    {
        UseAbility();
    }

    protected override void UseAbility()
    {
        Health characterHealth = GetComponent<CharacterStats>().Health;
        characterHealth.Heal(150);
    }
}
