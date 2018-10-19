public class Tristana_E_Debuff : AbilityBuff
{
    protected Tristana_E_Debuff()
    {
        buffName = "Explosive Charge";

        isADebuff = true;

        buffDuration = 4;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaE_Debuff";
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, buffDuration);
    }
}
