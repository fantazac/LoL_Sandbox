using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AutoTargeted : Ability
{
    public override bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager)
    {
        return true;
    }

    public override Vector3 GetDestination()
    {
        return Vector3.zero; // This will change
    }
}
