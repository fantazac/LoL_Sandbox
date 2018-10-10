using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    private void Start()
    {
        StaticObjects.Terrain = GetComponent<TerrainCollider>();
    }
}
