using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvent : MonoBehaviour
{
    private void OnMouseEnter()
    {
        StaticObjects.Character.CharacterMouseManager.HoverObject(gameObject);
    }

    private void OnMouseExit()
    {
        StaticObjects.Character.CharacterMouseManager.UnhoverObject(gameObject);
    }
}
