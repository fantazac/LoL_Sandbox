using UnityEngine;

public class DestroyAllDummies : AutoTargeted
{
    private SpawnDummy[] spawnDummyAbilities;

    protected DestroyAllDummies()
    {
        abilityName = "Destroy All Dummies";

        AbilityLevel = 1;
    }

    protected override void Start()
    {
        base.Start();

        spawnDummyAbilities = GetComponents<SpawnDummy>();
    }

    protected void OnDestroy()
    {
        RemoveAllDummies();
    }

    public override void UseAbility()
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
