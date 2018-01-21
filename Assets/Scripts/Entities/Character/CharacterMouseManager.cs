using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouseManager : MonoBehaviour
{
    public Entity HoveredEntity { get; private set; }
    public Entity ClickedEntity { get; private set; }

    public void HoverEntity(Entity hoveredEntity)
    {
        HoveredEntity = hoveredEntity;
    }

    public void UnhoverEntity(Entity unhoveredEntity)
    {
        if(HoveredEntity == unhoveredEntity)
        {
            HoveredEntity = null;
        }
    }

    public bool HoveredEntityIsAnEnemy(EntityTeam team)
    {
        if(HoveredEntity != null)
        {
            return team != HoveredEntity.Team;
        }
        return false;
    }
}
