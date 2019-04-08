using UnityEngine;

public class CriticalStrikeChance : Stat
{
    private const float CRITICAL_STRIKE_CHANCE_CAP = 100;

    protected override void UpdateTotal()
    {
        total = Mathf.Clamp(percentBonus, 0, CRITICAL_STRIKE_CHANCE_CAP);
    }
}
