public class Varus_W_Debuff : AbilityBuff
{
    protected Varus_W_Debuff()
    {
        buffName = "Blight";

        isADebuff = true;

        buffDuration = 6;
        buffMaximumStacks = 3;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusW_Debuff";
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, 0, buffDuration, buffMaximumStacks);
    }
}
