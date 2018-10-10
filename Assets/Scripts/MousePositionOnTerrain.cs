using UnityEngine;

public static class MousePositionOnTerrain
{
    public static bool GetRaycastHit(Vector3 mousePosition, out RaycastHit hit)
    {
        return StaticObjects.Terrain.Raycast(GetRay(mousePosition), out hit, Mathf.Infinity);
    }

    private static Ray GetRay(Vector3 mousePosition)
    {
        return StaticObjects.CharacterCamera.ScreenPointToRay(mousePosition);
    }
}
