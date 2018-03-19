using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILevelUpArrow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int arrowId;

    private Image image;

    private Color unavailableColor;

    private bool arrowIsEnabled;

    public delegate void OnAbilityLevelUpHandler(int arrowId);
    public event OnAbilityLevelUpHandler OnAbilityLevelUp;

    private UILevelUpArrow()
    {
        unavailableColor = Color.gray;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        DisableArrow();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnAbilityLevelUp != null && arrowIsEnabled)
        {
            OnAbilityLevelUp(arrowId);
        }
    }

    public void SetArrowState(int abilityPoints)
    {
        if(abilityPoints > 0)
        {
            EnableArrow();
        }
        else
        {
            DisableArrow();
        }
    }

    private void EnableArrow()
    {
        arrowIsEnabled = true;
        image.color = Color.white;
    }

    private void DisableArrow()
    {
        arrowIsEnabled = false;
        image.color = unavailableColor;
    }
}
