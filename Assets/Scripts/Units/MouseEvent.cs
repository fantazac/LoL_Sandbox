using UnityEngine;

public class MouseEvent : MonoBehaviour
{
    private void OnMouseEnter()
    {
        StaticObjects.Character.MouseManager.HoverUnit(GetComponentInParent<Unit>());
    }

    private void OnMouseExit()
    {
        StaticObjects.Character.MouseManager.UnhoverUnit(GetComponentInParent<Unit>());
    }
}
