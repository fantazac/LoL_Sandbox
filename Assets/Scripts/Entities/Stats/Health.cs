using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float baseHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float bonusHealth;//health gained from abilities, items, masteries, etc...
    [SerializeField]
    private float maxHealth;//baseHealth + bonusHealth

    public void SetBaseHealth(float baseHealth)
    {
        this.baseHealth = baseHealth;
        maxHealth = baseHealth;
        currentHealth = maxHealth;//change this
    }

    public float GetBaseHealth()
    {
        return baseHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetBonusHealth()
    {
        return bonusHealth;
    }

    public float GetMaximumHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }

    public void Heal(float heal)
    {
        if (currentHealth + heal >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += heal;
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public string GetUIText()
    {
        return "HEALTH: " + GetCurrentHealth() + " / " + GetMaximumHealth() + " (" + GetBaseHealth() + " + " + GetBonusHealth() + ")";
    }

    public void Hit(float damage)
    {
        if(currentHealth <= damage)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= damage;
        }
    }
}
