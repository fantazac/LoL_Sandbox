using UnityEngine;

public static class AttackIsCritical
{
    public static bool CheckIfAttackIsCritical(float criticalStrikeChance)
    {
        if (criticalStrikeChance.AlmostEquals(0, 0))
        {
            return false;
        }

        if (criticalStrikeChance.AlmostEquals(100, 0))
        {
            return true;
        }

        return Random.Range(0f, 100f) <= criticalStrikeChance;
    }
}
