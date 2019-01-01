using UnityEngine;

public abstract class AutoTargeted : Ability
{
    public override bool CanBeCast(Vector3 mousePosition)
    {
        return true;
    }

    public override Vector3 GetDestination()
    {
        return Vector3.down; // This will change
    }

    public override bool CanBeCast(Unit target) { return false; }
    public override void UseAbility(Unit target) { }
}
