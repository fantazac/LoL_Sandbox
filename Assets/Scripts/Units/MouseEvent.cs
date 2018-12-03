using UnityEngine;

public class MouseEvent : MonoBehaviour
{
    private void OnMouseEnter()
    {
        StaticObjects.Character.CharacterMouseManager.HoverUnit(GetComponentInParent<Unit>());
    }

    private void OnMouseExit()
    {
        StaticObjects.Character.CharacterMouseManager.UnhoverUnit(GetComponentInParent<Unit>());
    }
}
