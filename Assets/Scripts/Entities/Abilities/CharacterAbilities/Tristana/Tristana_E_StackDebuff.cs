public class Tristana_E_StackDebuff : AbilityBuff
{
    protected Tristana_E_StackDebuff()
    {
        buffName = "Explosive Charge";

        isADebuff = true;

        buffMaximumStacks = 4;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaE_StackDebuff";
    }

    protected override void SetupBuff(Buff buff, Entity affectedEntity)
    {
        if (buff == null)
        {
            buff = CreateNewBuff(affectedEntity);
            affectedEntity.BuffManager.ApplyBuff(buff, buffSprite, isADebuff);
        }
        else
        {
            buff.IncreaseCurrentStacks();
        }
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, GetBuffDuration(affectedEntity), buffMaximumStacks);
    }
}
