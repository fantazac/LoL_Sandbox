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
        FinalAdjustments(destination);
        UseResource();

        SetupDash();
        character.EntityDisplacementManager.OnDisplacementFinished += ApplyDamageAndSlowToAllEnemiesInRadius;

        FinishAbilityCast();
    }

    private void ApplyDamageAndSlowToAllEnemiesInRadius()
    {
        Entity tempEntity;

        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, effectRadius))
        {
            tempEntity = collider.GetComponentInParent<Entity>();
            if (tempEntity != null && TargetIsValid.CheckIfTargetIsValid(tempEntity, affectedUnitType, character.Team))
            {
                float damage = GetAbilityDamage(tempEntity);
                tempEntity.EntityStatsManager.ReduceHealth(damageType, damage);
                AbilityHit(tempEntity, damage);
                AbilityDebuffs[0].AddNewBuffToAffectedEntity(tempEntity);
            }
        }
    }
}
