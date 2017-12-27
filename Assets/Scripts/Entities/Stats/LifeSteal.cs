using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSteal : MonoBehaviour
{
    [SerializeField]
    private float baseLifeSteal;
    [SerializeField]
    private float currentLifeSteal;
    [SerializeField]
    private float bonusLifeSteal;

    public void SetBaseLifeSteal(float baseLifeSteal)
    {
        this.baseLifeSteal = baseLifeSteal;
        currentLifeSteal = baseLifeSteal;//change this
    }

    public float GetBaseLifeSteal()
    {
        return baseLifeSteal;
    }

    public float GetCurrentLifeSteal()
    {
        return currentLifeSteal;
    }

    public float GetBonusLifeSteal()
    {
        return bonusLifeSteal;
    }
}
