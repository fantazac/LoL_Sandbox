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
        return character.MovementManager.CharacterHeightOffset;
    }

    protected override IEnumerator AbilityWithCastTimeAndChannelTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        character.MovementManager.StopAllMovement();//This is to cancel any movement command made during the cast time
        AddNewBuffToAffectedUnit(character);
        IsBeingChanneled = true;

        yield return delayChannelTime;

        IsBeingChanneled = false;
        AbilityBuffs[0].ConsumeBuff(character);

        transform.position = GetDestination();
        character.MovementManager.NotifyCharacterMoved();

        FinishAbilityCast();
    }

    private void AddNewBuffToAffectedUnit(Unit affectedUnit)
    {
        affectedUnit.StatsManager.Health.OnResourceReduced += CancelRecall;
        ((Character)affectedUnit).AbilityManager.OnAnAbilityUsed += CancelRecall;
        ((Character)affectedUnit).MovementManager.CharacterMoved += CancelRecall;
        AbilityBuffs[0].AddNewBuffToAffectedUnit(affectedUnit);
    }

    private void RemoveBuffFromAffectedUnit(Unit affectedUnit)
    {
        affectedUnit.StatsManager.Health.OnResourceReduced -= CancelRecall;
        ((Character)affectedUnit).AbilityManager.OnAnAbilityUsed -= CancelRecall;
        ((Character)affectedUnit).MovementManager.CharacterMoved -= CancelRecall;
    }

    private void CancelRecall()
    {
        IsBeingChanneled = false;
        CancelAbility();
        AbilityBuffs[0].ConsumeBuff(character);
    }
}
