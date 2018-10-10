using UnityEngine;
using UnityEngine.EventSystems;

public class UIAbility : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO: Cast with indicator
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TODO: Show tooltip
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //TODO: Hide tooltip
    }
}
