using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDummy : Ability
{
    protected const int MAXIMUM_DUMMY_AMOUNT = 3;

    protected List<GameObject> dummies;

    private RaycastHit hit;

    protected SpawnDummy()
    {
        dummies = new List<GameObject>();
    }

    protected override void Start()
    {
        if (!StaticObjects.OnlineMode)
        {
            base.Start();

            character.CharacterInput.OnPressedN += PressedInput;
        }
    }

    protected void OnDestroy()
    {
        while (dummies.Count > 0)//more efficient way to do this?
        {
            Destroy(dummies[0]);
            dummies.RemoveAt(0);
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
        if(MousePositionOnTerrain.GetRaycastHit(Input.mousePosition, out hit))
        {
            if (dummies.Count == MAXIMUM_DUMMY_AMOUNT)
            {
                Destroy(dummies[0]);
                dummies.RemoveAt(0);
            }
            GameObject dummy = (GameObject)Instantiate(Resources.Load("Dummy"), hit.point + character.CharacterMovement.CharacterHeightOffset, Quaternion.identity);
            dummy.transform.rotation = Quaternion.LookRotation((transform.position - dummy.transform.position).normalized);
            dummies.Add(dummy);
        }
    }
}
