using UnityEngine;

public abstract class UnitTargeted : Ability, IUnitTargeted
{
    public virtual bool CanBeCast(Unit target)
    {
        return target.IsTargetable(affectedUnitTypes, affectedTeams);
    }

    public virtual void UseAbility(Unit target)
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= range)
        {
            UseAbilityInRange(target);
        }
        else if (!champion.StatusManager.StatusEffectsOnCharacter.Contains(StatusEffect.ROOT))
        {
            champion.ChampionMovementManager.SetMoveTowardsTarget(target, range, false);
            champion.ChampionMovementManager.OnChampionIsInTargetRange += UseAbilityInRange;
        }
    }

    private void UseAbilityInRange(Unit target)
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
}
