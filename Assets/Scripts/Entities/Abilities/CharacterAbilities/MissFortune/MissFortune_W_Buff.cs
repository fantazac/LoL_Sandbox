public class MissFortune_W_Buff : AbilityBuff
{
    protected MissFortune_W_Buff()
    {
        buffName = "Guns Blazing";

        buffDuration = 4;
        buffPercentValue = 40;
        buffPercentValuePerLevel = 15;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW_Buff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.StatsManager.AttackSpeed.AddPercentBonus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.StatsManager.AttackSpeed.RemovePercentBonus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, buffDuration);
    }
}
