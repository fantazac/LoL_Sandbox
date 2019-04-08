using UnityEngine;

public class ChampionStatusManager : StatusManager
{
    private Champion champion;

    private void Start()
    {
        champion = GetComponent<Champion>();
    }

    protected override void OnDisarm()
    {
        champion.AbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
        champion.AbilityManager.CancelAllChannelingAbilities(); //TODO: verify, wiki says yes though
    }

    protected override void OnDisrupt()
    {
        champion.AbilityManager.CancelAllChannelingAbilities();
    }

    protected override void OnEntangle()
    {
        OnDisarm();
        champion.ChampionMovementManager.StopMovementTowardsPointIfHasEvent();
    }

    protected override void OnForcedAction()
    {
        champion.AbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
        champion.AbilityManager.CancelAllChannelingAbilities();
        champion.MovementManager.StopMovement();
    }

    protected override void OnKnockDown()
    {
        champion.DisplacementManager.StopCurrentDisplacement();
    }

    protected override void OnKnockUp()
    {
        champion.AbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
        champion.AbilityManager.CancelAllChannelingAbilities();
        champion.ChampionMovementManager.SetCharacterIsInTargetRangeEventForBasicAttack();
        champion.ChampionMovementManager.SetMoveTowardsHalfDistanceOfAbilityCastRange();
    }

    protected override void OnRoot()
    {
        champion.AbilityManager.CancelAllChannelingAbilities();
        champion.ChampionMovementManager.StopMovementTowardsPointIfHasEvent();
    }

    protected override void OnSilence()
    {
        champion.AbilityManager.CancelAllChannelingAbilities();
    }

    protected override void OnStun()
    {
        champion.AbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
        champion.AbilityManager.CancelAllChannelingAbilities();
        champion.ChampionMovementManager.StopMovementTowardsPointIfHasEvent();
    }

    protected override void OnSuspension()
    {
        OnForcedAction();
        if (!champion.ChampionMovementManager.IsMovingTowardsPositionForAnEvent() && !champion.ChampionMovementManager.IsMovingTowardsTarget())
        {
            champion.AutoAttackManager.EnableAutoAttackWithBiggerRange();
        }
    }

    protected override void SetCannotUseBasicAbilities(int count)
    {
        base.SetCannotUseBasicAbilities(count);

        if (CanBlockAbilitiesOrBasicAttacks(count, blockBasicAbilitiesCount))
        {
            champion.AbilityManager.BlockAllBasicAbilities();
        }
        else if (CanUnblockAbilitiesOrBasicAttacks(count, blockBasicAbilitiesCount))
        {
            champion.AbilityManager.UnblockAllBasicAbilities(blockMovementAbilitiesCount == 0, blockSummonerAbilitiesCount == 0);
        }
    }

    protected override void SetCannotUseBasicAttacks(int count)
    {
        base.SetCannotUseBasicAttacks(count);

        if (CanBlockAbilitiesOrBasicAttacks(count, blockBasicAttacksCount))
        {
            champion.BasicAttack.StopBasicAttack(true);
        }
    }

    protected override void SetCannotUseMovementAbilities(int count)
    {
        base.SetCannotUseMovementAbilities(count);

        if (blockBasicAbilitiesCount != 0) return;

        if (CanBlockAbilitiesOrBasicAttacks(count, blockMovementAbilitiesCount))
        {
            champion.AbilityManager.BlockAllMovementAbilities();
        }
        else if (CanUnblockAbilitiesOrBasicAttacks(count, blockMovementAbilitiesCount))
        {
            champion.AbilityManager.UnblockAllMovementAbilities();
        }
    }

    protected override void SetCannotUseSummonerAbilities(int count)
    {
        base.SetCannotUseSummonerAbilities(count);

        if (CanBlockAbilitiesOrBasicAttacks(count, blockSummonerAbilitiesCount))
        {
            champion.AbilityManager.BlockAllSummonerAbilities();
        }
        else if (CanUnblockAbilitiesOrBasicAttacks(count, blockSummonerAbilitiesCount))
        {
            champion.AbilityManager.UnblockAllSummonerAbilities(blockMovementAbilitiesCount == 0);
        }
    }
}
