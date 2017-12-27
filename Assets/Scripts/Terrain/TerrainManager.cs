using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    private void Start()
    {
        StaticObjects.Terrain = GetComponent<TerrainCollider>();
    }
}
