using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvent : MonoBehaviour
{
    private void OnMouseEnter()
    {
        StaticObjects.Character.CharacterMouseManager.HoverEntity(GetComponent<Entity>());
    }

    private void OnMouseExit()
    {
        StaticObjects.Character.CharacterMouseManager.UnhoverEntity(GetComponent<Entity>());
    }
}
