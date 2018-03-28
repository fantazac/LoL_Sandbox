using UnityEngine;

public class Heal : SelfTargeted, SummonerAbility
{
    private const float BASE_HEAL_VALUE = 75;
    private const float HEAL_INCREASE_PER_LEVEL = 15;

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

        buffDuration = 1;
        buffFlatBonus = 90;
        buffPercentBonus = 30;

        debuffDuration = 35;
        debuffPercentBonus = 0.5f;

        IsEnabled = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/SummonerAbilities/Heal";
        buffSpritePath = "Sprites/Characters/SummonerAbilities/Ghost";
        debuffSpritePath = "Sprites/Characters/SummonerAbilities/Heal";
    }

    protected override void Start()
    {
        base.Start();

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
        buffFlatBonus = (BASE_HEAL_VALUE + (HEAL_INCREASE_PER_LEVEL * level));
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
            AddNewBuffToEntityHit(targetedCharacter);
            AddNewDebuffToEntityHit(targetedCharacter);
        }

        AddNewBuffToEntityHit(character);
        AddNewDebuffToEntityHit(character);

        StartCooldown(startCooldownOnAbilityCast);
    }

    protected override void AddNewDebuffToEntityHit(Entity entityHit)
    {
        Buff debuff = entityHit.EntityBuffManager.GetDebuffOfSameType(this);
        if (debuff != null)
        {
            debuff.ConsumeBuff();
        }
        debuff = new Buff(this, entityHit, true, debuffDuration);
        entityHit.EntityBuffManager.ApplyDebuff(debuff, debuffSprite);
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        if (entityHit.EntityBuffManager.GetDebuffOfSameType(this) != null)
        {
            entityHit.EntityStats.Health.Restore(buffFlatBonus * debuffPercentBonus);
        }
        else
        {
            entityHit.EntityStats.Health.Restore(buffFlatBonus);
        }
        entityHit.EntityStats.MovementSpeed.AddPercentBonus(buffPercentBonus);
        EntitiesAffectedByBuff.Add(entityHit);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        entityHit.EntityStats.MovementSpeed.RemovePercentBonus(buffPercentBonus);
        EntitiesAffectedByBuff.Remove(entityHit);
    }
}
