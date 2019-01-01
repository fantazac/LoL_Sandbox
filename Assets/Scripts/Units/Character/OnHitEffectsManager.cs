using UnityEngine;

public class OnHitEffectsManager : MonoBehaviour
{
    private Unit unit;
    private PercentBonusOnlyStat lifeSteal;

    public delegate void OnApplyOnHitEffectsHandler(Unit unitHit, float damage);
    public event OnApplyOnHitEffectsHandler OnApplyOnHitEffects;

    private void Start()
    {
        unit = GetComponent<Unit>();
        lifeSteal = unit.StatsManager.LifeSteal;
    }

    public void ApplyOnHitEffectsToUnitHit(Unit unitHit, float damage)
    {
        if (OnApplyOnHitEffects != null)
        {
            OnApplyOnHitEffects(unitHit, damage);
        }
        unit.StatsManager.RestoreHealth(damage * lifeSteal.GetTotal());
    }
}
