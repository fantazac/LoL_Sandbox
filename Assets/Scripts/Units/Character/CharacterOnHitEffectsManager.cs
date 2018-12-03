using UnityEngine;

public class CharacterOnHitEffectsManager : MonoBehaviour
{
    private Character character;
    private PercentBonusOnlyStat lifeSteal;

    public delegate void OnApplyOnHitEffectsHandler(Unit unitHit, float damage);
    public event OnApplyOnHitEffectsHandler OnApplyOnHitEffects;

    private void Start()
    {
        character = GetComponent<Character>();
        lifeSteal = character.StatsManager.LifeSteal;
    }

    public void ApplyOnHitEffectsToUnitHit(Unit unitHit, float damage)
    {
        if (OnApplyOnHitEffects != null)
        {
            OnApplyOnHitEffects(unitHit, damage);
        }
        character.StatsManager.RestoreHealth(damage * lifeSteal.GetTotal());
    }
}
