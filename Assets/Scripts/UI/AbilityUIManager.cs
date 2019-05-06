using UnityEngine;
using UnityEngine.UI;

public class AbilityUIManager : MonoBehaviour
{
    [SerializeField] private Image[] abilityImages;
    [SerializeField] private Image[] abilityRecastImages;
    [SerializeField] private Text[] abilityCostTexts;
    [SerializeField] private Image[] abilityOnCooldownImages;
    [SerializeField] private Text[] abilityCooldownTexts;
    [SerializeField] private GameObject[] abilityNotEnoughResourceObjects;
    [SerializeField] private GameObject[] abilityBlockedObjects;
    [SerializeField] private GameObject[] abilityLevelPoints;
    [SerializeField] private GameObject abilityLevelPoint;

    private readonly Color abilityColorDisabled;
    private readonly Color abilityColorEnabled;
    private readonly Color abilityLevelPointColor;

    private AbilityUIManager()
    {
        abilityColorDisabled = Color.gray;
        abilityColorEnabled = Color.white;
        abilityLevelPointColor = new Color(0.85f, 0.8f, 0.3f);
    }

    public void SetAbilitySprite(AbilityCategory abilityCategory, int abilityId, Sprite abilitySprite)
    {
        abilityImages[GetAbilityId(abilityCategory, abilityId)].sprite = abilitySprite;
    }

    public void SetAbilityRecastSprite(AbilityCategory abilityCategory, int abilityId, Sprite abilityRecastSprite)
    {
        abilityRecastImages[GetAbilityId(abilityCategory, abilityId)].sprite = abilityRecastSprite;
    }

    private int GetAbilityId(AbilityCategory abilityCategory, int abilityId)
    {
        int id = abilityId;
        switch (abilityCategory)
        {
            case AbilityCategory.CharacterAbility:
                id += 1;
                break;
            case AbilityCategory.SummonerAbility:
                id += 5;
                break;
            case AbilityCategory.OtherCharacterAbility:
                id += 7;
                break;
        }

        return id;
    }

    public void SetAbilityCost(int abilityId, float resourceCost)
    {
        abilityCostTexts[abilityId].text = resourceCost <= 0 ? "" : ((int)resourceCost).ToString();
    }

    public void SetMaxAbilityLevel(int abilityId, int abilityMaxLevel)
    {
        RectTransform parent = abilityLevelPoints[abilityId].GetComponent<RectTransform>();
        parent.anchoredPosition += 4.5f * abilityMaxLevel * Vector2.left + Vector2.right;
        for (int i = 0; i < abilityMaxLevel; i++)
        {
            Instantiate(abilityLevelPoint).transform.SetParent(parent, false);
        }
    }

    public void LevelUpAbility(int abilityId, int abilityLevel)
    {
        abilityLevelPoints[abilityId].transform.GetChild(abilityLevel - 1).GetComponent<Image>().color = abilityLevelPointColor;
    }

    public void SetAbilityOnCooldown(AbilityCategory abilityCategory, int abilityId, bool abilityIsBlocked, bool canBeRecasted, bool abilityUsesResource)
    {
        int id = GetAbilityId(abilityCategory, abilityId);
        if (canBeRecasted)
        {
            abilityRecastImages[id].gameObject.SetActive(false);
        }

        if (abilityUsesResource)
        {
            UpdateAbilityHasEnoughResource(abilityCategory, abilityId, true, false);
        }

        if (abilityIsBlocked)
        {
            abilityBlockedObjects[id - 1].SetActive(false);
        }

        abilityImages[id].color = abilityColorDisabled;
        abilityOnCooldownImages[id].fillAmount = 1;
    }

    public void SetAbilityOnCooldownForRecast(AbilityCategory abilityCategory, int abilityId)
    {
        int id = GetAbilityId(abilityCategory, abilityId);
        abilityRecastImages[id].gameObject.SetActive(true);
        abilityRecastImages[id].color = abilityColorDisabled;
        abilityOnCooldownImages[id].fillAmount = 1;
    }

    public void UpdateAbilityCooldown(AbilityCategory abilityCategory, int abilityId, float cooldown, float cooldownRemaining)
    {
        int id = GetAbilityId(abilityCategory, abilityId);

        abilityOnCooldownImages[id].fillAmount = cooldownRemaining / cooldown;

        SetCooldownText(id, cooldownRemaining);
    }

    public void UpdateAbilityCooldownForRecast(AbilityCategory abilityCategory, int abilityId, float cooldown, float cooldownRemaining)
    {
        UpdateAbilityCooldown(abilityCategory, abilityId, cooldown, cooldownRemaining);
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

    public void SetAbilityOffCooldown(AbilityCategory abilityCategory, int abilityId, bool abilityUsesResource, bool abilityIsEnabled, bool abilityIsBlocked,
        bool championHasEnoughResourceToCastAbility)
    {
        int id = GetAbilityId(abilityCategory, abilityId);
        abilityOnCooldownImages[id].fillAmount = 0;
        abilityCooldownTexts[id].text = "";

        if (abilityUsesResource)
        {
            if (abilityIsEnabled)
            {
                abilityImages[id].color = abilityColorEnabled;
            }
            UpdateAbilityHasEnoughResource(abilityCategory, abilityId, championHasEnoughResourceToCastAbility, abilityIsBlocked);
        }
        else if (abilityIsBlocked)
        {
            BlockAbility(abilityCategory, abilityId);
        }
        else if (abilityIsEnabled)
        {
            abilityImages[id].color = abilityColorEnabled;
        }
    }

    public void SetAbilityOffCooldownForRecast(AbilityCategory abilityCategory, int abilityId)
    {
        int id = GetAbilityId(abilityCategory, abilityId);
        abilityOnCooldownImages[id].fillAmount = 0;
        abilityRecastImages[id].color = abilityColorEnabled;
        abilityCooldownTexts[id].text = "";
    }

    public void DisableAbility(AbilityCategory abilityCategory, int abilityId, bool abilityUsesResource)
    {
        abilityImages[GetAbilityId(abilityCategory, abilityId)].color = abilityColorDisabled;
        if (abilityUsesResource)
        {
            UpdateAbilityHasEnoughResource(abilityCategory, abilityId, true, false);
        }
    }

    public void EnableAbility(AbilityCategory abilityCategory, int abilityId, bool abilityUsesResource, bool characterHasEnoughResourceToCastAbility, bool abilityIsBlocked)
    {
        abilityImages[GetAbilityId(abilityCategory, abilityId)].color = abilityColorEnabled;
        if (abilityUsesResource)
        {
            UpdateAbilityHasEnoughResource(abilityCategory, abilityId, characterHasEnoughResourceToCastAbility, abilityIsBlocked);   
        }
        else if (abilityIsBlocked)
        {
            BlockAbility(abilityCategory, abilityId);
        }
    }

    public void BlockAbility(AbilityCategory abilityCategory, int abilityId)
    {
        if (abilityCategory == AbilityCategory.OfflineAbility || abilityCategory == AbilityCategory.OtherCharacterAbility) return;

        int id = GetAbilityId(abilityCategory, abilityId);
        abilityImages[id].color = abilityColorDisabled;
        abilityBlockedObjects[id - 1].SetActive(true);
    }

    public void UnblockAbility(AbilityCategory abilityCategory, int abilityId)
    {
        if (abilityCategory == AbilityCategory.OfflineAbility || abilityCategory == AbilityCategory.OtherCharacterAbility) return;

        int id = GetAbilityId(abilityCategory, abilityId);
        abilityImages[id].color = abilityColorEnabled;
        abilityBlockedObjects[id - 1].SetActive(false);
    }

    public void UpdateAbilityHasEnoughResource(AbilityCategory abilityCategory, int abilityId, bool championHasEnoughResourceToCastAbility, bool abilityIsBlocked)
    {
        int id = GetAbilityId(abilityCategory, abilityId);
        GameObject abilityNotEnoughResourceObject = abilityNotEnoughResourceObjects[id - 1];
        if (championHasEnoughResourceToCastAbility == abilityNotEnoughResourceObject.activeSelf)
        {
            abilityNotEnoughResourceObject.SetActive(!championHasEnoughResourceToCastAbility);
        }

        if (!abilityIsBlocked) return;
        
        if (championHasEnoughResourceToCastAbility)
        {
            BlockAbility(abilityCategory, abilityId);
        }
        else
        {
            UnblockAbility(abilityCategory, abilityId);
        }
    }
}
