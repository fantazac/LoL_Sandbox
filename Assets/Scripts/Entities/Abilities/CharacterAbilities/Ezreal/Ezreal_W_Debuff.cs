public class Ezreal_W_Debuff : AbilityBuff
{
    protected Ezreal_W_Debuff()
    {
        buffName = "Essence Flux";

        isADebuff = true;

        buffDuration = 4;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealW_Debuff";
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, GetBuffDuration(affectedEntity));
    }
}
