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

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        buff.SetBuffValue(buff.BuffValue + (affectedUnit.StatsManager.AttackSpeed.GetAttackSpeedBonus() * bonusAttackSpeedScaling));
        affectedUnit.StatsManager.AttackSpeed.AddPercentBonus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.AttackSpeed.RemovePercentBonus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, buffPercentValue, buffDuration);
    }
}
