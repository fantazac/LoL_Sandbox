using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    private Unit unit;

    private Shield shield;
    private Shield physicalShield;
    private Shield magicShield;

    public delegate void OnShieldChangedHandler(ShieldType shieldType, float shieldValue);

    public event OnShieldChangedHandler OnShieldChanged;

    private void Start()
    {
        unit = GetComponent<Unit>();

        shield = new Shield(unit);
        physicalShield = new Shield(unit);
        magicShield = new Shield(unit);
    }

    public void AddNewShield(ShieldType shieldType, AbilityBuff sourceAbilityBuff, float shieldValue)
    {
        Shield shieldToChange = GetShield(shieldType);
        shieldToChange.AddNewShield(sourceAbilityBuff, shieldValue);
        OnShieldChanged?.Invoke(shieldType, shieldToChange.GetTotal());
    }

    public void RemoveShield(ShieldType shieldType, AbilityBuff sourceAbilityBuff)
    {
        Shield shieldToChange = GetShield(shieldType);
        shieldToChange.RemoveShield(sourceAbilityBuff);
        OnShieldChanged?.Invoke(shieldType, shieldToChange.GetTotal());
    }

    public void UpdateShield(ShieldType shieldType, AbilityBuff sourceAbilityBuff, float shieldChangeValue)
    {
        Shield shieldToChange = GetShield(shieldType);
        shieldToChange.UpdateShield(sourceAbilityBuff, shieldChangeValue);
        OnShieldChanged?.Invoke(shieldType, shieldToChange.GetTotal());
    }

    private Shield GetShield(ShieldType shieldType)
    {
        switch (shieldType)
        {
            case ShieldType.NORMAL:
                return shield;
            case ShieldType.MAGIC:
                return magicShield;
            default:
                return physicalShield;
        }
    }

    public float DamageShield(ShieldType shieldType, float damage)
    {
        float remainingDamage = damage;
        switch (shieldType)
        {
            case ShieldType.MAGIC when magicShield.GetTotal() > 0:
            {
                remainingDamage = magicShield.DamageShield(remainingDamage);
                OnShieldChanged?.Invoke(shieldType, magicShield.GetTotal());

                break;
            }
            case ShieldType.PHYSICAL when physicalShield.GetTotal() > 0:
            {
                remainingDamage = physicalShield.DamageShield(remainingDamage);
                OnShieldChanged?.Invoke(shieldType, physicalShield.GetTotal());

                break;
            }
        }

        if (remainingDamage <= 0 || shield.GetTotal() <= 0) return remainingDamage;

        remainingDamage = shield.DamageShield(remainingDamage);
        OnShieldChanged?.Invoke(ShieldType.NORMAL, shield.GetTotal());

        return remainingDamage;
    }
}
