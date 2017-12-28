using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource : MonoBehaviour
{
    [SerializeField]
    protected ResourceType type;
    [SerializeField]
    protected float baseResource;
    [SerializeField]
    protected float currentResource;
    [SerializeField]
    protected float bonusResource;
    [SerializeField]
    protected float maxResource;

    public void SetBaseResource(float baseResource)
    {
        this.baseResource = baseResource;
        maxResource = baseResource;
        currentResource = maxResource;//change this
    }

    public ResourceType GetResourceType()
    {
        return type;
    }

    public float GetBaseResource()
    {
        return baseResource;
    }

    public float GetCurrentResource()
    {
        return currentResource;
    }

    public float GetBonusResource()
    {
        return bonusResource;
    }

    public float GetMaximumResource()
    {
        return maxResource;
    }

    public float GetResourcePercent()
    {
        return currentResource / maxResource;
    }

    public string GetUIText()
    {
        return GetResourceType() + ": " + GetCurrentResource() + " / " + GetMaximumResource() + " (" + GetBaseResource() + " + " + GetBonusResource() + ")";
    }
}

public enum ResourceType
{
    ENERGY,
    FURY,
    MANA,
}
