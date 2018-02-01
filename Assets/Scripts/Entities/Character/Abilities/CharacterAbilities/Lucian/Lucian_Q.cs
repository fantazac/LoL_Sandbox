﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_Q : UnitTargeted, CharacterAbility
{
    [SerializeField]
    protected GameObject areaOfEffectPrefab;

    protected float durationAoE;

    protected Lucian_Q()
    {
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.PHYSICAL;

        range = 500;
        damage = 130;
        cooldown = 6;
        castTime = 0.3f;
        delayCastTime = new WaitForSeconds(castTime);

        startCooldownOnFinishAbilityCast = true;

        durationAoE = 0.15f;

        HasCastTime = true;
    }

    protected override void Start()
    {
        CastableAbilitiesWhileActive.Add(GetComponent<Lucian_W>());
        CastableAbilitiesWhileActive.Add(GetComponent<Lucian_R>());

        base.Start();
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        positionOnCast = transform.position + (transform.forward * areaOfEffectPrefab.transform.localScale.z * 0.5f);

        yield return delayCastTime;

        AreaOfEffect aoe = ((GameObject)Instantiate(areaOfEffectPrefab, positionOnCast, transform.rotation)).GetComponent<AreaOfEffect>();
        aoe.ActivateAreaOfEffect(new List<Entity>(), character.Team, affectedUnitType, durationAoE);
        aoe.OnAbilityEffectHit += OnAreaOfEffectHit;
        FinishAbilityCast();
    }
}
