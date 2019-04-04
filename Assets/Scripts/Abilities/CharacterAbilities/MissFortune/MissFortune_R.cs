using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MissFortune_R : DirectionTargetedProjectile
{
    private int amountOfWavesToShoot;
    private readonly int amountOfWavesToShootPerLevel;

    private WaitForSeconds delayBetweenWaves;

    protected MissFortune_R()
    {
        abilityName = "Bullet Time";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 3;

        range = 1400;
        speed = 2000;
        totalADScaling = 0.75f; // 75%
        totalAPScaling = 0.2f; // 20%
        resourceCost = 100; // 100
        baseCooldown = 120; // 120/110/100
        baseCooldownPerLevel = -10;
        channelTime = 3;
        delayChannelTime = new WaitForSeconds(channelTime);

        amountOfWavesToShoot = 12; // 12/14/16
        amountOfWavesToShootPerLevel = 2;
        delayBetweenWaves = new WaitForSeconds(channelTime / amountOfWavesToShoot - 0.015f);

        CanMoveWhileChanneling = true;
        CanUseAnyAbilityWhileChanneling = true;

        IsAnUltimateAbility = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneR";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/MissFortune/MissFortuneR";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<MissFortune_R_Buff>() };

        AbilityBuffs[0].OnAbilityBuffRemoved += RemoveBuffFromAffectedUnit;
    }

    public override void LevelUpExtraStats()
    {
        amountOfWavesToShoot += amountOfWavesToShootPerLevel;
        delayBetweenWaves = new WaitForSeconds(channelTime / amountOfWavesToShoot - 0.015f);
    }

    protected override void RotationOnAbilityCast(Vector3 destination)
    {
        champion.OrientationManager.RotateCharacterInstantly(destination);
    }

    protected override IEnumerator AbilityWithChannelTime()
    {
        UseResource();
        champion.MovementManager.StopMovement();
        AddNewBuffToAffectedUnit(champion);
        IsBeingChanneled = true;

        for (int i = 0; i < amountOfWavesToShoot; i++)
        {
            yield return delayBetweenWaves;

            ShootWave();
        }

        IsBeingChanneled = false;
        FinishAbilityCast();
    }

    private void AddNewBuffToAffectedUnit(Unit affectedUnit)
    {
        champion.AbilityManager.OnAnAbilityUsed += CancelMissFortuneR;
        champion.ChampionMovementManager.ChampionMoved += CancelMissFortuneR;
        //TODO: if hard cc'd, cancel as well
        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);
    }

    private void RemoveBuffFromAffectedUnit(Unit unitHit)
    {
        champion.AbilityManager.OnAnAbilityUsed -= CancelMissFortuneR;
        champion.ChampionMovementManager.ChampionMoved -= CancelMissFortuneR;
        //TODO: remove cc cancel
    }

    private void CancelMissFortuneR()
    {
        IsBeingChanneled = false;
        CancelAbility();
        AbilityBuffs[0].ConsumeBuff(champion);
    }

    private void ShootWave()
    {
        SpawnProjectile(transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.5f), transform.rotation);
    }
}
