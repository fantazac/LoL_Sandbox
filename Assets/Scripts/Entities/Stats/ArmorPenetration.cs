using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPenetration : MonoBehaviour
{
    [SerializeField]
    private float baseArmorPenetrationFlat;
    [SerializeField]
    private float currentArmorPenetrationFlat;
    [SerializeField]
    private float bonusArmorPenetrationFlat;

    [SerializeField]
    private float baseArmorPenetrationPercent;
    [SerializeField]
    private float currentArmorPenetrationPercent;
    [SerializeField]
    private float bonusArmorPenetrationPercent;

    public void SetBaseArmorPenetration(float baseArmorPenetrationFlat, float baseArmorPenetrationPercent)
    {
        this.baseArmorPenetrationFlat = baseArmorPenetrationFlat;
        currentArmorPenetrationFlat = baseArmorPenetrationFlat;//change this

        this.baseArmorPenetrationPercent = baseArmorPenetrationPercent;
        currentArmorPenetrationPercent = baseArmorPenetrationPercent;//change this
    }

    public float GetBaseArmorPenetrationFlat()
    {
        return baseArmorPenetrationFlat;
    }

    public float GetCurrentArmorPenetrationFlat()
    {
        return currentArmorPenetrationFlat;
    }

    public float GetBonusArmorPenetrationFlat()
    {
        return bonusArmorPenetrationFlat;
    }

    public float GetBaseArmorPenetrationPercent()
    {
        return baseArmorPenetrationPercent;
    }

    public float GetCurrentArmorPenetrationPercent()
    {
        return currentArmorPenetrationPercent;
    }

    public float GetBonusArmorPenetrationPercent()
    {
        return bonusArmorPenetrationPercent;
    }

    public string GetUIText()
    {
        return "ARMOR PENETRATION: " + GetCurrentArmorPenetrationFlat() + " (" + GetBaseArmorPenetrationFlat() + " + " + GetBonusArmorPenetrationFlat() +
            ") | " + GetCurrentArmorPenetrationPercent() + "% (" + GetBaseArmorPenetrationPercent() + "% + " + GetBonusArmorPenetrationPercent() + "%)";
    }
}
