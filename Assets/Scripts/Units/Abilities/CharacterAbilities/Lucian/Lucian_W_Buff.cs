public class Lucian_W_Buff : AbilityBuff
{
    protected Lucian_W_Buff()
    {
        buffName = "Ardent Blaze";

        buffDuration = 1;
        buffFlatValue = 60;// 60/65/70/75/80
        buffFlatValuePerLevel = 5;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianW_Buff";
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.MovementSpeed.AddFlatBonus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.MovementSpeed.RemoveFlatBonus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, buffFlatValue, buffDuration);
    }
}
