using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllDummies : AutoTargeted, OfflineAbility
{
    private SpawnDummy[] spawnDummyAbilities;

    protected DestroyAllDummies()
    {
        abilityName = "Destroy All Dummies";

        OfflineOnly = true;

        IsEnabled = true;
    }

    protected override void Start()
    {
        if (!StaticObjects.OnlineMode)
        {
            base.Start();

            spawnDummyAbilities = GetComponents<SpawnDummy>();
        }
    }

    protected void OnDestroy()
    {
        RemoveAllDummies();
    }

    public override bool CanBeCast(Vector3 mousePosition)
    {
        return !StaticObjects.OnlineMode || !OfflineOnly; // Is !(StaticObjects.OnlineMode || OfflineOnly) better?
    }

    public override void UseAbility(Vector3 destination)
    {
        RemoveAllDummies();
    }

    protected void RemoveAllDummies()
    {
        foreach (SpawnDummy spawnDummy in spawnDummyAbilities)
        {
            spawnDummy.RemoveAllDummies();
        }
    }

    protected override void SetResourcePaths() { }
}
