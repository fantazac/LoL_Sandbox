using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_W_Passive : PassiveTargeted, CharacterAbility
{
    private float initialBuffFlatBonus;

    private float passiveBuffActivationTime;
    private WaitForSeconds delayPassiveBuffActivation;

    private IEnumerator passiveBuffActivationCoroutine;

    protected MissFortune_W_Passive()
    {
        abilityType = AbilityType.Passive;

        DoNotShowAbilityOnUI = true;

        initialBuffFlatBonus = 25;
        buffFlatBonus = 60;
        buffFlatBonusPerLevel = 10;

        passiveBuffActivationTime = 5;
        delayPassiveBuffActivation = new WaitForSeconds(passiveBuffActivationTime);

        affectedByCooldownReduction = true;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW_Buff_Passive";
    }

    protected override void Start()
    {
        base.Start();

        character.EntityStats.Health.OnHealthReduced += PreparePassiveBuffCooldown;
    }

    private void PreparePassiveBuffCooldown()
    {
        ConsumeBuff(character);
        if (passiveBuffActivationCoroutine != null)
        {
            StopCoroutine(passiveBuffActivationCoroutine);
        }
        passiveBuffActivationCoroutine = PassiveBuffCooldown();
    }

    private IEnumerator PassiveBuffCooldown()
    {
        yield return delayPassiveBuffActivation;

        AddNewPassiveBuffToEntityHit(character);
    }

    protected virtual void AddNewPassiveBuffToEntityHit(Entity entityHit)
    {
        Buff passiveBuff = entityHit.EntityBuffManager.GetBuff(this);
        if (passiveBuff == null)
        {
            passiveBuff = new Buff(this, entityHit, false, buffDuration);
            entityHit.EntityBuffManager.ApplyBuff(passiveBuff, buffSprite);
        }
    }
}
