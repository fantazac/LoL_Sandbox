﻿using System.Collections;
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

        character.CharacterLevelManager.OnLevelUp += OnLevelUp;
        OnLevelUp(1);

        character.EntityStats.Health.OnCurrentHealthValueChanged += OnCurrentHealthChanged;
        character.EntityStats.Health.OnMaxHealthValueChanged += OnMaxHealthChanged;

        maxHealth = character.EntityStats.Health.GetTotal();
        if (character.EntityStats.Resource)
        {
            character.EntityStats.Resource.OnCurrentResourceValueChanged += OnCurrentResourceChanged;
            character.EntityStats.Resource.OnMaxResourceValueChanged += OnMaxResourceChanged;
            maxResource = character.EntityStats.Resource.GetTotal();
        }
    }

    private void OnLevelUp(int level)
    {
        levelText.text = "" + level;
    }

    private void LateUpdate()
    {
        Vector3 position = characterCamera.WorldToScreenPoint(character.transform.position);
        GetComponent<RectTransform>().anchoredPosition =
            Vector2.right * (position.x + healthBarOffset.x) * (1920f / Screen.width) * xRatioOffset +
            Vector2.up * (position.y + healthBarOffset.y) * (1080f / Screen.height) +
            characterYOffset;
    }

    private void OnCurrentHealthChanged()
    {
        healthImage.fillAmount = character.EntityStats.Health.GetCurrentValue() / maxHealth;
    }

    private void OnMaxHealthChanged()
    {
        maxHealth = character.EntityStats.Health.GetTotal();
        //TODO: black bars to better see how much max health the characters have
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
