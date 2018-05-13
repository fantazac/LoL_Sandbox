using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_Q : UnitTargetedAoE
{
    protected float durationAoE;

    protected Lucian_Q()
    {
        abilityName = "Piercing Light";

        abilityType = AbilityType.AreaOfEffect;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 5;

        range = 500;
        damage = 85;// 85/120/155/190/225
        damagePerLevel = 35;
        bonusADScaling = 0.6f;// 60/70/80/90/100 %
        bonusADScalingPerLevel = 0.1f;
        resourceCost = 50;// 50/60/70/80/90
        resourceCostPerLevel = 10;
        baseCooldown = 9;// 9/8/7/6/5
        baseCooldownPerLevel = -1;
        castTime = 0.4f;
        delayCastTime = new WaitForSeconds(castTime);

        durationAoE = 0.15f;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianQ";

        areaOfEffectPrefabPath = "CharacterAbilitiesPrefabs/Lucian/LucianQ";
    }

    protected override void Start()
    {
        AbilitiesToDisableWhileActive = new Ability[] { this, GetComponent<Lucian_E>() };

        base.Start();

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override void OnCharacterLevelUp(int level)
    {
        castTime = (0.409f - (0.009f * level));
        delayCastTime = new WaitForSeconds(castTime);
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        Quaternion currentRotation = transform.rotation;
        transform.LookAt(destinationOnCast);
        SetPositionAndRotationOnCast(transform.position + (transform.forward * areaOfEffectPrefab.transform.localScale.z * 0.5f));
        transform.rotation = currentRotation;
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        character.CharacterOrientation.RotateCharacterInstantly(destinationOnCast);

        AreaOfEffect aoe = (Instantiate(areaOfEffectPrefab, positionOnCast, rotationOnCast)).GetComponent<AreaOfEffect>();
        aoe.CreateAreaOfEffect(new List<Entity>(), character.Team, affectedUnitType, durationAoE);
        aoe.ActivateAreaOfEffect();
        aoe.OnAbilityEffectHit += OnAbilityEffectHit;

        FinishAbilityCast();
    }
}
