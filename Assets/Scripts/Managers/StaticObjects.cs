using System.Collections.Generic;
using UnityEngine;

public class StaticObjects : MonoBehaviour
{
    public static Champion Champion { get; set; }
    public static Camera ChampionCamera { get; set; }
    public static bool OnlineMode { get; set; }
    public static TerrainCollider Terrain { get; set; }
    public static float MultiplyingFactor { get; private set; }
    public static Dictionary<string, Unit> Units { get; set; }

    private StaticObjects()
    {
        MultiplyingFactor = 0.01f;
    }
}
