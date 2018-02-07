using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIManager : MonoBehaviour
{
    [SerializeField]
    private Image[] abilityImages;
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
        Image abilityImage = abilityImages[abilityId];
        abilityImage.color = abilityColorOnCooldown;
        abilityImage.fillAmount = 0;
    }

    public void UpdateAbilityCooldown(int abilityId, float cooldown, float cooldownRemaining)
    {
        abilityImages[abilityId].fillAmount = 1 - (cooldownRemaining / cooldown);

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
        Image abilityImage = abilityImages[abilityId];
        abilityImage.fillAmount = 1;
        abilityImage.color = Color.white;
        abilityCooldownTexts[abilityId].text = "";
    }
}
