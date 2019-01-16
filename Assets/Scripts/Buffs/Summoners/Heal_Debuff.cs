public class Heal_Debuff : AbilityBuff
{
    protected Heal_Debuff()
    {
        buffName = "Heal";

        isADebuff = true;

        buffDuration = 35;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/SummonerAbilities/Heal";
    }

    public override void AddNewBuffToAffectedUnit(Unit affectedUnit)
    {
        SetupBuff(affectedUnit.BuffManager.GetDebuffOfSameType(this), affectedUnit);
    }

    protected override void SetupBuff(Buff buff, Unit affectedUnit)
    {
        if (buff != null)
        {
            Consume(affectedUnit, buff);
        }
        affectedUnit.BuffManager.ApplyBuff(CreateNewBuff(affectedUnit), buffSprite, isADebuff);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, 0, GetBuffDuration(affectedUnit));
    }
}
