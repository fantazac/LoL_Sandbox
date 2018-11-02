using System.Collections.Generic;

public class Shield
{
    private Entity entity;

    private float totalShield;

    private Dictionary<AbilityBuff, float> shieldSources;
    private List<AbilityBuff> shieldSourcesInOrderOfCreation;

    public Shield(Entity entity)
    {
        this.entity = entity;

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
            RemoveShield(sourceAbilityBuff);
        }
        shieldSourcesInOrderOfCreation.Add(sourceAbilityBuff);
        shieldSources.Add(sourceAbilityBuff, shieldValue);
        UpdateTotal();
    }

    public void RemoveShield(AbilityBuff sourceAbilityBuff)
    {
        shieldSourcesInOrderOfCreation.Remove(sourceAbilityBuff);
        shieldSources.Remove(sourceAbilityBuff);
        UpdateTotal();
    }

    public void UpdateShield(AbilityBuff sourceAbilityBuff, float shieldChangeValue)
    {
        if (shieldSources.ContainsKey(sourceAbilityBuff))
        {
            shieldSources[sourceAbilityBuff] += shieldChangeValue;
            if (shieldSources[sourceAbilityBuff] <= 0)
            {
                sourceAbilityBuff.ConsumeBuff(entity);
            }
            UpdateTotal();
        }
    }

    public float DamageShield(float damage)
    {
        float remainingDamage = damage;

        foreach (KeyValuePair<AbilityBuff, float> shieldSource in shieldSources)
        {
            float shieldValue = shieldSource.Value - remainingDamage;
            if (shieldValue > 0)
            {
                shieldSources[shieldSource.Key] = shieldValue;
                remainingDamage = 0;
                break;
            }
            else
            {
                shieldSource.Key.ConsumeBuff(entity);
                remainingDamage = -shieldValue;
            }
        }
        UpdateTotal();

        return remainingDamage;
    }
}
