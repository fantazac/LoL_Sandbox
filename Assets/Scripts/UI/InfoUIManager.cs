using UnityEngine;
using UnityEngine.UI;

public class InfoUIManager : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private Text levelText;
    [SerializeField] private BuffUIManager buffUIManager;
    [SerializeField] private BuffUIManager debuffUIManager;
    [SerializeField] private HealthUIManager healthUIManager;

    private Unit selectedUnit;

    public void SetSelectedUnit(Unit toSelectUnit, bool fromRightClickEvent = false)
    {
        if ((!selectedUnit && fromRightClickEvent) || selectedUnit == toSelectUnit) return;

        if (selectedUnit)
        {
            selectedUnit.BuffManager.SetUIManagers(null, null);
            healthUIManager.ResetHealthAndResource();
            buffUIManager.ResetBuffs();
            debuffUIManager.ResetBuffs();
        }
        else
        {
            gameObject.SetActive(true);
        }

        selectedUnit = toSelectUnit;

        if (selectedUnit)
        {
            portraitImage.sprite = toSelectUnit.PortraitSprite;
            healthUIManager.SetHealthAndResource(selectedUnit);
            selectedUnit.BuffManager.SetUIManagers(buffUIManager, debuffUIManager);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
