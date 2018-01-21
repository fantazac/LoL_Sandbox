using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_Q : UnitTargeted, CharacterAbility
{
    [SerializeField]
    protected GameObject areaOfEffectPrefab;

    protected float durationAoE;

    protected Lucian_Q()
    {
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;

        range = 500;
        damage = 130;
        castTime = 0.3f;
        delayCastTime = new WaitForSeconds(castTime);

        durationAoE = 0.15f;

        CanStopMovement = true;
        HasCastTime = true;
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        AreaOfEffect aoe = ((GameObject)Instantiate(areaOfEffectPrefab, transform.position + (transform.forward * areaOfEffectPrefab.transform.localScale.z * 0.5f), transform.rotation)).GetComponent<AreaOfEffect>();
        aoe.ActivateAreaOfEffect(new List<Entity>(), character.Team, affectedUnitType, durationAoE);
        aoe.OnAbilityEffectHit += OnAreaOfEffectHit;
        FinishAbilityCast();
    }
}
