using UnityEngine;

public abstract class UnitTargeted : Ability
{
    public override bool CanBeCast(Unit target)
    {
        return IsAValidTarget(target);
    }

    protected bool IsAValidTarget(Unit target)
    {
        return target.IsTargetable(affectedUnitTypes, affectedTeams);
    }

    public override void UseAbility(Unit target)
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= range)
        {
            UseAbilityInRange(target);
        }
        else if (!champion.StatusManager.StatusEffectsOnCharacter.Contains(StatusEffect.ROOT))
        {
            champion.ChampionMovementManager.SetMoveTowardsTarget(target, range, false);
            champion.ChampionMovementManager.ChampionIsInTargetRange += UseAbilityInRange;
        }
    }

    protected virtual void UseAbilityInRange(Unit target)
    {
        StartAbilityCast();

        if (!champion.ChampionMovementManager.IsMovingTowardsTarget() && !champion.BasicAttack.CurrentTarget())
        {
            champion.ChampionMovementManager.SetMoveTowardsTarget(target, champion.StatsManager.AttackRange.GetTotal(), true, true);
        }

        targetedUnit = target;
        destinationOnCast = target.transform.position;
        RotationOnAbilityCast(target.transform.position);

        StartCorrectCoroutine();
    }

    public override bool CanBeCast(Vector3 mousePosition) { return false; }
    public override Vector3 GetDestination() { return Vector3.down; }
    public override void UseAbility(Vector3 destination) { }
}
