﻿using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;
    [SerializeField]
    private Image resourceImage;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text resourceText;

    private Unit unit;

    private int maxHealth;
    private int maxResource;

    private string healthSelfPath;
    private string healthEnemyPath;

    private HealthUIManager()
    {
        healthSelfPath = "Sprites/UI/health_self";
        healthEnemyPath = "Sprites/UI/health_enemy";
    }

    public void ResetHealthAndResource()
    {
        unit.StatsManager.Health.OnCurrentResourceChanged -= OnCurrentHealthChanged;
        if (unit.StatsManager.ResourceType != ResourceType.NONE)
        {
            unit.StatsManager.Resource.OnCurrentResourceChanged -= OnCurrentResourceChanged;
        }
        unit = null;
    }

    public void SetHealthAndResource(Unit unit)
    {
        this.unit = unit;

        unit.StatsManager.Health.OnCurrentResourceChanged += OnCurrentHealthChanged;

        maxHealth = Mathf.CeilToInt(unit.StatsManager.Health.GetTotal());

        if (StaticObjects.Champion.Team == unit.Team)
        {
            healthImage.sprite = Resources.Load<Sprite>(healthSelfPath);
        }
        else
        {
            healthImage.sprite = Resources.Load<Sprite>(healthEnemyPath);
        }

        OnCurrentHealthChanged(unit.StatsManager.Health.GetCurrentValue());

        if (unit.StatsManager.ResourceType != ResourceType.NONE)
        {
            resourceImage.gameObject.SetActive(true);

            unit.StatsManager.Resource.OnCurrentResourceChanged += OnCurrentResourceChanged;

            maxResource = (int)unit.StatsManager.Resource.GetTotal();

            if (unit.StatsManager.ResourceType == ResourceType.MANA)
            {
                resourceImage.color = new Color(57f / 255f, 170f / 255f, 222f / 255f);
            }
            else if (unit.StatsManager.ResourceType == ResourceType.ENERGY)
            {
                resourceImage.color = new Color(234f / 255f, 221f / 255f, 90f / 255f);
            }
            else if (unit.StatsManager.ResourceType == ResourceType.FURY)
            {
                resourceImage.color = new Color(244f / 255f, 4f / 255f, 13f / 255f);
            }

            OnCurrentResourceChanged(unit.StatsManager.Resource.GetCurrentValue());
        }
        else
        {
            resourceImage.gameObject.SetActive(false);
        }
    }

    private void OnCurrentHealthChanged(float currentValue)
    {
        maxHealth = Mathf.CeilToInt(unit.StatsManager.Health.GetTotal());
        healthImage.fillAmount = currentValue / maxHealth;
        healthText.text = Mathf.CeilToInt(currentValue) + " / " + maxHealth;
    }

    private void OnCurrentResourceChanged(float currentValue)
    {
        maxResource = (int)unit.StatsManager.Resource.GetTotal();
        resourceImage.fillAmount = currentValue / maxResource;
        resourceText.text = (int)currentValue + " / " + maxResource;
    }
}