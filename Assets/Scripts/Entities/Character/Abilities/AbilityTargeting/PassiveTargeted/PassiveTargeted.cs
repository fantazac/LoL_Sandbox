using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveTargeted : Ability
{
    public override bool CanBeCast(Vector3 mousePosition)
    {
        Debug.Log("This should not happen - CanBeCast(mousePosition) - " + this);
        return true;
    }

    public override bool CanBeCast(Entity target)
    {
        Debug.Log("This should not happen - CanBeCast(target) - " + this);
        return true;
    }

    public override Vector3 GetDestination()
    {
        Debug.Log("This should not happen - GetDestination - " + this);
        return Vector3.down;
    }

    public override void UseAbility(Vector3 destination)
    {
        Debug.Log("This should not happen - UseAbility(destination) - " + this);
    }

    public override void UseAbility(Entity target)
    {
        Debug.Log("This should not happen - UseAbility(target) - " + this);
    }

    protected void PassiveEffect(Ability ability)
    {
        AbilityBuffs[0].AddNewBuffToEntityHit(character);
    }
}
