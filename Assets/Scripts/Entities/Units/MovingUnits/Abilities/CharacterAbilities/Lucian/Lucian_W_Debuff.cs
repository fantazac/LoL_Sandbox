public class Lucian_W_Debuff : AbilityBuff
{
    protected Lucian_W_Debuff()
    {
        buffName = "Ardent Blaze";

        isADebuff = true;

        buffDuration = 6;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianW_Debuff";
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, 0, GetBuffDuration(affectedUnit));
    }
}
