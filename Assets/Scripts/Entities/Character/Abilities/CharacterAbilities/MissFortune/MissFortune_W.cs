using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_W : SelfTargeted, CharacterAbility
{
    private float baseCooldownReductionOnPassiveHit;
    private float cooldownReductionOnPassiveHit;

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

    public override void UseAbility(Vector3 destination)
    {
        AbilityBuffs[1].AddNewBuffToEntityHit(character);

        StartCooldown(startCooldownOnAbilityCast);
    }
}
