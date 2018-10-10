using UnityEngine;

public class CharacterMouseManager : MonoBehaviour
{
    public Entity HoveredEntity { get; private set; }
    public Entity ClickedEntity { get; private set; }

    private Character character;

    private RaycastHit hit;

    private const float RADIUS_RUBBER_BANDING = 0.3f;

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
        Entity closestEnemyEntity = FindClosestEnemyEntity();

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

    private Entity FindClosestEnemyEntity()
    {
        Entity closestEnemyEntity = null;
        foreach (Collider collider in Physics.OverlapSphere(hit.point, RADIUS_RUBBER_BANDING))
        {
            closestEnemyEntity = collider.GetComponentInParent<Entity>();
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
