using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllDummies : AutoTargeted, OfflineAbility
{
    [SerializeField]
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
        spawnDummyAbilities[0].RemoveAllDummies();
        spawnDummyAbilities[1].RemoveAllDummies();
    }

    protected override void SetSpritePaths() { }
}
