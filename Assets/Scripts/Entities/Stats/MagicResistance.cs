using UnityEngine;

public class MagicResistance : Stat
{
    private float GetMagicDamageTakenMultiplier()
    {
        if (total >= 0)
        {
            return 100 / (100 + total);
        }
        else
        {
            return 2 - 100 / (100 - total);
        }
    }

    public float GetMagicEffectiveHealthPercent()
    {
        return 1 + (GetMagicDamageReductionPercent() / GetMagicDamageTakenMultiplier());
    }

    public float GetMagicDamageReductionPercent()
    {
        return 1 - GetMagicDamageTakenMultiplier();
    }
}
