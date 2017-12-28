using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceRegeneration : MonoBehaviour
{
    [SerializeField]
    protected ResourceType type;
    [SerializeField]
    private float baseResourceRegeneration;
    [SerializeField]
    private float currentResourceRegeneration;
    [SerializeField]
    private float bonusResourceRegenerationFlat;
    [SerializeField]
    private float bonusResourceRegenerationPercent;

    public void SetBaseResourceRegeneration(float baseResourceRegeneration)
    {
        this.baseResourceRegeneration = baseResourceRegeneration;
        currentResourceRegeneration = baseResourceRegeneration;//change this
    }

    public ResourceType GetResourceType()
    {
        return type;
    }

    public float GetBaseResourceRegeneration()
    {
        return baseResourceRegeneration;
    }

    public float GetCurrentResourceRegeneration()
    {
        return currentResourceRegeneration;//formule: baseResourceRegeneration + ((baseResourceRegeneration + bonusResourceRegenerationFlat) * bonusResourceRegenerationPercent)
    }

    public float GetBonusResourceRegenerationFlat()
    {
        return bonusResourceRegenerationFlat;
    }

    public float GetBonusResourceRegenerationPercent()
    {
        return bonusResourceRegenerationPercent;
    }

    public string GetUIText()
    {
        return GetResourceType() + " REGENERATION: " + GetCurrentResourceRegeneration() + " (" + GetBaseResourceRegeneration() + " + ((" + GetBaseResourceRegeneration() +
                " + " + GetBonusResourceRegenerationFlat() + ") * " + GetBonusResourceRegenerationPercent() + "%))";
    }
}
