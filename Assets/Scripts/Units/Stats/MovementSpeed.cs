﻿using System.Collections.Generic;

public class MovementSpeed : MultiplicativeBonusStat
{
    private const float LOW_CAP = 220;
    private const float FIRST_CAP = 415;
    private const float SECOND_CAP = 490;

    private float currentSlowResistance;

    private List<float> slows = new List<float>();//This needs to be initialized before the parent constructor if we don't want to add a null check in GetBiggestSlow.

    public MovementSpeed(float initialBaseValue) : base(initialBaseValue) { }

    public void SubscribeToSlowResistanceChangedEvent(SlowResistance slowResistance)
    {
        slowResistance.OnSlowResistanceChanged += OnSlowResistanceChanged;
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

        total *= 0.01f;
    }

    private void OnSlowResistanceChanged(float slowResistance)
    {
        currentSlowResistance = slowResistance;
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
}
