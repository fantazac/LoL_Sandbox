using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Unit HoveredUnit { get; private set; }
    //public Unit ClickedUnit { get; private set; }

    private Champion champion;

    private RaycastHit hit;

    private const float RADIUS_RUBBER_BANDING = 0.3f;

    private void Start()
    {
        champion = StaticObjects.Champion;

        if (!champion.IsLocalChampion()) return;

        champion.InputManager.OnLeftClick += PressedLeftClick;
        champion.InputManager.OnRightClick += PressedRightClick;
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

    private bool HoveredUnitIsAnEnemy(Team team)
    {
        if (HoveredUnit)
        {
            return team != HoveredUnit.Team;
        }

        return false;
    }

    private void PressedLeftClick(Vector3 mousePosition)
    {
        if (champion.InfoUIManager)
        {
            champion.InfoUIManager.SetSelectedUnit(HoveredUnit != champion ? HoveredUnit : null);
        }
    }

    private void PressedRightClick(Vector3 mousePosition)
    {
        bool hitTerrain = MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
        Unit closestEnemyUnit = FindClosestEnemyUnit();

        if (HoveredUnitIsAnEnemy(champion.Team))
        {
            champion.ChampionMovementManager.PrepareMovementTowardsTarget(HoveredUnit);
            champion.InfoUIManager.SetSelectedUnit(HoveredUnit, true);
        }
        else if (closestEnemyUnit != null)
        {
            champion.ChampionMovementManager.PrepareMovementTowardsTarget(closestEnemyUnit);
            champion.InfoUIManager.SetSelectedUnit(closestEnemyUnit, true);
        }
        else if (hitTerrain)
        {
            champion.ChampionMovementManager.PrepareMovementTowardsPoint(hit.point);
        }
    }

    private Unit FindClosestEnemyUnit()
    {
        Unit closestEnemyUnit = null;
        foreach (Collider other in Physics.OverlapSphere(hit.point, RADIUS_RUBBER_BANDING))
        {
            closestEnemyUnit = other.GetComponentInParent<Unit>();
            if (closestEnemyUnit && closestEnemyUnit.Team != champion.Team)
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
