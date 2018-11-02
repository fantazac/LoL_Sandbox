public class Tristana_W_Debuff : AbilityBuff
{
    protected Tristana_W_Debuff()
    {
        buffName = "Rocket Jump";

        isADebuff = true;

        buffDuration = 1;// 1/1.5/2/2.5/3
        buffDurationPerLevel = 0.5f;
        buffPercentValue = 60;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaW_Debuff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatsManager.MovementSpeed.AddPercentMalus(buffPercentValue);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatsManager.MovementSpeed.RemovePercentMalus(buffPercentValue);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, GetBuffDuration(affectedEntity));
    }
}
