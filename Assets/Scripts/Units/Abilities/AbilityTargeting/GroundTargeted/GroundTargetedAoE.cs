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
        else if (!champion.StatusManager.CrowdControlEffectsOnCharacter.Contains(CrowdControlEffect.ROOT))
        {
            champion.MovementManager.SetMoveTowardsPoint(destination, range);
            champion.MovementManager.CharacterIsInDestinationRange += base.UseAbility;
        }
    }
}
