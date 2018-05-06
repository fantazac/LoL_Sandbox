using UnityEngine;

public class Heal : SelfTargeted
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

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Heal";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Heal_Buff>() };
        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Heal_Debuff>() };

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

        //TODO: Extract this into a class/method called 

        if (hit.point != Vector3.down)
        {
            float distance = float.MaxValue;
            float tempDistance;

            Vector3 mouseGroundPosition = Vector3.right * hit.point.x + Vector3.forward * hit.point.z;
            foreach (Collider collider in Physics.OverlapCapsule(mouseGroundPosition, mouseGroundPosition + Vector3.up * 5, MOUSE_RADIUS))
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

            Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
            foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, range))
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
            AbilityBuffs[0].AddNewBuffToAffectedEntity(targetedCharacter);
            AbilityDebuffs[0].AddNewBuffToAffectedEntity(targetedCharacter);
        }

        AbilityBuffs[0].AddNewBuffToAffectedEntity(character);
        AbilityDebuffs[0].AddNewBuffToAffectedEntity(character);

        StartCooldown(startCooldownOnAbilityCast);
    }
}
