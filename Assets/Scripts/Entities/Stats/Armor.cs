using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField]
    private float baseArmor;
    [SerializeField]
    private float currentArmor;
    [SerializeField]
    private float bonusArmor;
    [SerializeField]
    private float physicalDamageTakenMultiplier;

    public void SetBaseArmor(float baseArmor)
    {
        this.baseArmor = baseArmor;
        currentArmor = baseArmor;//change this
        physicalDamageTakenMultiplier = 1f - (currentArmor / (100f + currentArmor));
    }

    public float GetBaseArmor()
    {
        return baseArmor;
    }

    public float GetCurrentArmor()
    {
        return currentArmor;
    }

    public float GetBonusArmor()
    {
        return bonusArmor;
    }

    public float GetPhysicalDamageTakenMultiplier()
    {
        return physicalDamageTakenMultiplier;
    }

    public float GetPhysicalDamageReductionPercent()
    {
        return (1f - physicalDamageTakenMultiplier) * 100;
    }
}
