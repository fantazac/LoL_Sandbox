public class Varus_R_TetherDebuff : AbilityBuff
{
    protected Varus_R_TetherDebuff()
    {
        buffName = "Chain of Corruption";

        isADebuff = true;

        buffDuration = 2;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusR_TetherDebuff";
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, GetBuffDuration(affectedEntity));
    }
}
