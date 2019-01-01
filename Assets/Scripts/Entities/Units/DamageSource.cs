using UnityEngine;

public class DamageSource : MonoBehaviour
{
    protected DamageType damageType;

    public delegate void OnKilledUnitHandler(DamageSource damageSource, Unit killedUnit);
    public event OnKilledUnitHandler OnKilledUnit;

    protected void DamageUnit(Unit unitToDamage, float damage)
    {
        DamageUnit(unitToDamage, damageType, damage);
    }

    protected void DamageUnit(Unit unitToDamage, DamageType damageType, float damage)
    {
        unitToDamage.StatsManager.ReduceHealth(this, damageType, damage);
    }

    public void KilledUnit(Unit killedUnit)
    {
        if (OnKilledUnit != null)
        {
            OnKilledUnit(this, killedUnit);
        }
    }
}
