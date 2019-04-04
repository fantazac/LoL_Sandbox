using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_Q : UnitTargetedAoE
{
    private readonly float durationAoE;

    protected Lucian_Q()
    {
        abilityName = "Piercing Light";

        abilityType = AbilityType.AREA_OF_EFFECT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 5;

        range = 500;
        damage = 85; // 85/120/155/190/225
        damagePerLevel = 35;
        bonusADScaling = 0.6f; // 60/70/80/90/100 %
        bonusADScalingPerLevel = 0.1f;
        resourceCost = 50; // 50/60/70/80/90
        resourceCostPerLevel = 10;
        baseCooldown = 9; // 9/8/7/6/5
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

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override void Start()
    {
        AbilitiesToDisableWhileActive.Add(this);
        AbilitiesToDisableWhileActive.Add(GetComponent<Lucian_E>());

        base.Start();

        champion.LevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override void OnCharacterLevelUp(int level)
    {
        castTime = 0.409f - (0.009f * level);
        delayCastTime = new WaitForSeconds(castTime);
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        Quaternion currentRotation = transform.rotation;
        transform.LookAt(destinationOnCast);
        SetPositionAndRotationOnCast(transform.position + (transform.forward * areaOfEffectPrefab.transform.GetChild(0).localScale.z * 0.5f));
        transform.rotation = currentRotation;
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        champion.OrientationManager.RotateCharacterInstantly(destinationOnCast);

        AreaOfEffect aoe = (Instantiate(areaOfEffectPrefab, positionOnCast, rotationOnCast)).GetComponent<AreaOfEffect>();
        aoe.CreateAreaOfEffect(affectedTeams, affectedUnitTypes, durationAoE);
        aoe.OnAreaOfEffectHit += OnAreaOfEffectHit;
        aoe.ActivateAreaOfEffect();

        FinishAbilityCast();
    }
}
