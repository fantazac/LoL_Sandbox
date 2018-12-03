public class Varus_P_Buff : AbilityBuff
{
    private Varus_P varusP;

    private float bonusAttackSpeedScaling;

    protected Varus_P_Buff()
    {
        buffName = "Living Vengeance";

        buffDuration = 5;
        buffPercentValue = 20;
        bonusAttackSpeedScaling = 15;
    }

    protected override void Start()
    {
        varusP = GetComponent<Varus_P>();
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusP_Buff";
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
        return new Buff(this, affectedUnit, buffPercentValue, varusP.GetDurationForNextBuff());
    }
}
