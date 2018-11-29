public class Varus_P : PassiveTargeted, DamageSourceOnEntityKill
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

        AbilityBuffs[1].OnAbilityBuffRemoved += RemoveBuffFromEntity;
    }

    public void KilledEntity(Entity killedEntity)
    {
        if (killedEntity is Character)
        {
            durationForNextBuff = 0;
            AbilityBuffs[0].ConsumeBuff(character);
            AbilityBuffs[1].AddNewBuffToAffectedEntity(character);
        }
        else
        {
            Buff buff = character.EntityBuffManager.GetBuff(AbilityBuffs[1]);
            if (buff != null)
            {
                durationForNextBuff = buffDuration - buff.DurationRemaining;
            }
            else
            {
                durationForNextBuff = buffDuration;
                AbilityBuffs[0].AddNewBuffToAffectedEntity(character);
            }
        }
    }

    private void RemoveBuffFromEntity(Entity entityHit)
    {
        if (durationForNextBuff > 0)
        {
            AbilityBuffs[0].AddNewBuffToAffectedEntity(character);
        }
    }

    public float GetDurationForNextBuff()
    {
        return durationForNextBuff;
    }
}
