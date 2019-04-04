public class GrievousWoundsDebuff : AbilityBuff
{
    protected GrievousWoundsDebuff()
    {
        buffName = "Grievous Wounds";

        isADebuff = true;

        buffPercentValue = 40;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/_Character/GrievousWoundsDebuff";
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.GrievousWounds.AddGrievousWoundsSource();
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.GrievousWounds.RemoveGrievousWoundsSource();
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, buffPercentValue);
    }
}
