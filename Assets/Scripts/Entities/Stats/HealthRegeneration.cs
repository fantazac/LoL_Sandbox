using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegeneration : MonoBehaviour //the value of this stat is: per 5 seconds of play, you regenerate this much health
{
    [SerializeField]
    private float baseHealthRegeneration;
    [SerializeField]
    private float currentHealthRegeneration;
    [SerializeField]
    private float bonusHealthRegenerationFlat;
    [SerializeField]
    private float bonusHealthRegenerationPercent;

    public void SetBaseHealthRegeneration(float baseHealthRegeneration)
    {
        this.baseHealthRegeneration = baseHealthRegeneration;
        currentHealthRegeneration = baseHealthRegeneration;//change this
    }

    public float GetBaseHealthRegeneration()
    {
        return baseHealthRegeneration;
    }

    public float GetCurrentHealthRegeneration()
    {
        return currentHealthRegeneration;//formule: baseHealthRegeneration + ((baseHealthRegeneration + bonusHealthRegenerationFlat) * bonusHealthRegenerationPercent)
    }

    public float GetBonusHealthRegenerationFlat()
    {
        return bonusHealthRegenerationFlat;
    }

    public float GetBonusHealthRegenerationPercent()
    {
        return bonusHealthRegenerationPercent;
    }

    public string GetUIText()
    {
        return "HEALTH REGENERATION: " + GetCurrentHealthRegeneration() + " (" + GetBaseHealthRegeneration() + " + ((" + GetBaseHealthRegeneration() + 
            " + " + GetBonusHealthRegenerationFlat() + ") * " + GetBonusHealthRegenerationPercent() + "%))";
    }
}
