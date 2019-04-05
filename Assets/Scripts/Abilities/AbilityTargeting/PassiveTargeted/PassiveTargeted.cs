public abstract class PassiveTargeted : Ability
{
    protected void PassiveEffect(Ability ability, Unit unitHit)
    {
        PassiveEffect(ability);
    }

    protected void PassiveEffect(Ability ability)
    {
        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);
    }
}
