using UnityEngine;

public class CriticalStrikeChance : Stat
{
    private const float CRITICAL_STRIKE_CHANCE_CAP = 100;

    public CriticalStrikeChance() : base() { }

    public override void UpdateTotal()
    {
        total = Mathf.Clamp(currentBaseValue + percentBonus, 0, CRITICAL_STRIKE_CHANCE_CAP);
    }
}
