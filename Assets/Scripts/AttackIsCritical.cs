using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttackIsCritical
{
    public static bool CheckIfAttackIsCritical(float criticalStrikeChance)
    {
        if(criticalStrikeChance == 0)
        {
            return false;
        }
        if(criticalStrikeChance == 100)
        {
            return true;
        }
        return Random.Range(0f, 100f) <= criticalStrikeChance;
    }
}
