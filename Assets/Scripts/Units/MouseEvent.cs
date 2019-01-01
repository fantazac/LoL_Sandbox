using UnityEngine;

public class MouseEvent : MonoBehaviour
{
    private void OnMouseEnter()
    {
        StaticObjects.Champion.MouseManager.HoverUnit(GetComponentInParent<Unit>());
    }

    private void OnMouseExit()
    {
        StaticObjects.Champion.MouseManager.UnhoverUnit(GetComponentInParent<Unit>());
    }
}
