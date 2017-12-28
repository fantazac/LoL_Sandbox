using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownReduction : MonoBehaviour
{
    [SerializeField]
    private float baseCooldownReduction;
    [SerializeField]
    private float currentCooldownReduction;
    [SerializeField]
    private float bonusCooldownReduction;
    [SerializeField]
    private float cooldownReductionMultiplier;

    public void SetBaseCooldownReduction(float baseCooldownReduction)
    {
        this.baseCooldownReduction = baseCooldownReduction;
        currentCooldownReduction = baseCooldownReduction;//change this
        cooldownReductionMultiplier = currentCooldownReduction / 100f;
    }

    public float GetBaseCooldownReduction()
    {
        return baseCooldownReduction;
    }

    public float GetCurrentCooldownReduction()
    {
        return currentCooldownReduction;
    }

    public float GetBonusCooldownReduction()
    {
        return bonusCooldownReduction;
    }

    public float GetCooldownReductionMultiplier()
    {
        return cooldownReductionMultiplier;
    }

    public string GetUIText()
    {
        return "COOLDOWN REDUCTION: " + GetCurrentCooldownReduction() + "% (" + GetBaseCooldownReduction() + " + " + GetBonusCooldownReduction() + ")";
    }
}
