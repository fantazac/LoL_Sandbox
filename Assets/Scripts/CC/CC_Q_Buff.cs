public class CC_Q_Buff : AbilityBuff
{
    protected CC_Q_Buff()
    {
        buffName = "CC BTW";

        buffDuration = 5;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/CC/CCQ_Buff";
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, 0, buffDuration);
    }
}
