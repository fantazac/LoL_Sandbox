using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllDummies : AutoTargeted, OtherAbility
{
    [SerializeField]
    private SpawnDummy[] spawnDummyAbilities;

    protected DestroyAllDummies()
    {
        OfflineOnly = true;
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

    public override bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager)
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
}
