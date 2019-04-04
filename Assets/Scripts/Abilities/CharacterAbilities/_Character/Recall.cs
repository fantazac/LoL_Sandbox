using System.Collections;
using UnityEngine;

public class Recall : AutoTargetedBlink
{
    protected Recall()
    {
        abilityName = "Recall";

        abilityType = AbilityType.BLINK;

        castTime = 0.5f;
        channelTime = 8;
        delayCastTime = new WaitForSeconds(castTime);
        delayChannelTime = new WaitForSeconds(channelTime);

        AbilityLevel = 1;

        CanMoveWhileChanneling = true;
        CanUseAnyAbilityWhileChanneling = true;

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/_Character/Recall";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Recall_Buff>() };

        AbilityBuffs[0].OnAbilityBuffRemoved += RemoveBuffFromAffectedUnit;
    }

    public override Vector3 GetDestination()
    {
        return champion.CharacterHeightOffset;
    }

    protected override IEnumerator AbilityWithCastTimeAndChannelTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        champion.MovementManager.StopMovement(); //This is to cancel any movement command made during the cast time
        AddNewBuffToAffectedUnit(champion);
        IsBeingChanneled = true;

        yield return delayChannelTime;

        IsBeingChanneled = false;
        AbilityBuffs[0].ConsumeBuff(champion);

        transform.position = GetDestination();
        champion.ChampionMovementManager.NotifyChampionMoved();

        FinishAbilityCast();
    }

    private void AddNewBuffToAffectedUnit(Unit affectedUnit)
    {
        champion.StatsManager.Health.OnResourceReduced += CancelRecall;
        champion.AbilityManager.OnAnAbilityUsed += CancelRecall;
        champion.ChampionMovementManager.ChampionMoved += CancelRecall;
        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);
    }

    private void RemoveBuffFromAffectedUnit(Unit affectedUnit)
    {
        champion.StatsManager.Health.OnResourceReduced -= CancelRecall;
        champion.AbilityManager.OnAnAbilityUsed -= CancelRecall;
        champion.ChampionMovementManager.ChampionMoved -= CancelRecall;
    }

    private void CancelRecall()
    {
        IsBeingChanneled = false;
        CancelAbility();
        AbilityBuffs[0].ConsumeBuff(champion);
    }
}
