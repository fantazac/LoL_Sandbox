public class Old_Ezreal_W_Buff : AbilityBuff
{
    protected Old_Ezreal_W_Buff()
    {
        buffName = "Essence Flux";

        buffDuration = 5;
        buffPercentValue = 20;
        buffPercentValuePerLevel = 5;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealW_Buff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.EntityStatsManager.AttackSpeed.AddPercentBonus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.EntityStatsManager.AttackSpeed.RemovePercentBonus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, buffDuration, buffMaximumStacks);
    }
}
