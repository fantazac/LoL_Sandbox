using System.Collections;
using UnityEngine;

public class Tristana_W : DirectionTargetedDash//TODO: GroundTargetedDash
{
    private float effectRadius;

    protected Tristana_W()
    {
        abilityName = "Rocket Jump";

        abilityType = AbilityType.DASH;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        damageType = DamageType.MAGIC;
        effectType = AbilityEffectType.AREA_OF_EFFECT;

        MaxLevel = 5;

        damage = 85;// 85/135/185/235/285
        damagePerLevel = 50;
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
            if (tempUnit != null && tempUnit.IsTargetable(affectedUnitType, champion.Team))
            {
                float damage = GetAbilityDamage(tempUnit);
                Debug.Log(damage);
                DamageUnit(tempUnit, damage);
                AbilityHit(tempUnit, damage);
                AbilityDebuffs[0].AddNewBuffToAffectedUnit(tempUnit);
            }
        }
    }
}
