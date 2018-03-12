using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticObjects : MonoBehaviour
{
    public static Character Character { get; set; }
    public static Camera CharacterCamera { get; set; }
    public static bool OnlineMode { get; set; }
    public static TerrainCollider Terrain { get; set; }
    public static float MultiplyingFactor { get; private set; }

    private StaticObjects()
    {
        MultiplyingFactor = 0.01f;
    }
}
