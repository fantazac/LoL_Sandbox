using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tristana_W : DirectionTargetedDash //TODO: GroundTargetedDash
{
    private float effectRadius;

    protected Tristana_W()
    {
        abilityName = "Rocket Jump";

        abilityType = AbilityType.DASH;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        damageType = DamageType.MAGIC;
        effectType = AbilityEffectType.AREA_OF_EFFECT;

        MaxLevel = 5;

        damage = 85; // 85/135/185/235/285
        damagePerLevel = 50;
        range = 900;
        resourceCost = 60;
        baseCooldown = 22; // 22/20/18/16/14
        baseCooldownPerLevel = -2;
        dashSpeed = 14; //TODO: VERIFY ACTUAL VALUE
        castTime = 0.35f; //TODO: VERIFY ACTUAL VALUE
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

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
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
        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        foreach (Collider other in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            Unit tempUnit = other.GetComponentInParent<Unit>();

            if (!tempUnit || !tempUnit.IsTargetable(affectedUnitTypes, affectedTeams)) continue;

            float abilityDamage = GetAbilityDamage(tempUnit);
            DamageUnit(tempUnit, abilityDamage);
            AbilityHit(tempUnit, abilityDamage);
            AbilityDebuffs[0].AddNewBuffToAffectedUnit(tempUnit);
        }
    }
}
