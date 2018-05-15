using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityLevelUpUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buffsUI;
    [SerializeField]
    private GameObject debuffsUI;
    [SerializeField]
    private List<UILevelUpArrow> uiLevelUpArrows;
    [SerializeField]
    private Text levelUpText;

    private string baseLevelUpText;

    private RectTransform buffsUITransform;
    private RectTransform debuffsUITransform;

    private Vector2 buffsUIInitialPosition;
    private Vector2 debuffsUIInitialPosition;

    private Vector2 buffsUISecondPosition;
    private Vector2 debuffsUISecondPosition;

    public delegate void OnAbilityLevelUpHandler(int abilityId);
    public event OnAbilityLevelUpHandler OnAbilityLevelUp;

    private AbilityLevelUpUIManager()
    {
        baseLevelUpText = "LEVEL UP! +";
    }

    private void Awake()
    {
        buffsUITransform = buffsUI.GetComponent<RectTransform>();
        debuffsUITransform = debuffsUI.GetComponent<RectTransform>();

        buffsUIInitialPosition = buffsUITransform.anchoredPosition;
        debuffsUIInitialPosition = debuffsUITransform.anchoredPosition;

        Vector2 upVector = Vector2.up * 80;

        buffsUISecondPosition = buffsUITransform.anchoredPosition + upVector;
        debuffsUISecondPosition = debuffsUITransform.anchoredPosition + upVector;

        foreach (UILevelUpArrow uiLevelUpArrow in uiLevelUpArrows)
        {
            uiLevelUpArrow.OnAbilityLevelUp += LevelUpAbility;
        }
    }

    private void OnEnable()
    {
        buffsUITransform.anchoredPosition = buffsUISecondPosition;
        debuffsUITransform.anchoredPosition = debuffsUISecondPosition;
    }

    private void OnDisable()
    {
        foreach (UILevelUpArrow uiLevelUpArrow in uiLevelUpArrows)
        {
            uiLevelUpArrow.SetArrowState(0);
        }
        buffsUITransform.anchoredPosition = buffsUIInitialPosition;
        debuffsUITransform.anchoredPosition = debuffsUIInitialPosition;
    }

    public void SetAbilityPoints(int abilityPoints, int pointsAvailableForQ, int pointsAvailableForW, int pointsAvailableForE, int pointsAvailableForR)
    {
        if (abilityPoints > 0)
        {
            levelUpText.text = baseLevelUpText + abilityPoints;
            uiLevelUpArrows[0].SetArrowState(pointsAvailableForQ);
            uiLevelUpArrows[1].SetArrowState(pointsAvailableForW);
            uiLevelUpArrows[2].SetArrowState(pointsAvailableForE);
            uiLevelUpArrows[3].SetArrowState(pointsAvailableForR);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void LevelUpAbility(int arrowId)
    {
        if (OnAbilityLevelUp != null)
        {
            OnAbilityLevelUp(arrowId);
        }
    }
}
