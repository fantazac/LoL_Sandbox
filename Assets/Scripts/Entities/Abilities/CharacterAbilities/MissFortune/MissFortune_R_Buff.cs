public class MissFortune_R_Buff : AbilityBuff
{
    protected MissFortune_R_Buff()
    {
        buffName = "Bullet Time";

        buffDuration = 3;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneR_Buff";
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, buffDuration);
    }
}
