using UnityEngine;

public abstract class GroundTargetedAoE : GroundTargeted
{
    protected string areaOfEffectPrefabPath;
    protected GameObject areaOfEffectPrefab;

    protected override void LoadPrefabs()
    {
        areaOfEffectPrefab = Resources.Load<GameObject>(areaOfEffectPrefabPath);
    }

    public override void UseAbility(Vector3 destination)
    {
        if (Vector3.Distance(destination, transform.position) <= range)
        {
            base.UseAbility(destination);
        }
        else if (!champion.StatusManager.StatusEffectsOnCharacter.Contains(StatusEffect.ROOT))
        {
            champion.ChampionMovementManager.SetMoveTowardsPoint(destination, range);
            champion.ChampionMovementManager.OnChampionIsInDestinationRange += base.UseAbility;
        }
    }
}
