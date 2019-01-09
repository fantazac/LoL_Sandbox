using UnityEngine;
using UnityEngine.UI;

public class InfoUIManager : MonoBehaviour
{
    [SerializeField]
    private Image portraitImage;
    [SerializeField]
    private BuffUIManager buffUIManager;
    [SerializeField]
    private BuffUIManager debuffUIManager;

    private Unit selectedUnit;

    public void SetSelectedUnit(Unit toSelectUnit, bool fromRightClickEvent = false)
    {
        if ((selectedUnit || !fromRightClickEvent) && selectedUnit != toSelectUnit)
        {
            if (selectedUnit)
            {
                selectedUnit.BuffManager.SetUIManagers(null, null);
                buffUIManager.ResetBuffs();
                debuffUIManager.ResetBuffs();
            }
            else
            {
                buffUIManager.gameObject.SetActive(true);
                debuffUIManager.gameObject.SetActive(true);
                gameObject.SetActive(true);
            }

            selectedUnit = toSelectUnit;

            if (selectedUnit)
            {
                portraitImage.sprite = toSelectUnit.PortraitSprite;
                selectedUnit.BuffManager.SetUIManagers(buffUIManager, debuffUIManager);
            }
            else
            {
                buffUIManager.gameObject.SetActive(false);
                debuffUIManager.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}
