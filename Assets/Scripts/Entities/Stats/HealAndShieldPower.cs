﻿public class HealAndShieldPower : Stat
{
    public override void UpdateTotal()
    {
        total = percentBonus * 0.01f;
    }
}
