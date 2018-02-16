using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouseManager : MonoBehaviour
{
    public Entity HoveredEntity { get; private set; }
    public Entity ClickedEntity { get; private set; }

    private Character character;

    private RaycastHit hit;

    private void Start()
    {
        character = StaticObjects.Character;
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
        if (HoveredEntity == unhoveredEntity)
        {
            HoveredEntity = null;
        }
    }

    public bool HoveredEntityIsAnEnemy(EntityTeam team)
    {
        if (HoveredEntity != null)
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
        bool hitTerrain = MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
        Collider[] colliders = Physics.OverlapSphere(hit.point, 0.3f);
        Entity closestEnemyEntity = FindClosestEnemyEntity(colliders);

        if (HoveredEntityIsAnEnemy(character.Team))
        {
            character.CharacterMovement.PrepareMovementTowardsTarget(HoveredEntity);
        }
        else if (closestEnemyEntity != null)
        {
            character.CharacterMovement.PrepareMovementTowardsTarget(closestEnemyEntity);
        }
        else if (hitTerrain)
        {
            character.CharacterMovement.PrepareMovementTowardsPoint(hit.point);
        }
    }

    private Entity FindClosestEnemyEntity(Collider[] colliders)
    {
        Entity closestEnemyEntity = null;
        foreach (Collider collider in colliders)
        {
            closestEnemyEntity = collider.GetComponent<Entity>();
            if (closestEnemyEntity != null && closestEnemyEntity.Team != character.Team)
            {
                break;
            }
            else
            {
                closestEnemyEntity = null;
            }
        }
        return closestEnemyEntity;
    }
}
