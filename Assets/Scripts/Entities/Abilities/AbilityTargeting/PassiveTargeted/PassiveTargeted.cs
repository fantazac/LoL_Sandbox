using UnityEngine;

public abstract class PassiveTargeted : Ability
{
    public override bool CanBeCast(Vector3 mousePosition)
    {
        return true;
    }

    public override bool CanBeCast(Entity target)
    {
        return true;
    }

    public override Vector3 GetDestination()
    {
        return Vector3.down;
    }

    public override void UseAbility(Vector3 destination) { }
    public override void UseAbility(Entity target) { }

    protected void PassiveEffect(Ability ability, Entity entityHit)
    {
        PassiveEffect(ability);
    }

    protected void PassiveEffect(Ability ability)
    {
        AbilityBuffs[0].AddNewBuffToAffectedEntity(character);
    }
}
