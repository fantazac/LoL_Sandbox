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

    protected override void SetupBuff(Buff buff, Unit affectedUnit)
    {
        if (buff == null)
        {
            buff = CreateNewBuff(affectedUnit);
            affectedUnit.BuffManager.ApplyBuff(buff, buffSprite, isADebuff);
        }
        else
        {
            buff.IncreaseCurrentStacks();
        }
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, 0, GetBuffDuration(affectedUnit), buffMaximumStacks);
    }
}
