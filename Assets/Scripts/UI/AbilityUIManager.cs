using UnityEngine;
using UnityEngine.UI;

public class AbilityUIManager : MonoBehaviour
{
    [SerializeField]
    private Image[] abilityImages;
    [SerializeField]
    private Text[] abilityCostTexts;
    [SerializeField]
    private Image[] abilityOnCooldownImages;
    [SerializeField]
    private Image[] abilityOnCooldownForRecastImages;
    [SerializeField]
    private Text[] abilityCooldownTexts;
    [SerializeField]
    private GameObject[] abilityNotEnoughResourceObjects;
    [SerializeField]
    private GameObject[] abilityBlockedObjects;
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

    public void SetAbilitySprite(AbilityCategory abilityCategory, int abilityId, Sprite abilitySprite)
    {
        abilityImages[GetAbilityId(abilityCategory, abilityId)].sprite = abilitySprite;
    }

    private int GetAbilityId(AbilityCategory abilityCategory, int abilityId)
    {
        int id = abilityId;
        if (abilityCategory == AbilityCategory.CharacterAbility)
        {
            id += 1;
        }
        else if (abilityCategory == AbilityCategory.SummonerAbility)
        {
            id += 5;
        }
        else if (abilityCategory == AbilityCategory.OtherCharacterAbility)
        {
            id += 7;
        }
        return id;
    }

    public void SetAbilityCost(int abilityId, float resourceCost)
    {
        abilityCostTexts[abilityId].text = resourceCost == 0 ? "" : ((int)resourceCost).ToString();
    }

    public void SetMaxAbilityLevel(int abilityId, int abilityMaxLevel)
    {
        RectTransform parent = abilityLevelPoints[abilityId].GetComponent<RectTransform>();
        parent.anchoredPosition = parent.anchoredPosition + (Vector2.left * 4.5f * abilityMaxLevel + Vector2.right);
        for (int i = 0; i < abilityMaxLevel; i++)
        {
            Instantiate(abilityLevelPoint).transform.SetParent(parent, false);
        }
    }

    public void LevelUpAbility(int abilityId, int abilityLevel)
    {
        abilityLevelPoints[abilityId].transform.GetChild(abilityLevel - 1).GetComponent<Image>().color = abilityLevelPointColor;
    }

    public void SetAbilityOnCooldown(AbilityCategory abilityCategory, int abilityId, bool abilityIsBlocked)
    {
        int id = GetAbilityId(abilityCategory, abilityId);
        if (abilityIsBlocked)
        {
            abilityBlockedObjects[id - 1].SetActive(false);
        }
        abilityImages[id].color = abilityColorOnCooldown;
        abilityOnCooldownImages[id].fillAmount = 1;
    }

    public void SetAbilityOnCooldownForRecast(AbilityCategory abilityCategory, int abilityId)
    {
        int id = GetAbilityId(abilityCategory, abilityId);
        abilityImages[id].color = abilityColorOnCooldownForRecast;
        abilityOnCooldownImages[id].fillAmount = 1;
    }

    public void UpdateAbilityCooldown(AbilityCategory abilityCategory, int abilityId, float cooldown, float cooldownRemaining)
    {
        int id = GetAbilityId(abilityCategory, abilityId);

        abilityOnCooldownImages[id].fillAmount = cooldownRemaining / cooldown;

        SetCooldownText(id, cooldownRemaining);
    }

    public void UpdateAbilityCooldownForRecast(AbilityCategory abilityCategory, int abilityId, float cooldownForRecast, float cooldownRemaining)
    {
        int id = GetAbilityId(abilityCategory, abilityId);

        abilityOnCooldownImages[id].fillAmount = cooldownRemaining / cooldownForRecast;

        SetCooldownText(id, cooldownRemaining);
    }

    private void SetCooldownText(int abilityId, float cooldownRemaining)
    {
        string cooldownString = "";
        if (cooldownRemaining >= 0.7f)
        {
            cooldownString = Mathf.CeilToInt(cooldownRemaining).ToString();
        }
        else if (cooldownRemaining > 0)
        {
            cooldownString = cooldownRemaining.ToString("f1");
        }
        abilityCooldownTexts[abilityId].text = cooldownString;
    }

    public void SetAbilityOffCooldown(AbilityCategory abilityCategory, int abilityId, bool abilityUsesResource, bool abilityIsEnabled, bool abilityIsBlocked)
    {
        int id = GetAbilityId(abilityCategory, abilityId);
        abilityOnCooldownImages[id].fillAmount = 0;
        if (abilityIsBlocked)
        {
            BlockAbility(abilityCategory, abilityId, abilityUsesResource);
        }
        else if (abilityIsEnabled)
        {
            abilityImages[id].color = Color.white;
        }
        abilityCooldownTexts[id].text = "";
    }

    public void SetAbilityOffCooldownForRecast(AbilityCategory abilityCategory, int abilityId)
    {
        int id = GetAbilityId(abilityCategory, abilityId);
        abilityOnCooldownImages[id].fillAmount = 0;
        abilityImages[id].color = Color.white;
        abilityCooldownTexts[id].text = "";
    }

    public void DisableAbility(AbilityCategory abilityCategory, int abilityId, bool abilityUsesResource)
    {
        abilityImages[GetAbilityId(abilityCategory, abilityId)].color = abilityColorOnCooldown;
        if (abilityUsesResource)
        {
            UpdateAbilityHasEnoughResource(abilityId, true);
        }
    }

    public void EnableAbility(AbilityCategory abilityCategory, int abilityId, bool characterHasEnoughResourceToCastAbility)
    {
        abilityImages[GetAbilityId(abilityCategory, abilityId)].color = Color.white;
        UpdateAbilityHasEnoughResource(abilityId, characterHasEnoughResourceToCastAbility);
    }

    public void BlockAbility(AbilityCategory abilityCategory, int abilityId, bool abilityUsesResource)
    {
        if (abilityCategory != AbilityCategory.OfflineAbility && abilityCategory != AbilityCategory.OtherCharacterAbility)
        {
            int id = GetAbilityId(abilityCategory, abilityId);
            abilityImages[id].color = abilityColorOnCooldown;
            abilityBlockedObjects[id - 1].SetActive(true);
            if (abilityUsesResource)
            {
                UpdateAbilityHasEnoughResource(abilityId, true);
            }
        }
    }

    public void UnblockAbility(AbilityCategory abilityCategory, int abilityId, bool characterHasEnoughResourceToCastAbility)
    {
        if (abilityCategory != AbilityCategory.OfflineAbility && abilityCategory != AbilityCategory.OtherCharacterAbility)
        {
            int id = GetAbilityId(abilityCategory, abilityId);
            abilityImages[id].color = Color.white;
            abilityBlockedObjects[id - 1].SetActive(false);
            UpdateAbilityHasEnoughResource(abilityId, characterHasEnoughResourceToCastAbility);
        }
    }

    public void UpdateAbilityHasEnoughResource(int abilityId, bool characterHasEnoughResourceToCastAbility)
    {
        GameObject abilityNotEnoughResourceObject = abilityNotEnoughResourceObjects[abilityId];
        if ((characterHasEnoughResourceToCastAbility && abilityNotEnoughResourceObject.activeSelf) || (!characterHasEnoughResourceToCastAbility && !abilityNotEnoughResourceObject.activeSelf))
        {
            abilityNotEnoughResourceObject.SetActive(!characterHasEnoughResourceToCastAbility);
        }
    }
}
