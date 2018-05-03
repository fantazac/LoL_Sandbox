using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundTargetedAoE : GroundTargeted
{
    protected string areaOfEffectPrefabPath;
    protected GameObject areaOfEffectPrefab;

    protected override void LoadPrefabs()
    {
        areaOfEffectPrefab = Resources.Load<GameObject>(areaOfEffectPrefabPath);
    }
}
