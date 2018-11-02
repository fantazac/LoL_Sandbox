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

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatsManager.MovementSpeed.AddFlatBonus(buffFlatValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatsManager.MovementSpeed.RemoveFlatBonus(buffFlatValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffFlatValue, buffDuration);
    }
}
