public class Recall_Buff : AbilityBuff
{
    protected Recall_Buff()
    {
        buffName = "Recall";
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/_Character/Recall";
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit);
    }
}
