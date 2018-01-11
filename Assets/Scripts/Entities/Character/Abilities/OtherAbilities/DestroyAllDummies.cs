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

    public override bool CanBeCast(Vector3 mousePosition)
    {
        return !StaticObjects.OnlineMode || !OfflineOnly;
    }

    public override Vector3 GetDestination()
    {
        return new Vector3();
    }

    protected void RemoveAllDummies()
    {
        spawnDummyAbilities[0].RemoveAllDummies();
        spawnDummyAbilities[1].RemoveAllDummies();
    }

    public override void UseAbility(Vector3 destination = default(Vector3))
    {
        RemoveAllDummies();
    }
}
