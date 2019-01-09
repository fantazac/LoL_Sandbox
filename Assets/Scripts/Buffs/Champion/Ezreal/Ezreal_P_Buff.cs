public class Ezreal_P_Buff : AbilityBuff
{
    protected Ezreal_P_Buff()
    {
        buffName = "Rising Spell Force";

        buffDuration = 6;
        buffMaximumStacks = 5;
        buffPercentValue = 10;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealP_Buff";
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.AttackSpeed.AddPercentBonus(buff.BuffValue * buff.CurrentStacks);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.AttackSpeed.RemovePercentBonus(buff.BuffValue * buff.CurrentStacks);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, buffPercentValue, buffDuration, buffMaximumStacks);
    }
}
