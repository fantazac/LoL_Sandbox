using UnityEngine;

public class Varus_P : PassiveTargeted
{
    private float buffDuration;
    private float durationForNextBuff;

    protected Varus_P()
    {
        abilityName = "Living Vengeance";

        abilityType = AbilityType.PASSIVE;

        AbilityLevel = 1;

        buffDuration = 5;

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusP";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Varus_P_Buff>(), gameObject.AddComponent<Varus_P_BuffChampionTakedown>() };

        AbilityBuffs[1].OnAbilityBuffRemoved += RemoveBuffFromAffectedUnit;

        foreach (DamageSource damageSource in character.CharacterAbilityManager.CharacterAbilities)
        {
            damageSource.OnKilledUnit += OnUnitKilled;
        }
        foreach (DamageSource damageSource in character.CharacterAbilityManager.SummonerAbilities)
        {
            damageSource.OnKilledUnit += OnUnitKilled;
        }
        character.BasicAttack.OnKilledUnit += OnUnitKilled;
    }

    private void OnUnitKilled(DamageSource damageSource, Unit killedUnit)
    {
        if (!(killedUnit is Character))
        {
            durationForNextBuff = 0;
            AbilityBuffs[0].ConsumeBuff(character);
            AbilityBuffs[1].AddNewBuffToAffectedUnit(character);
        }
        else
        {
            Buff buff = character.BuffManager.GetBuff(AbilityBuffs[1]);
            if (buff != null)
            {
                durationForNextBuff = buffDuration - buff.DurationRemaining;
            }
            else
            {
                durationForNextBuff = buffDuration;
                AbilityBuffs[0].ConsumeBuff(character);
                AbilityBuffs[0].AddNewBuffToAffectedUnit(character);
            }
        }
    }

    private void RemoveBuffFromAffectedUnit(Unit affectedUnit)
    {
        if (durationForNextBuff > 0)
        {
            AbilityBuffs[0].AddNewBuffToAffectedUnit(character);
        }
    }

    public float GetDurationForNextBuff()
    {
        return durationForNextBuff;
    }
}
