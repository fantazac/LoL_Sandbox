using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalStrikeChance : MonoBehaviour
{
    [SerializeField]
    private float baseCriticalStrikeChance;
    [SerializeField]
    private float currentCriticalStrikeChance;
    [SerializeField]
    private float bonusCriticalStrikeChance;

    public void SetBaseCriticalStrikeChance(float baseCriticalStrikeChance)
    {
        this.baseCriticalStrikeChance = baseCriticalStrikeChance;
        currentCriticalStrikeChance = baseCriticalStrikeChance;//change this
    }

    public float GetBaseCriticalStrikeChance()
    {
        return baseCriticalStrikeChance;
    }

    public float GetCurrentCriticalStrikeChance()
    {
        return currentCriticalStrikeChance;
    }

    public float GetBonusCriticalStrikeChance()
    {
        return bonusCriticalStrikeChance;
    }

    public string GetUIText()
    {
        return "CRITICAL STRIKE CHANCE: " + GetCurrentCriticalStrikeChance() + "% (" + GetBaseCriticalStrikeChance() + " + " + GetBonusCriticalStrikeChance() + ")";
    }
}
