using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellVamp : MonoBehaviour
{
    [SerializeField]
    private float baseSpellVamp;
    [SerializeField]
    private float currentSpellVamp;
    [SerializeField]
    private float bonusSpellVamp;

    public void SetBaseSpellVamp(float baseSpellVamp)
    {
        this.baseSpellVamp = baseSpellVamp;
        currentSpellVamp = baseSpellVamp;//change this
    }

    public float GetBaseSpellVamp()
    {
        return baseSpellVamp;
    }

    public float GetCurrentSpellVamp()
    {
        return currentSpellVamp;
    }

    public float GetBonusSpellVamp()
    {
        return bonusSpellVamp;
    }
}
