using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;
    private RectTransform healthImageTransform;
    [SerializeField]
    private Image shieldImage;
    private RectTransform shieldImageTransform;
    [SerializeField]
    private Image physicalShieldImage;
    private RectTransform physicalShieldImageTransform;
    [SerializeField]
    private Image magicShieldImage;
    private RectTransform magicShieldImageTransform;
    [SerializeField]
    private GameObject healthSeperators;
    [SerializeField]
    private Image resourceImage;
    [SerializeField]
    private Image healthBarImage;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Image statusImage;
    [SerializeField]
    private Text statusText;

    private Character character;

    private Vector2 healthBarOffset;
    private Vector2 characterYOffset;

    private float xRatioOffset;

    private Camera characterCamera;
    private Rect canvas;

    private float maxHealth;
    private float maxResource;
    private float[] shields;

    [SerializeField]
    private GameObject mark100Prefab;
    [SerializeField]
    private GameObject mark1000Prefab;

    private float healthWidth;
    private Vector2 healthBarXOrigin;

    private string healthSelfPath;
    private string healthAllyPath;
    private string healthEnemyPath;
    private string healthBarAllyPath;
    private string healthBarEnemyPath;
    private string shieldPath;
    private string physicalShieldPath;
    private string magicShieldPath;

    private HealthBar()
    {
        healthWidth = 105;
        healthBarXOrigin = Vector2.right * -0.5f * healthWidth;

        healthSelfPath = "Sprites/UI/health_self";
        healthAllyPath = "Sprites/UI/health_ally";
        healthEnemyPath = "Sprites/UI/health_enemy";
        healthBarAllyPath = "Sprites/UI/healthbar_ally";
        healthBarEnemyPath = "Sprites/UI/healthbar_enemy";
        shieldPath = "Sprites/UI/healthbar_shield";
        physicalShieldPath = "Sprites/UI/healthbar_physicalShield";
        magicShieldPath = "Sprites/UI/healthbar_magicShield";

        shields = new float[] { 0, 0, 0 };
    }

    private void Start()
    {
        decimal aspectRatio = decimal.Round((decimal)Screen.width / Screen.height, 2, System.MidpointRounding.AwayFromZero);
        xRatioOffset = (float)(aspectRatio / 1.77m);

        characterCamera = StaticObjects.CharacterCamera;
        healthBarOffset = Vector2.right * Screen.width * -0.5f + Vector2.up * Screen.height * -0.5f;
        characterYOffset = Vector2.up * 120;
    }

    public void SetupHealthBar(Character character)
    {
        this.character = character;

        character.EntityStatsManager.Health.OnCurrentResourceChanged += OnCurrentHealthChanged;

        character.EntityShieldManager.OnShieldChanged += OnShieldChanged;

        maxHealth = character.EntityStatsManager.Health.GetTotal();

        if (character.EntityStatsManager.ResourceType != ResourceType.NONE)
        {
            if (character.EntityStatsManager.ResourceType == ResourceType.MANA)
            {
                resourceImage.color = new Color(57f / 255f, 170f / 255f, 222f / 255f);
            }
            else if (character.EntityStatsManager.ResourceType == ResourceType.ENERGY)
            {
                resourceImage.color = new Color(234f / 255f, 221f / 255f, 90f / 255f);
            }
            else if (character.EntityStatsManager.ResourceType == ResourceType.FURY)
            {
                resourceImage.color = new Color(244f / 255f, 4f / 255f, 13f / 255f);
            }
            else
            {
                Destroy(resourceImage.gameObject);
            }

            character.EntityStatsManager.Resource.OnCurrentResourceChanged += OnCurrentResourceChanged;
            maxResource = character.EntityStatsManager.Resource.GetTotal();
        }
        else
        {
            Destroy(resourceImage.gameObject);
        }

        if (character.CharacterLevelManager)
        {
            character.CharacterLevelManager.OnLevelUp += OnLevelUp;
            OnLevelUp(character.CharacterLevelManager.Level == 0 ? 1 : character.CharacterLevelManager.Level);
        }
        else
        {
            OnLevelUp(1);
        }

        if (StaticObjects.Character.Team == character.Team)
        {
            if (StaticObjects.Character == character)
            {
                healthImage.sprite = Resources.Load<Sprite>(healthSelfPath);
            }
            else
            {
                healthImage.sprite = Resources.Load<Sprite>(healthAllyPath);
            }
            healthBarImage.sprite = Resources.Load<Sprite>(healthBarAllyPath);
        }
        else
        {
            healthBarImage.sprite = Resources.Load<Sprite>(healthBarEnemyPath);
            healthImage.sprite = Resources.Load<Sprite>(healthEnemyPath);
        }
        shieldImage.sprite = Resources.Load<Sprite>(shieldPath);
        physicalShieldImage.sprite = Resources.Load<Sprite>(physicalShieldPath);
        magicShieldImage.sprite = Resources.Load<Sprite>(magicShieldPath);

        healthImageTransform = healthImage.GetComponent<RectTransform>();
        shieldImageTransform = shieldImage.GetComponent<RectTransform>();
        physicalShieldImageTransform = physicalShieldImage.GetComponent<RectTransform>();
        magicShieldImageTransform = magicShieldImage.GetComponent<RectTransform>();

        SetHealthBarSeparators();
    }

    private void SetHealthBarSeparators()
    {
        Transform[] childrenTransforms = healthSeperators.transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childrenTransforms.Length; i++)
        {
            Destroy(childrenTransforms[i].gameObject);
        }
        float totalHealthValue = GetTotalHealthValue();
        float healthSeparatorFactor = GetHealthSeperatorFactor(totalHealthValue);
        float percentPer100Health = healthSeparatorFactor / totalHealthValue;
        Vector2 widthPer100Health = Vector2.right * (float)(decimal.Round((decimal)(percentPer100Health * healthWidth), 2, System.MidpointRounding.AwayFromZero));
        float modulo = 1000f / healthSeparatorFactor;
        float loopValue = totalHealthValue / healthSeparatorFactor;
        for (int i = 1; i < loopValue; i++)
        {
            GameObject separator;
            if (i % modulo == 0)
            {
                separator = Instantiate(mark1000Prefab);
                separator.transform.SetParent(healthSeperators.transform, false);
                separator.GetComponent<RectTransform>().anchoredPosition = healthBarXOrigin + (widthPer100Health * i);
            }
            else
            {
                separator = Instantiate(mark100Prefab);
                separator.transform.SetParent(healthSeperators.transform, false);
                separator.GetComponent<RectTransform>().anchoredPosition = healthBarXOrigin + (widthPer100Health * i) + Vector2.up * 3;
            }

        }
    }

    private void OnDestroy()
    {
        character.EntityStatsManager.Health.OnCurrentResourceChanged -= OnCurrentHealthChanged;
        if (character.EntityStatsManager.Resource != null)
        {
            character.EntityStatsManager.Resource.OnCurrentResourceChanged -= OnCurrentResourceChanged;
        }
        character.EntityShieldManager.OnShieldChanged -= OnShieldChanged;
    }

    private float GetHealthSeperatorFactor(float totalHealthValue)
    {
        if (totalHealthValue < 3000)
        {
            return 100f;
        }
        else if (totalHealthValue < 6000)
        {
            return 200f;
        }
        else if (totalHealthValue < 10000)
        {
            return 250f;
        }
        return 500f;
    }

    private void OnLevelUp(int level)
    {
        levelText.text = "" + level;
    }

    public Character GetCharacter()
    {
        return character;
    }

    private void LateUpdate()
    {
        if (character)
        {
            Vector3 position = characterCamera.WorldToScreenPoint(character.transform.position);
            GetComponent<RectTransform>().anchoredPosition =
                Vector2.right * (position.x + healthBarOffset.x) * (1920f / Screen.width) * xRatioOffset +
                Vector2.up * (position.y + healthBarOffset.y) * (1080f / Screen.height) +
                characterYOffset;
        }
        else
        {
            enabled = false;
        }
    }

    private void OnCurrentHealthChanged(float currentHealthValue)
    {
        SetMaxHealth();
        UpdateHealthTransform(currentHealthValue);
        UpdateShieldTransforms();
        SetHealthBarSeparators();
    }

    private void OnCurrentResourceChanged(float currentResourceValue)
    {
        maxResource = character.EntityStatsManager.Resource.GetTotal();
        resourceImage.fillAmount = currentResourceValue / maxResource;
    }

    private void OnShieldChanged(ShieldType shieldType, float shieldValue)
    {
        shields[(int)shieldType] = shieldValue;

        UpdateHealthTransform(character.EntityStatsManager.Health.GetCurrentValue());
        UpdateShieldTransforms();
        SetHealthBarSeparators();
    }

    private void UpdateHealthTransform(float currentHealthValue)
    {
        healthImageTransform.localScale = new Vector3(currentHealthValue / GetTotalHealthValue(), 1, 1);
    }

    private void SetMaxHealth()
    {
        maxHealth = character.EntityStatsManager.Health.GetTotal();
    }

    private void UpdateShieldTransforms()
    {
        float totalHealthValue = GetTotalHealthValue();

        if (!PhysicalShieldExists())
        {
            physicalShieldImage.gameObject.SetActive(false);
        }
        else
        {
            if (!physicalShieldImage.gameObject.activeSelf)
            {
                physicalShieldImage.gameObject.SetActive(true);
            }

            SetShieldSize(physicalShieldImageTransform, GetPhysicalShieldValue(), totalHealthValue);
            SetShieldPositionOnHealthBar(physicalShieldImageTransform, healthImageTransform);
        }

        if (!MagicShieldExists())
        {
            magicShieldImage.gameObject.SetActive(false);
        }
        else
        {
            if (!magicShieldImage.gameObject.activeSelf)
            {
                magicShieldImage.gameObject.SetActive(true);
            }

            SetShieldSize(magicShieldImageTransform, GetMagicShieldValue(), totalHealthValue);
            if (!PhysicalShieldExists())
            {
                SetShieldPositionOnHealthBar(magicShieldImageTransform, healthImageTransform);
            }
            else
            {
                SetShieldPositionOnHealthBar(magicShieldImageTransform, physicalShieldImageTransform);
            }
        }

        if (!NormalShieldExists())
        {
            shieldImage.gameObject.SetActive(false);
        }
        else
        {
            if (!shieldImage.gameObject.activeSelf)
            {
                shieldImage.gameObject.SetActive(true);
            }

            SetShieldSize(shieldImageTransform, GetNormalShieldValue(), totalHealthValue);
            if (!MagicShieldExists())
            {
                if (!PhysicalShieldExists())
                {
                    SetShieldPositionOnHealthBar(shieldImageTransform, healthImageTransform);
                }
                else
                {
                    SetShieldPositionOnHealthBar(shieldImageTransform, physicalShieldImageTransform);
                }
            }
            else
            {
                SetShieldPositionOnHealthBar(shieldImageTransform, magicShieldImageTransform);
            }
        }
    }

    private void SetShieldSize(RectTransform shieldTransform, float shieldValue, float totalHealthValue)
    {
        shieldTransform.localScale = new Vector3(shieldValue / totalHealthValue, 1, 1);
    }

    private void SetShieldPositionOnHealthBar(RectTransform shieldTransform, RectTransform transformToTouch)
    {
        shieldTransform.localPosition = new Vector3(transformToTouch.localPosition.x + transformToTouch.localScale.x * healthWidth, 3, 0);
    }

    private bool PhysicalShieldExists()
    {
        return GetPhysicalShieldValue() > 0;
    }

    private bool MagicShieldExists()
    {
        return GetMagicShieldValue() > 0;
    }

    private bool NormalShieldExists()
    {
        return GetNormalShieldValue() > 0;
    }

    private float GetPhysicalShieldValue()
    {
        return shields[1];
    }

    private float GetMagicShieldValue()
    {
        return shields[2];
    }

    private float GetNormalShieldValue()
    {
        return shields[0];
    }

    private float GetTotalHealthValue()
    {
        float GetCurrentHealthValue = GetTotalShieldValue() + character.EntityStatsManager.Health.GetCurrentValue();
        if (maxHealth >= GetCurrentHealthValue)
        {
            return maxHealth;
        }
        else
        {
            return GetCurrentHealthValue;
        }
    }

    private float GetTotalShieldValue()
    {
        return shields[0] + shields[1] + shields[2];
    }
}
