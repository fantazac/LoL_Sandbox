using UnityEngine;

public class DestroyAllDummies : AutoTargeted
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
        if (StaticObjects.OnlineMode) return;
        
        base.Start();

        spawnDummyAbilities = GetComponents<SpawnDummy>();
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
        StartAbilityCast();

        RemoveAllDummies();

        FinishAbilityCast();
    }

    private void RemoveAllDummies()
    {
        foreach (SpawnDummy spawnDummy in spawnDummyAbilities)
        {
            spawnDummy.RemoveAllDummies();
        }
    }

    protected override void SetResourcePaths() { }
}
