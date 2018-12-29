using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tristana_W : DirectionTargetedDash//TODO: GroundTargetedDash
{
    private float effectRadius;

    protected Tristana_W()
    {
        abilityName = "Rocket Jump";

        abilityType = AbilityType.DASH;
        affectedTeams = AffectedTeams.GetEnemyTeams(champion.Team);
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        damageType = DamageType.MAGIC;
        effectType = AbilityEffectType.AREA_OF_EFFECT;

        MaxLevel = 5;

        range = 900;
        resourceCost = 60;
        baseCooldown = 22;// 22/20/18/16/14
        baseCooldownPerLevel = -2;
        dashSpeed = 14;//TODO: VERIFY ACTUAL VALUE
        castTime = 0.35f;//TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        effectRadius = 350;

        IsAMovementAbility = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaW";
    }

    protected override void ModifyValues()
    {
        effectRadius *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }

    protected override void Start()
    {
        base.Start();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Tristana_W_Debuff>() };
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        RotationOnAbilityCast(destination);
        FinalAdjustments(destination);
        UseResource();

        SetupDash();
        champion.DisplacementManager.OnDisplacementFinished += ApplyDamageAndSlowToAllEnemiesInRadius;

        FinishAbilityCast();
    }

    private void ApplyDamageAndSlowToAllEnemiesInRadius()
    {
        Unit tempUnit;

        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            tempUnit = collider.GetComponentInParent<Unit>();
            if (tempUnit != null && TargetIsValid(tempUnit))
            {
                float damage = GetAbilityDamage(tempUnit);
                DamageUnit(tempUnit, damage);
                AbilityHit(tempUnit, damage);
                AbilityDebuffs[0].AddNewBuffToAffectedUnit(tempUnit);
            }
        }
    }
}
