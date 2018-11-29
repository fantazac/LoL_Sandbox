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

    protected override void ApplyBuffEffect(Entity affectedEntity, Buff buff)
    {
        float healAmount = buffFlatValue * character.EntityStatsManager.HealAndShieldPower.GetTotal();
        if (affectedEntity.EntityBuffManager.IsAffectedByDebuffOfSameType(healDebuff))
        {
            healAmount *= ratioIfAffectedByHealDebuff;
        }
        affectedEntity.EntityStatsManager.RestoreHealth(healAmount);
        affectedEntity.EntityStatsManager.MovementSpeed.AddPercentBonus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.EntityStatsManager.MovementSpeed.RemovePercentBonus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, buffDuration);
    }
}
