using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelfTargeted : Ability
{
    public override bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager)
    {
        return true;
    }

    public override Vector3 GetDestination()
    {
        return character.transform.position;
    }

    public override bool CanBeCast(Entity target, CharacterAbilityManager characterAbilityManager) { return false; }
    public override void UseAbility(Entity target) { }
}
