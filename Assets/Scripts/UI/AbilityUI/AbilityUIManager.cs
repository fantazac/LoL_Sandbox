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

    public void UpdateAbilityCooldown(int abilityId, float cooldown, float cooldownRemaining, string cooldownForUI)
    {
        abilityImages[abilityId].fillAmount = 1 - (cooldownRemaining / cooldown);
        abilityCooldownTexts[abilityId].text = cooldownForUI;
    }

    public void SetAbilityOffCooldown(int abilityId)
    {
        Image abilityImage = abilityImages[abilityId];
        abilityImage.fillAmount = 1;
        abilityImage.color = Color.white;
        abilityCooldownTexts[abilityId].text = "";
    }
}
