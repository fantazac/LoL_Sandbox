using System.Collections.Generic;
using UnityEngine;

public class MovementSpeed : Stat
{
    private const float LOW_CAP = 220;
    private const float FIRST_CAP = 415;
    private const float SECOND_CAP = 490;

    private float multiplicativePercentBonus;
    private float currentSlowResistance;

    private List<float> slows = new List<float>();//This needs to be initialized before the constructor if we don't want to add a null check in GetBiggestSlow.

    public MovementSpeed(float initialBaseValue) : base(initialBaseValue) { }

    public void SubscribeToSlowResistanceChangedEvent(SlowResistance slowResistance)
    {
        slowResistance.OnSlowResistanceChanged += OnSlowResistanceChanged;
    }

    public override float GetTotal()
    {
        return base.GetTotal() * StaticObjects.MultiplyingFactor;
    }

    public override void UpdateTotal()
    {
        total = (currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f)) * (1 - (GetBiggestSlow() * 0.01f * (1 - currentSlowResistance))) * (1 + (multiplicativePercentBonus * 0.01f));

        if (total > SECOND_CAP)
        {
            total = (total * 0.5f) + 230;
        }
        else if (total > FIRST_CAP)
        {
            total = (total * 0.8f) + 83;
        }
        else if (total < LOW_CAP)
        {
            total = (total * 0.5f) + 110;
        }
    }

    private void OnSlowResistanceChanged(float slowResistance)
    {
        currentSlowResistance = slowResistance * 0.01f;
        UpdateTotal();
    }

    public float GetBiggestSlow()
    {
        float biggestSlow = 0;
        foreach (float slow in slows)
        {
            if (slow > biggestSlow)
            {
                biggestSlow = slow;
            }
        }
        return biggestSlow;
    }

    protected override void CalculatePercentMalusIncrease(float percentMalus)
    {
        slows.Add(percentMalus);
        UpdateTotal();
    }

    protected override void CalculatePercentMalusReduction(float percentMalus)
    {
        slows.Remove(percentMalus);
        UpdateTotal();
    }

    public float GetMultiplicativePercentBonus()
    {
        return multiplicativePercentBonus;
    }

    private void AddMultiplicativePercentBonus(float multiplicativePercentBonus)
    {
        this.multiplicativePercentBonus += multiplicativePercentBonus;
        UpdateTotal();
    }

    private void RemoveMultiplicativePercentBonus(float multiplicativePercentBonus)
    {
        this.multiplicativePercentBonus -= multiplicativePercentBonus;
        UpdateTotal();
    }
}
