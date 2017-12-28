using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField]
    private float baseAttackRange;
    [SerializeField]
    private float currentAttackRange;
    [SerializeField]
    private float bonusAttackRange;

    public void SetBaseAttackRange(float baseAttackRange)
    {
        this.baseAttackRange = baseAttackRange;
        currentAttackRange = baseAttackRange;//change this
    }

    public float GetBaseAttackRange()
    {
        return baseAttackRange;
    }

    public float GetCurrentAttackRange()
    {
        return currentAttackRange;
    }

    public float GetBonusAttackRange()
    {
        return bonusAttackRange;
    }

    public string GetUIText()
    {
        return "ATTACK RANGE: " + GetCurrentAttackRange() + " (" + GetBaseAttackRange() + " + " + GetBonusAttackRange() + ")";
    }
}
