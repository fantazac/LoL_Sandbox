using System.Collections.Generic;

public class Shield
{
    private Unit unit;

    private float totalShield;

    private Dictionary<AbilityBuff, float> shieldSources;
    private List<AbilityBuff> shieldSourcesInOrderOfCreation;

    public Shield(Unit unit)
    {
        this.unit = unit;

        shieldSources = new Dictionary<AbilityBuff, float>();
        shieldSourcesInOrderOfCreation = new List<AbilityBuff>();
    }

    private void UpdateTotal()
    {
        totalShield = 0;
        foreach (KeyValuePair<AbilityBuff, float> shieldSource in shieldSources)
        {
            totalShield += shieldSource.Value;
        }
    }

    public float GetTotal()
    {
        return totalShield;
    }

    public void AddNewShield(AbilityBuff sourceAbilityBuff, float shieldValue)
    {
        if (shieldSources.ContainsKey(sourceAbilityBuff))
        {
            RemoveShieldFromContainers(sourceAbilityBuff);
        }
        shieldSourcesInOrderOfCreation.Add(sourceAbilityBuff);
        shieldSources.Add(sourceAbilityBuff, shieldValue);
        UpdateTotal();
    }

    public void RemoveShield(AbilityBuff sourceAbilityBuff)
    {
        RemoveShieldFromContainers(sourceAbilityBuff);
        UpdateTotal();
    }

    private void RemoveShieldFromContainers(AbilityBuff sourceAbilityBuff)
    {
        shieldSourcesInOrderOfCreation.Remove(sourceAbilityBuff);
        shieldSources.Remove(sourceAbilityBuff);
    }

    public void UpdateShield(AbilityBuff sourceAbilityBuff, float shieldChangeValue)
    {
        if (shieldSources.ContainsKey(sourceAbilityBuff))
        {
            shieldSources[sourceAbilityBuff] += shieldChangeValue;
            if (shieldSources[sourceAbilityBuff] <= 0)
            {
                sourceAbilityBuff.ConsumeBuff(unit);
            }
            else
            {
                UpdateTotal();
            }
        }
    }

    public float DamageShield(float damage)
    {
        float remainingDamage = damage;

        for (int i = 0; i < shieldSourcesInOrderOfCreation.Count; i++)
        {
            AbilityBuff abilityBuff = shieldSourcesInOrderOfCreation[i];
            float shieldValue = shieldSources[abilityBuff] - remainingDamage;
            if (shieldValue > 0)
            {
                shieldSources[abilityBuff] = shieldValue;
                remainingDamage = 0;
                break;
            }
            else
            {
                i--;
                abilityBuff.ConsumeBuff(unit);
                remainingDamage = -shieldValue;
            }
        }
        UpdateTotal();

        return remainingDamage;
    }
}
