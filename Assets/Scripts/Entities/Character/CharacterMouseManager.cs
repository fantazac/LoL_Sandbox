using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouseManager : CharacterBase
{
    public GameObject HoveredObject { get; private set; }//ENTITY
    public GameObject ClickedObject { get; private set; }//ENTITY

    public void HoverObject(GameObject hoveredObject)
    {
        HoveredObject = hoveredObject;
    }

    public void UnhoverObject(GameObject unhoveredObject)
    {
        if(HoveredObject == unhoveredObject)
        {
            HoveredObject = null;
        }
    }
}
