public class MissFortune_W_PassiveBuff : AbilityBuff
{
    private float baseBuffFlatBonus;

    protected MissFortune_W_PassiveBuff()
    {
        buffName = "Strut";

        showBuffValueOnUI = true;

        baseBuffFlatBonus = 25;
        buffDuration = 5;

        buffFlatValue = 60;
        buffFlatValuePerLevel = 10;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW_PassiveBuff";
    }

    public override void UpdateBuffOnAffectedEntities(float oldFlatValue, float newFlatValue, float oldPercentValue, float newPercentValue)
    {
        foreach (Entity affectedEntity in EntitiesAffectedByBuff)
        {
            BuffUpdatingWithDelay buff = (BuffUpdatingWithDelay)affectedEntity.BuffManager.GetBuff(this);
            if (buff != null)
            {
                if (buff.BuffValue != baseBuffFlatBonus)
                {
                    affectedEntity.StatsManager.MovementSpeed.RemoveFlatBonus(oldFlatValue);
                    affectedEntity.StatsManager.MovementSpeed.AddFlatBonus(newFlatValue);
                    buff.SetBuffValueOnUI(newFlatValue);
                }
                else
                {
                    buff.SetBuffValuePostDelay(newFlatValue);
                }
            }
        }
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
        return new BuffUpdatingWithDelay(this, affectedEntity, baseBuffFlatBonus, buffDuration, buffFlatValue);
    }
}
