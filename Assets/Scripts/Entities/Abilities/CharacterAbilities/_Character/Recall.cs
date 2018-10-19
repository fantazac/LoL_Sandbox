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

        AbilityBuffs[0].OnAbilityBuffRemoved += RemoveBuffFromEntityHit;
    }

    public override Vector3 GetDestination()
    {
        return character.CharacterMovement.CharacterHeightOffset;
    }

    protected override IEnumerator AbilityWithCastTimeAndChannelTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        character.CharacterMovement.StopAllMovement();
        AddNewBuffToEntityHit(character);
        IsBeingChanneled = true;

        yield return delayChannelTime;

        IsBeingChanneled = false;
        AbilityBuffs[0].ConsumeBuff(character);

        transform.position = GetDestination();
        character.CharacterMovement.NotifyCharacterMoved();

        FinishAbilityCast();
    }

    private void AddNewBuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.Health.OnHealthReduced += CancelRecall;
        ((Character)entityHit).CharacterAbilityManager.OnAnAbilityUsed += CancelRecall;
        ((Character)entityHit).CharacterMovement.CharacterMoved += CancelRecall;
        AbilityBuffs[0].AddNewBuffToAffectedEntity(entityHit);
    }

    private void RemoveBuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.Health.OnHealthReduced -= CancelRecall;
        ((Character)entityHit).CharacterAbilityManager.OnAnAbilityUsed -= CancelRecall;
        ((Character)entityHit).CharacterMovement.CharacterMoved -= CancelRecall;
    }

    private void CancelRecall()
    {
        IsBeingChanneled = false;
        CancelAbility();
        AbilityBuffs[0].ConsumeBuff(character);
    }
}
