using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouseManager : MonoBehaviour
{
    public Entity HoveredEntity { get; private set; }
    public Entity ClickedEntity { get; private set; }

    private Character character;

    private RaycastHit hit;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        if (!StaticObjects.OnlineMode || character.PhotonView.isMine)
        {
            character.CharacterInput.OnRightClick += PressedRightClick;
        }
    }

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

    private void PressedLeftClick(Vector3 mousePosition)
    {
        //TODO
    }

    private void PressedRightClick(Vector3 mousePosition)
    {
        if (HoveredEntityIsAnEnemy(character.Team))
        {
            character.CharacterMovement.PrepareMovementTowardsTarget(HoveredEntity);
        }
        else if (MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit))
        {
            character.CharacterMovement.PrepareMovementTowardsPoint(hit.point);
        }
    }
}
