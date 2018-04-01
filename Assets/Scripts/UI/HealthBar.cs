using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;
    [SerializeField]
    private Image resourceImage;
    [SerializeField]
    private Text levelText;

    private Character character;

    private Vector2 healthBarOffset;
    private Vector2 characterYOffset;

    private float xRatioOffset;

    private Camera characterCamera;
    private Rect canvas;

    private float maxHealth;
    private float maxResource;

    [SerializeField]
    private GameObject mark100Prefab;
    [SerializeField]
    private GameObject mark1000Prefab;

    private float healthWidth;
    private Vector2 healthBarXOrigin;

    private HealthBar()
    {
        healthWidth = 105;
        healthBarXOrigin = Vector2.right * -0.5f * healthWidth;
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

        character.EntityStats.Health.OnCurrentHealthValueChanged += OnCurrentHealthChanged;
        character.EntityStats.Health.OnMaxHealthValueChanged += OnMaxHealthChanged;

        maxHealth = character.EntityStats.Health.GetTotal();
        if (character.EntityStats.Resource)
        {
            character.EntityStats.Resource.OnCurrentResourceValueChanged += OnCurrentResourceChanged;
            character.EntityStats.Resource.OnMaxResourceValueChanged += OnMaxResourceChanged;
            maxResource = character.EntityStats.Resource.GetTotal();
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

        SetHealthBarSeparators();
    }

    private void SetHealthBarSeparators()
    {
        Transform[] childrenTransforms = healthImage.transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childrenTransforms.Length; i++)
        {
            Destroy(childrenTransforms[i].gameObject);
        }
        float healthSeparatorFactor = GetHealthSeperatorFactor();
        float percentPer100Health = healthSeparatorFactor / maxHealth;
        Vector2 widthPer100Health = Vector2.right * (float)(decimal.Round((decimal)(percentPer100Health * healthWidth), 2, System.MidpointRounding.AwayFromZero));
        float modulo = 1000f / healthSeparatorFactor;
        float loopValue = maxHealth / healthSeparatorFactor;
        for (int i = 1; i < loopValue; i++)
        {
            GameObject separator;
            if (i % modulo == 0)
            {
                separator = Instantiate(mark1000Prefab);
                separator.transform.SetParent(healthImage.transform, false);
                separator.GetComponent<RectTransform>().anchoredPosition = healthBarXOrigin + (widthPer100Health * i);
            }
            else
            {
                separator = Instantiate(mark100Prefab);
                separator.transform.SetParent(healthImage.transform, false);
                separator.GetComponent<RectTransform>().anchoredPosition = healthBarXOrigin + (widthPer100Health * i) + Vector2.up * 3;
            }

        }
        OnCurrentHealthChanged();
        OnCurrentResourceChanged();
    }

    private float GetHealthSeperatorFactor()
    {
        if (maxHealth < 3000)
        {
            return 100f;
        }
        else if (maxHealth < 6000)
        {
            return 200f;
        }
        else if (maxHealth < 10000)
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

    private void OnCurrentHealthChanged()
    {
        healthImage.fillAmount = character.EntityStats.Health.GetCurrentValue() / maxHealth;
    }

    private void OnMaxHealthChanged()
    {
        maxHealth = character.EntityStats.Health.GetTotal();
        SetHealthBarSeparators();
    }

    private void OnCurrentResourceChanged()
    {
        resourceImage.fillAmount = character.EntityStats.Resource.GetCurrentValue() / maxResource;
    }

    private void OnMaxResourceChanged()
    {
        maxResource = character.EntityStats.Resource.GetTotal();
    }
}
