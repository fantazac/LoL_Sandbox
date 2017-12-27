using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPenetration : MonoBehaviour
{
    [SerializeField]
    private float baseMagicPenetrationFlat;
    [SerializeField]
    private float currentMagicPenetrationFlat;
    [SerializeField]
    private float bonusMagicPenetrationFlat;

    [SerializeField]
    private float baseMagicPenetrationPercent;
    [SerializeField]
    private float currentMagicPenetrationPercent;
    [SerializeField]
    private float bonusMagicPenetrationPercent;

    public void SetBaseMagicPenetration(float baseMagicPenetrationFlat, float baseMagicPenetrationPercent)
    {
        this.baseMagicPenetrationFlat = baseMagicPenetrationFlat;
        currentMagicPenetrationFlat = baseMagicPenetrationFlat;//change this

        this.baseMagicPenetrationPercent = baseMagicPenetrationPercent;
        currentMagicPenetrationPercent = baseMagicPenetrationPercent;//change this
    }

    public float GetBaseMagicPenetrationFlat()
    {
        return baseMagicPenetrationFlat;
    }

    public float GetCurrentMagicPenetrationFlat()
    {
        return currentMagicPenetrationFlat;
    }

    public float GetBonusMagicPenetrationFlat()
    {
        return bonusMagicPenetrationFlat;
    }

    public float GetBaseMagicPenetrationPercent()
    {
        return baseMagicPenetrationPercent;
    }

    public float GetCurrentMagicPenetrationPercent()
    {
        return currentMagicPenetrationPercent;
    }

    public float GetBonusMagicPenetrationPercent()
    {
        return bonusMagicPenetrationPercent;
    }
}
