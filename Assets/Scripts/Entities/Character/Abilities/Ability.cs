using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Character character;

    public bool CanStopMovement { get; protected set; }

    protected virtual void Start()
    {
        character = GetComponent<Character>();
        CanStopMovement = false;
    }

    protected void PressedInput()
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_Ability();
        }
        else
        {
            UseAbility();
        }
    }

    protected void SendToServer_Ability()
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability", PhotonTargets.AllViaServer);
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability()
    {
        UseAbility();
    }

    protected virtual void UseAbility() { }
}
