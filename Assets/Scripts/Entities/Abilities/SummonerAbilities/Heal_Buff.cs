public class Heal_Buff : AbilityBuff
{
    private float ratioIfAffectedByHealDebuff;

    private Heal_Debuff healDebuff;

    protected Heal_Buff()
    {
        buffName = "Heal";

        buffDuration = 1;
        buffFlatValue = 90;
        buffFlatValuePerLevel = 15;
        buffPercentValue = 30;

        ratioIfAffectedByHealDebuff = 0.5f;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/SummonerAbilities/Ghost";
    }

    protected override void Start()
    {
        base.Start();

        healDebuff = GetComponent<Heal_Debuff>();
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        if (affectedEntity.EntityBuffManager.GetDebuffOfSameType(healDebuff) != null)
        {
            affectedEntity.EntityStats.Health.Restore(buffFlatValue * ratioIfAffectedByHealDebuff);
        }
        else
        {
            affectedEntity.EntityStats.Health.Restore(buffFlatValue);
        }
        affectedEntity.EntityStats.MovementSpeed.AddPercentBonus(buffPercentValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStats.MovementSpeed.RemovePercentBonus(buffPercentValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffFlatValue, buffDuration);
    }
}
