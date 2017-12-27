using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tenacity : MonoBehaviour
{
    [SerializeField]
    private float baseTenacity;
    [SerializeField]
    private float currentTenacity;
    [SerializeField]
    private float bonusTenacity;

    public void SetBaseTenacity(float baseTenacity)
    {
        this.baseTenacity = baseTenacity;
        currentTenacity = baseTenacity;//change this
    }

    public float GetBaseTenacity()
    {
        return baseTenacity;
    }

    public float GetCurrentTenacity()
    {
        return currentTenacity;
    }

    public float GetBonusTenacity()
    {
        return bonusTenacity;
    }
}
