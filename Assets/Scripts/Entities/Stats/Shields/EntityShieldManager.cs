using UnityEngine;

public class EntityShieldManager : MonoBehaviour
{
    private Entity entity;

    private Shield shield;
    private Shield physicalShield;
    private Shield magicShield;

    public delegate void OnShieldChangedHandler(ShieldType shieldType, float shieldValue);
    public event OnShieldChangedHandler OnShieldChanged;

    private void Start()
    {
        entity = GetComponent<Entity>();

        shield = new Shield(entity);
        physicalShield = new Shield(entity);
        magicShield = new Shield(entity);
    }

    public void AddNewShield(ShieldType shieldType, AbilityBuff sourceAbilityBuff, float shieldValue)
    {
        Shield shieldToChange = GetShield(shieldType);
        shieldToChange.AddNewShield(sourceAbilityBuff, shieldValue);
        if (OnShieldChanged != null)
        {
            OnShieldChanged(shieldType, shieldToChange.GetTotal());
        }
    }

    public void RemoveShield(ShieldType shieldType, AbilityBuff sourceAbilityBuff)
    {
        Shield shieldToChange = GetShield(shieldType);
        shieldToChange.RemoveShield(sourceAbilityBuff);
        if (OnShieldChanged != null)
        {
            OnShieldChanged(shieldType, shieldToChange.GetTotal());
        }
    }

    public void UpdateShield(ShieldType shieldType, AbilityBuff sourceAbilityBuff, float shieldChangeValue)
    {
        Shield shieldToChange = GetShield(shieldType);
        shieldToChange.UpdateShield(sourceAbilityBuff, shieldChangeValue);
        if (OnShieldChanged != null)
        {
            OnShieldChanged(shieldType, shieldToChange.GetTotal());
        }
    }

    private Shield GetShield(ShieldType shieldType)
    {
        if (shieldType == ShieldType.NORMAL)
        {
            return shield;
        }
        else if (shieldType == ShieldType.MAGIC)
        {
            return magicShield;
        }
        else
        {
            return physicalShield;
        }
    }

    public float DamageShield(ShieldType shieldType, float damage)
    {
        float remainingDamage = damage;

        if (shieldType == ShieldType.MAGIC)
        {
            remainingDamage = magicShield.DamageShield(remainingDamage);
            if (OnShieldChanged != null)
            {
                OnShieldChanged(shieldType, magicShield.GetTotal());
            }
        }
        else if (shieldType == ShieldType.PHYSICAL)
        {
            remainingDamage = physicalShield.DamageShield(remainingDamage);
            if (OnShieldChanged != null)
            {
                OnShieldChanged(shieldType, physicalShield.GetTotal());
            }
        }

        if (remainingDamage > 0)
        {
            remainingDamage = shield.DamageShield(remainingDamage);
            if (OnShieldChanged != null)
            {
                OnShieldChanged(shieldType, shield.GetTotal());
            }
        }

        return remainingDamage;
    }
}
