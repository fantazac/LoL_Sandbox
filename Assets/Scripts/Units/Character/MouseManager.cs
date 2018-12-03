using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Unit HoveredUnit { get; private set; }
    public Unit ClickedUnit { get; private set; }

    private Champion champion;

    private RaycastHit hit;

    private const float RADIUS_RUBBER_BANDING = 0.3f;

    private void Start()
    {
        champion = StaticObjects.Champion;
        if (champion.IsLocalChampion())
        {
            champion.InputManager.OnRightClick += PressedRightClick;
        }
    }

    public void HoverUnit(Unit hoveredUnit)
    {
        HoveredUnit = hoveredUnit;
    }

    public void UnhoverUnit(Unit unhoveredUnit)
    {
        if (HoveredUnit == unhoveredUnit)
        {
            HoveredUnit = null;
        }
    }

    public bool HoveredUnitIsAnEnemy(Team team)
    {
        if (HoveredUnit != null)
        {
            return team != HoveredUnit.Team;
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
        Unit closestEnemyUnit = FindClosestEnemyUnit();

        if (HoveredUnitIsAnEnemy(champion.Team))
        {
            champion.MovementManager.PrepareMovementTowardsTarget(HoveredUnit);
        }
        else if (closestEnemyUnit != null)
        {
            champion.MovementManager.PrepareMovementTowardsTarget(closestEnemyUnit);
        }
        else if (hitTerrain)
        {
            champion.MovementManager.PrepareMovementTowardsPoint(hit.point);
        }
    }

    private Unit FindClosestEnemyUnit()
    {
        Unit closestEnemyUnit = null;
        foreach (Collider collider in Physics.OverlapSphere(hit.point, RADIUS_RUBBER_BANDING))
        {
            closestEnemyUnit = collider.GetComponentInParent<Unit>();
            if (closestEnemyUnit != null && closestEnemyUnit.Team != champion.Team)
            {
                break;
            }
            else
            {
                closestEnemyUnit = null;
            }
        }
        return closestEnemyUnit;
    }
}
