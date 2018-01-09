using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllDummies : Ability, OtherAbility
{
    [SerializeField]
    private SpawnDummy[] spawnDummyAbilities;

    protected DestroyAllDummies()
    {
        OfflineOnly = true;
    }

    protected void OnDestroy()
    {
        RemoveAllDummies();
    }

    public override void OnPressedInput(Vector3 mousePosition)
    {
        UseAbility();
    }

    protected void RemoveAllDummies()
    {
        spawnDummyAbilities[0].RemoveAllDummies();
        spawnDummyAbilities[1].RemoveAllDummies();
    }

    protected override void UseAbility(Vector3 destination = default(Vector3))
    {
        RemoveAllDummies();
    }
}
