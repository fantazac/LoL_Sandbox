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

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatsManager.AttackSpeed.AddPercentBonus(buffValue + (affectedEntity.EntityStatsManager.AttackSpeed.GetAttackSpeedBonus() * bonusAttackSpeedScaling));
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatsManager.AttackSpeed.RemovePercentBonus(buffValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, buffDuration);
    }
}
