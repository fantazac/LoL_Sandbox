public class Barrier_Buff : AbilityBuff
{
    protected Barrier_Buff()
    {
        buffName = "Barrier";

        buffDuration = 2;
        buffFlatValue = 115;
        buffFlatValuePerLevel = 20;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/SummonerAbilities/Barrier_Buff";
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.ShieldManager.AddNewShield(ShieldType.NORMAL, this, buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.ShieldManager.RemoveShield(ShieldType.NORMAL, this);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, buffFlatValue, buffDuration);
    }
}
