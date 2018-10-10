﻿public class Lethality : Stat
{
    private int characterLevel;

    public override void UpdateTotal()
    {
        total = currentBaseValue + flatBonus;
    }

    public override void OnLevelUp(int level)
    {
        characterLevel = level;

        base.OnLevelUp(level);
    }

    public float GetCurrentValue()
    {
        return total * (0.6f + ((0.4f * characterLevel) / 18));
    }
}
