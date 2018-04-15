using UnityEngine;

public class Heal : SelfTargeted, SummonerAbility
{
    private const float MOUSE_RADIUS = 3;

    protected Heal()
    {
        abilityName = "Heal";

        abilityType = AbilityType.Heal;
        affectedUnitType = AbilityAffectedUnitType.ALLY_CHARACTERS;
        effectType = AbilityEffectType.HEALING;

        range = 850;
        baseCooldown = 240;

        startCooldownOnAbilityCast = true;
        CanBeCastDuringOtherAbilityCastTimes = true;

        IsEnabled = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Heal";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { GetComponent<Heal_Buff>() };
        AbilityDebuffs = new AbilityBuff[] { GetComponent<Heal_Debuff>() };

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
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
        return hit.point + character.CharacterMovement.CharacterHeightOffset;
    }

    public override void OnCharacterLevelUp(int level)
    {
        LevelUp();
    }

    public override void UseAbility(Vector3 destination)
    {
        Character targetedCharacter = null;
        Character tempCharacter;

        if (hit.point != Vector3.down)
        {
            float distance = float.MaxValue;
            float tempDistance;

            foreach (Collider collider in Physics.OverlapSphere(hit.point, MOUSE_RADIUS))
            {
                tempCharacter = collider.GetComponent<Character>();
                if (tempCharacter != null && tempCharacter != character && TargetIsValid.CheckIfTargetIsValid(tempCharacter, affectedUnitType, character.Team))
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

            foreach (Collider collider in Physics.OverlapSphere(transform.position, range))
            {
                tempCharacter = collider.GetComponent<Character>();
                if (tempCharacter != null && tempCharacter != character && TargetIsValid.CheckIfTargetIsValid(tempCharacter, affectedUnitType, character.Team))
                {
                    tempLowestHealth = tempCharacter.EntityStats.Health.GetCurrentValue();
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
            AbilityBuffs[0].AddNewBuffToEntityHit(targetedCharacter);
            AbilityDebuffs[0].AddNewDebuffToEntityHit(targetedCharacter);
        }

        AbilityBuffs[0].AddNewBuffToEntityHit(character);
        AbilityDebuffs[0].AddNewDebuffToEntityHit(character);

        StartCooldown(startCooldownOnAbilityCast);
    }
}
