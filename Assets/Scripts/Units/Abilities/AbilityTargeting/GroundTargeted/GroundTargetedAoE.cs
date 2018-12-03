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
        else if (!character.StatusManager.CrowdControlEffectsOnCharacter.Contains(CrowdControlEffect.ROOT))
        {
            character.MovementManager.SetMoveTowardsPoint(destination, range);
            character.MovementManager.CharacterIsInDestinationRange += base.UseAbility;
        }
    }
}
