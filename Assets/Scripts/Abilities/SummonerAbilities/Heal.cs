using System;
using System.Collections.Generic;
using UnityEngine;

public class Heal : GroundTargeted
{
    private const float MOUSE_RADIUS = 3;

    protected Heal()
    {
        abilityName = "Heal";

        abilityType = AbilityType.HEAL;
        affectedUnitTypes = new List<Type>() { typeof(Character) };
        effectType = AbilityEffectType.HEALING;

        range = 850;
        baseCooldown = 240;

        CanBeCastDuringOtherAbilityCastTimes = true;

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Heal";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetAllyTeam(allyTeam);
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Heal_Buff>() };
        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Heal_Debuff>() };

        champion.LevelManager.OnLevelUp += OnCharacterLevelUp;
    }

    public override bool CanBeCast(Vector3 mousePosition)
    {
        if (!MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit))
        {
            hit.point = Vector3.down;
        }

        return true;
    }

    public override Vector3 GetDestination()
    {
        return hit.point + champion.CharacterHeightOffset;
    }

    public override void OnCharacterLevelUp(int level)
    {
        LevelUpBuffsAndDebuffs();
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        Character targetedCharacter = null;
        Character tempCharacter;

        //TODO: Extract this into a class/method 

        if (hit.point != Vector3.down)
        {
            float distance = float.MaxValue;

            Vector3 mouseGroundPosition = Vector3.right * hit.point.x + Vector3.forward * hit.point.z;
            foreach (Collider other in Physics.OverlapCapsule(mouseGroundPosition, mouseGroundPosition + Vector3.up * 5, MOUSE_RADIUS))
            {
                tempCharacter = other.GetComponentInParent<Character>();

                if (!tempCharacter || tempCharacter == champion || !tempCharacter.IsTargetable(affectedUnitTypes, affectedTeams)) continue;

                float tempDistance = Vector3.Distance(transform.position, tempCharacter.transform.position);

                if (tempDistance >= distance || tempDistance >= range) continue;

                distance = tempDistance;
                targetedCharacter = tempCharacter;
            }
        }

        if (!targetedCharacter)
        {
            float lowestHealth = float.MaxValue;

            Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
            foreach (Collider other in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, range))
            {
                tempCharacter = other.GetComponentInParent<Character>();

                if (!tempCharacter || tempCharacter == champion || !tempCharacter.IsTargetable(affectedUnitTypes, affectedTeams)) continue;

                float tempLowestHealth = tempCharacter.StatsManager.Health.GetCurrentValue();

                if (tempLowestHealth >= lowestHealth) continue;

                lowestHealth = tempLowestHealth;
                targetedCharacter = tempCharacter;
            }
        }

        if (targetedCharacter)
        {
            AbilityBuffs[0].AddNewBuffToAffectedUnit(targetedCharacter);
            AbilityDebuffs[0].AddNewBuffToAffectedUnit(targetedCharacter);
        }

        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);
        AbilityDebuffs[0].AddNewBuffToAffectedUnit(champion);

        FinishAbilityCast();
    }
}
