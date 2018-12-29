using UnityEngine;

public abstract class PassiveTargeted : Ability
{
    public override bool CanBeCast(Vector3 mousePosition)
    {
        return true;
    }

    public override bool CanBeCast(Unit target)
    {
        return true;
    }

    public override Vector3 GetDestination()
    {
        return Vector3.down;
    }

    public override void UseAbility(Vector3 destination) { }
    public override void UseAbility(Unit target) { }

    protected void PassiveEffect(Ability ability, Unit unitHit)
    {
        PassiveEffect(ability);
    }

    protected void PassiveEffect(Ability ability)
    {
        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);
    }
}
