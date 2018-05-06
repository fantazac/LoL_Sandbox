using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_W : SelfTargeted
{
    private float baseCooldownReductionOnPassiveHit;
    private float cooldownReductionOnPassiveHit;

    private IEnumerator passiveBuffCooldownAfterTakingDamage;
    private float timeBeforeEnablingPassive;
    private WaitForSeconds delayPassiveBuff;

    protected MissFortune_W()
    {
        abilityName = "Strut";

        abilityType = AbilityType.Passive;

        MaxLevel = 5;

        resourceCost = 30;
        baseCooldown = 12;

        affectedByCooldownReduction = true;
        startCooldownOnAbilityCast = true;

        baseCooldownReductionOnPassiveHit = 2;

        timeBeforeEnablingPassive = 5;
        delayPassiveBuff = new WaitForSeconds(timeBeforeEnablingPassive);
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<MissFortune_W_PassiveBuff>(), gameObject.AddComponent<MissFortune_W_Buff>() };

        GetComponent<MissFortune_P>().OnPassiveHit += OnPassiveHit;
        character.EntityStats.Health.OnHealthReduced += OnDamageTaken;
    }

    protected override void SetCooldownForAbilityAffectedByCooldownReduction(float cooldownReduction)
    {
        base.SetCooldownForAbilityAffectedByCooldownReduction(cooldownReduction);

        cooldownReductionOnPassiveHit = baseCooldownReductionOnPassiveHit * (1 - (cooldownReduction * 0.01f));
    }

    private void OnPassiveHit()
    {
        if (IsOnCooldown)
        {
            cooldownRemaining -= cooldownReductionOnPassiveHit;
        }
    }

    private void OnDamageTaken()
    {
        if (AbilityLevel > 0)
        {
            AbilityBuffs[0].ConsumeBuff(character);

            if (passiveBuffCooldownAfterTakingDamage != null)
            {
                StopCoroutine(passiveBuffCooldownAfterTakingDamage);
            }
            passiveBuffCooldownAfterTakingDamage = PassiveBuffCooldownAfterTakingDamage();
            StartCoroutine(passiveBuffCooldownAfterTakingDamage);
        }
    }

    private void OnRevive()//TODO
    {
        if (AbilityLevel > 0)
        {
            EnableAbilityPassive();
        }
    }

    public override void UseAbility(Vector3 destination)
    {
        AbilityBuffs[1].AddNewBuffToAffectedEntity(character);

        StartCooldown(startCooldownOnAbilityCast);
    }

    public override void EnableAbilityPassive()
    {
        AbilityBuffs[0].AddNewBuffToAffectedEntity(character);
    }

    private IEnumerator PassiveBuffCooldownAfterTakingDamage()
    {
        yield return delayPassiveBuff;

        passiveBuffCooldownAfterTakingDamage = null;
        EnableAbilityPassive();
    }
}
