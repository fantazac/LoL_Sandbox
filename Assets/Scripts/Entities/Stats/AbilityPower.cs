using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPower : MonoBehaviour
{
    [SerializeField]
    private float baseAbilityPower;
    [SerializeField]
    private float currentAbilityPower;
    [SerializeField]
    private float bonusAbilityPower;

    public void SetBaseAbilityPower(float baseAbilityPower)
    {
        this.baseAbilityPower = baseAbilityPower;
        currentAbilityPower = baseAbilityPower;//change this
    }

    public float GetBaseAbilityPower()
    {
        return baseAbilityPower;
    }

    public float GetCurrentAbilityPower()
    {
        return currentAbilityPower;
    }

    public float GetBonusAbilityPower()
    {
        return bonusAbilityPower;
    }

    public string GetUIText()
    {
        return "ABILITY POWER: " + GetCurrentAbilityPower() + " (" + GetBaseAbilityPower() + " + " + GetBonusAbilityPower() + ")";
    }
}
