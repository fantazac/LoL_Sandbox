using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeed : MonoBehaviour
{
    [SerializeField]
    private float baseAttackSpeed;
    [SerializeField]
    private float currentAttackSpeed;
    [SerializeField]
    private float bonusAttackSpeedPercent;

    public void SetBaseAttackSpeed(float baseAttackSpeed)
    {
        this.baseAttackSpeed = baseAttackSpeed;
        currentAttackSpeed = baseAttackSpeed;//change this
    }

    public float GetBaseAttackSpeed()
    {
        return baseAttackSpeed;
    }

    public float GetCurrentAttackSpeed()
    {
        return currentAttackSpeed;
    }

    public float GetBonusAttackSpeedPercent()
    {
        return bonusAttackSpeedPercent;
    }

    public float GetBonusAttackSpeedFlat()
    {
        return bonusAttackSpeedPercent * baseAttackSpeed;
    }

    public string GetUIText()
    {
        return "ATTACK SPEED: " + GetCurrentAttackSpeed().ToString("0.00") + " (" + GetBaseAttackSpeed() + " + " + GetBonusAttackSpeedFlat() + " (" + GetBonusAttackSpeedPercent() + "%))";
    }
}
