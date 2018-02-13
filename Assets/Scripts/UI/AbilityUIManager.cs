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
    private Text[] abilityCooldownTexts;

    private Color abilityColorOnCooldown;

    private AbilityUIManager()
    {
        abilityColorOnCooldown = Color.gray;
    }

    public void SetAbilitySprite(int abilityId, Sprite abilitySprite)
    {
        abilityImages[abilityId].sprite = abilitySprite;
    }

    public void SetAbilityOnCooldown(int abilityId)
    {
        abilityImages[abilityId].color = abilityColorOnCooldown;
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

    public void SetAbilityOffCooldown(int abilityId)
    {
        abilityOnCooldownImages[abilityId].fillAmount = 0;
        abilityImages[abilityId].color = Color.white;
        abilityCooldownTexts[abilityId].text = "";
    }
}
