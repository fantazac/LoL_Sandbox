using UnityEngine;

public class Heal : SelfTargeted
{
    private const float MOUSE_RADIUS = 3;

    protected Heal()
    {
        abilityName = "Heal";

        abilityType = AbilityType.HEAL;
        affectedUnitType = AbilityAffectedUnitType.ALLY_CHARACTERS;
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
            float tempDistance;

            Vector3 mouseGroundPosition = Vector3.right * hit.point.x + Vector3.forward * hit.point.z;
            foreach (Collider collider in Physics.OverlapCapsule(mouseGroundPosition, mouseGroundPosition + Vector3.up * 5, MOUSE_RADIUS))
            {
                tempCharacter = collider.GetComponent<Character>();
                if (tempCharacter != null && tempCharacter != champion && TargetIsValid.CheckIfTargetIsValid(tempCharacter, affectedUnitType, champion.Team))
                {
                    tempDistance = Vector3.Distance(transform.position, tempCharacter.transform.position);
                    if (tempDistance < distance && tempDistance < range)
                    {
                        distance = tempDistance;
                        targetedCharacter = tempCharacter;
                    }
                }
            }
        }

        if (targetedCharacter == null)
        {
            float lowestHealth = float.MaxValue;
            float tempLowestHealth;

            Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
            foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, range))
            {
                tempCharacter = collider.GetComponent<Character>();
                if (tempCharacter != null && tempCharacter != champion && TargetIsValid.CheckIfTargetIsValid(tempCharacter, affectedUnitType, champion.Team))
                {
                    tempLowestHealth = tempCharacter.StatsManager.Health.GetCurrentValue();
                    if (tempLowestHealth < lowestHealth)
                    {
                        lowestHealth = tempLowestHealth;
                        targetedCharacter = tempCharacter;
                    }
                }
            }
        }

        if (targetedCharacter != null)
        {
            AbilityBuffs[0].AddNewBuffToAffectedUnit(targetedCharacter);
            AbilityDebuffs[0].AddNewBuffToAffectedUnit(targetedCharacter);
        }

        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);
        AbilityDebuffs[0].AddNewBuffToAffectedUnit(champion);

        FinishAbilityCast();
    }
}
