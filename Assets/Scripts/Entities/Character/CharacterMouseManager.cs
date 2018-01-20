using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouseManager : MonoBehaviour
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

    public bool HoveredObjectIsEnemy(EntityTeam team)
    {
        if(HoveredObject == null)
        {
            return false;
        }
        return team != HoveredObject.GetComponent<Character>().team;
    }
}
