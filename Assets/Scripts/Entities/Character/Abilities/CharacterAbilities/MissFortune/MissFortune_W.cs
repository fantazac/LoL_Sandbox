using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_W : PassiveTargeted, CharacterAbility
{
    private float initialBuffFlatBonus;

    private float passiveBuffActivationTime;
    private WaitForSeconds delayPassiveBuffActivation;

    private IEnumerator passiveBuffActivationCoroutine;

    protected MissFortune_W()
    {
        abilityType = AbilityType.Passive;

        DoNotShowAbilityOnUI = true;

        MaxLevel = 5;

        buffDuration = 5;
        initialBuffFlatBonus = 25;
        buffFlatBonus = 60;
        buffFlatBonusPerLevel = 10;

        passiveBuffActivationTime = 5;
        delayPassiveBuffActivation = new WaitForSeconds(passiveBuffActivationTime);

        affectedByCooldownReduction = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW";
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW_Buff_Passive";
    }

    protected override void Start()
    {
        base.Start();

        character.EntityStats.Health.OnHealthReduced += PreparePassiveBuffCooldown;
    }

    public override void EnableAbility()
    {
        base.EnableAbility();

        AddNewBuffToEntityHit(character);
    }

    private void PreparePassiveBuffCooldown()
    {
        if (passiveBuffActivationCoroutine != null)
        {
            StopCoroutine(passiveBuffActivationCoroutine);
            passiveBuffActivationCoroutine = null;
        }
        ConsumeBuff(character);
        passiveBuffActivationCoroutine = PassiveBuffCooldown();
        StartCoroutine(passiveBuffActivationCoroutine);
    }

    private IEnumerator PassiveBuffCooldown()
    {
        yield return delayPassiveBuffActivation;

        AddNewBuffToEntityHit(character);
        passiveBuffActivationCoroutine = null;
    }

    protected override void AddNewBuffToEntityHit(Entity entityHit)
    {
        Buff passiveBuff = entityHit.EntityBuffManager.GetBuff(this);
        if (passiveBuff == null)
        {
            passiveBuff = new Buff(this, entityHit, false, buffDuration);
            passiveBuff.SetBuffValue((int)initialBuffFlatBonus);
            entityHit.EntityBuffManager.ApplyBuff(passiveBuff, buffSprite);
        }
        else
        {
            passiveBuff = new Buff(this, entityHit, false);
            passiveBuff.SetBuffValue((int)buffFlatBonus);
            entityHit.EntityBuffManager.ApplyBuff(passiveBuff, buffSprite);
        }
    }

    public override void LevelUpExtraStats()
    {
        UpdateBuffOnAffectedEntities(buffFlatBonus, buffFlatBonus + buffFlatBonusPerLevel);
    }

    protected override void UpdateBuffOnAffectedEntities(float oldValue, float newValue)
    {
        foreach (Entity affectedEntity in EntitiesAffectedByBuff)
        {
            Buff buff = affectedEntity.EntityBuffManager.GetBuff(this);
            if (buff != null && buff.BuffValue != initialBuffFlatBonus)
            {
                affectedEntity.EntityStats.MovementSpeed.RemoveFlatBonus(oldValue);
                affectedEntity.EntityStats.MovementSpeed.AddFlatBonus(newValue);
                buff.SetBuffValue((int)newValue);
            }
        }
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int buffValue)
    {
        entityHit.EntityStats.MovementSpeed.AddFlatBonus(buffValue);
        EntitiesAffectedByBuff.Add(entityHit);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int buffValue)
    {
        entityHit.EntityStats.MovementSpeed.RemoveFlatBonus(buffValue);
        EntitiesAffectedByBuff.Remove(entityHit);

        if (passiveBuffActivationCoroutine == null && buffValue == initialBuffFlatBonus)
        {
            AddNewBuffToEntityHit(character);
        }
    }
}
