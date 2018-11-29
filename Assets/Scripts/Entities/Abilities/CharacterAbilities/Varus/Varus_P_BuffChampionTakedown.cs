public class Varus_P_BuffChampionTakedown : AbilityBuff
{
    private float bonusAttackSpeedScaling;

    protected Varus_P_BuffChampionTakedown()
    {
        buffName = "Living Vengeance";

        buffDuration = 5;
        buffPercentValue = 40;
        bonusAttackSpeedScaling = 30;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusP_BuffChampionTakedown";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, Buff buff)
    {
        buff.SetBuffValue(buff.BuffValue + (affectedEntity.StatsManager.AttackSpeed.GetAttackSpeedBonus() * bonusAttackSpeedScaling));
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
