public class Tristana_Q_Buff : AbilityBuff
{
    protected Tristana_Q_Buff()
    {
        buffName = "Rapid Fire";

        buffDuration = 7;
        buffPercentValue = 50;// 50/65/80/95/110
        buffPercentValuePerLevel = 15;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaQ_Buff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.StatsManager.AttackSpeed.AddPercentBonus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.StatsManager.AttackSpeed.RemovePercentBonus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, buffDuration);
    }
}
