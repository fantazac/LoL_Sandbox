public class Lucian_P_Buff : AbilityBuff
{
    protected Lucian_P_Buff()
    {
        buffName = "Lightslinger";

        buffDuration = 3;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianP_Buff";
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, 0, buffDuration);
    }
}
