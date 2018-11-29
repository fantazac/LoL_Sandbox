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

    protected override void ApplyBuffEffect(Entity affectedEntity, Buff buff)
    {
        buff.SetBuffValue(buff.BuffValue + (affectedEntity.EntityStatsManager.AttackSpeed.GetAttackSpeedBonus() * bonusAttackSpeedScaling));
        affectedEntity.EntityStatsManager.AttackSpeed.AddPercentBonus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.EntityStatsManager.AttackSpeed.RemovePercentBonus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, varusP.GetDurationForNextBuff());
    }
}
