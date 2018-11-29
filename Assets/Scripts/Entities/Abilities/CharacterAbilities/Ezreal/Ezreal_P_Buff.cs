public class Ezreal_P_Buff : AbilityBuff
{
    protected Ezreal_P_Buff()
    {
        buffName = "Rising Spell Force";

        buffDuration = 6;
        buffMaximumStacks = 5;
        buffPercentValue = 10;
        //buffPercentValuePerLevel = 2;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealP_Buff";
    }

    /*public override void UpdateBuffOnAffectedEntities(float oldFlatValue, float newFlatValue, float oldPercentValue, float newPercentValue)
    {
        foreach (Entity affectedEntity in EntitiesAffectedByBuff)
        {
            Buff buff = affectedEntity.EntityBuffManager.GetBuff(this);
            if (buff != null)
            {
                int currentStacks = buff.CurrentStacks;
                affectedEntity.EntityStats.AttackSpeed.RemovePercentBonus(oldPercentValue * currentStacks);
                affectedEntity.EntityStats.AttackSpeed.AddPercentBonus(newPercentValue * currentStacks);
                buff.SetBuffValue(newPercentValue);
            }
        }
    }*/

    protected override void ApplyBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.EntityStatsManager.AttackSpeed.AddPercentBonus(buff.BuffValue * buff.CurrentStacks);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, Buff buff)
    {
        affectedEntity.EntityStatsManager.AttackSpeed.RemovePercentBonus(buff.BuffValue * buff.CurrentStacks);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, buffPercentValue, buffDuration, buffMaximumStacks);
    }
}
