using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    [SerializeField]
    private float baseAttackDamage;
    [SerializeField]
    private float currentAttackDamage;
    [SerializeField]
    private float bonusAttackDamage;

    public void SetBaseAttackDamage(float baseAttackDamage)
    {
        this.baseAttackDamage = baseAttackDamage;
        currentAttackDamage = baseAttackDamage;//change this
    }

    public float GetBaseAttackDamage()
    {
        return baseAttackDamage;
    }

    public float GetCurrentAttackDamage()
    {
        return currentAttackDamage;
    }

    public float GetBonusAttackDamage()
    {
        return bonusAttackDamage;
    }
}
