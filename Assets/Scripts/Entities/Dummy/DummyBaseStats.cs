using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()
    {
        BaseHealth = 10000;

        BaseArmor = 100;
        BaseMagicResistance = 100;
        BaseAttackSpeed = 0.658f;
    }
}
