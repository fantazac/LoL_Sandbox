using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIManager : MonoBehaviour
{
    [SerializeField]
    private Image[] abilityImages;
    [SerializeField]
    private Image[] abilityOnCooldownImages;
    [SerializeField]
    private Image[] abilityOnCooldownForRecastImages;
    [SerializeField]
    private Text[] abilityCooldownTexts;
    [SerializeField]
    private GameObject[] abilityLevelPoints;
    [SerializeField]
    private GameObject abilityLevelPoint;

    private Color abilityColorOnCooldown;
    private Color abilityColorOnCooldownForRecast;
    private Color abilityLevelPointColor;

    private AbilityUIManager()
    {
        abilityColorOnCooldown = Color.gray;
        abilityColorOnCooldownForRecast = new Color(0.7f, 0.7f, 0.7f);
        abilityLevelPointColor = new Color(0.85f, 0.8f, 0.3f);
    }

    public void SetAbilitySprite(int abilityId, Sprite abilitySprite)
    {
        abilityImages[abilityId].sprite = abilitySprite;
    }

    public void SetMaxAbilityLevel(int abilityId, int abilityMaxLevel)
    {
        RectTransform parent = abilityLevelPoints[abilityId - 1].GetComponent<RectTransform>();
        parent.anchoredPosition = parent.anchoredPosition + (Vector2.left * 4.5f * abilityMaxLevel + Vector2.right);
        for (int i = 0; i < abilityMaxLevel; i++)
        {
            Instantiate(abilityLevelPoint).transform.SetParent(parent, false);
        }
    }

    public void LevelUpAbility(int abilityId, int abilityLevel)
    {
        abilityLevelPoints[abilityId - 1].transform.GetChild(abilityLevel - 1).GetComponent<Image>().color = abilityLevelPointColor;
    }

    public void SetAbilityOnCooldown(int abilityId)
    {
        abilityImages[abilityId].color = abilityColorOnCooldown;
        abilityOnCooldownImages[abilityId].fillAmount = 1;
    }

    public void SetAbilityOnCooldownForRecast(int abilityId)
    {
        abilityImages[abilityId].color = abilityColorOnCooldownForRecast;
        abilityOnCooldownImages[abilityId].fillAmount = 1;
    }

    public void UpdateAbilityCooldown(int abilityId, float cooldown, float cooldownRemaining)
    {
        abilityOnCooldownImages[abilityId].fillAmount = cooldownRemaining / cooldown;

        if (cooldownRemaining >= 1)
        {
            abilityCooldownTexts[abilityId].text = ((int)cooldownRemaining).ToString();
        }
        else if (cooldownRemaining <= 0)
        {
            abilityCooldownTexts[abilityId].text = "";
        }
        else
        {
            abilityCooldownTexts[abilityId].text = cooldownRemaining.ToString("f1");
        }
    }

    public void UpdateAbilityCooldownForRecast(int abilityId, float cooldownForRecast, float cooldownRemaining)
    {
        abilityOnCooldownImages[abilityId].fillAmount = cooldownRemaining / cooldownForRecast;

        if (cooldownRemaining >= 1)
        {
            abilityCooldownTexts[abilityId].text = ((int)cooldownRemaining).ToString();
        }
        else if (cooldownRemaining <= 0)
        {
            abilityCooldownTexts[abilityId].text = "";
        }
        else
        {
            abilityCooldownTexts[abilityId].text = cooldownRemaining.ToString("f1");
        }
    }

    public void SetAbilityOffCooldown(int abilityId, bool abilityIsEnabled)
    {
        abilityOnCooldownImages[abilityId].fillAmount = 0;
        if (abilityIsEnabled)
        {
            abilityImages[abilityId].color = Color.white;
        }
        abilityCooldownTexts[abilityId].text = "";
    }

    public void SetAbilityOffCooldownForRecast(int abilityId)
    {
        abilityOnCooldownImages[abilityId].fillAmount = 0;
        abilityImages[abilityId].color = Color.white;
        abilityCooldownTexts[abilityId].text = "";
    }

    public void DisableAbility(int abilityId)
    {
        abilityImages[abilityId].color = abilityColorOnCooldown;
    }

    public void EnableAbility(int abilityId)
    {
        abilityImages[abilityId].color = Color.white;
    }
}
