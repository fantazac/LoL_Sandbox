using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicResistance : MonoBehaviour
{
    [SerializeField]
    private float baseMagicResistance;
    [SerializeField]
    private float currentMagicResistance;
    [SerializeField]
    private float bonusMagicResistance;
    [SerializeField]
    private float magicDamageTakenMultiplier;

    public void SetBaseMagicResistance(float baseMagicResistance)
    {
        this.baseMagicResistance = baseMagicResistance;
        currentMagicResistance = baseMagicResistance;//change this
        magicDamageTakenMultiplier = 1f - (currentMagicResistance / (100f + currentMagicResistance));
    }

    public float GetBaseMagicResistance()
    {
        return baseMagicResistance;
    }

    public float GetCurrentMagicResistance()
    {
        return currentMagicResistance;
    }

    public float GetBonusMagicResistance()
    {
        return bonusMagicResistance;
    }

    public float GetMagicDamageTakenMultiplier()
    {
        return magicDamageTakenMultiplier;
    }

    public float GetMagicDamageReductionPercent()
    {
        return (1f - magicDamageTakenMultiplier) * 100;
    }

    public string GetUIText()
    {
        return "MAGIC RESISTANCE: " + GetCurrentMagicResistance() + " (" + GetBaseMagicResistance() + " + " + GetBonusMagicResistance() + 
            ") - Takes " + (int)GetMagicDamageReductionPercent() + "% reduced magic damage";
    }
}
