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

    protected override void ApplyBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.StatsManager.MovementSpeed.AddFlatBonus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.StatsManager.MovementSpeed.RemoveFlatBonus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffFlatValue, buffDuration);
    }
}
