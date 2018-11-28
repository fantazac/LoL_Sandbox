﻿public class Varus_P_Buff : AbilityBuff
{
    private Varus_P varusP;

    private float bonusAttackSpeedScaling;

    protected Varus_P_Buff()
    {
        buffName = "Living Vengeance";

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
        return new Buff(this, affectedEntity, buffPercentValue, varusP.GetDurationForNextBuff());
    }
}
