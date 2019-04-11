public class Heal_Buff : AbilityBuff
{
    private readonly float ratioIfAffectedByHealDebuff;

    private Heal_Debuff healDebuff;

    protected Heal_Buff()
    {
        buffName = "Heal";

        buffDuration = 1;
        buffFlatValue = 90;
        buffFlatValuePerLevel = 15;
        buffPercentValue = 30;

        ratioIfAffectedByHealDebuff = 0.5f;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/SummonerAbilities/Heal_Buff";
    }

    protected override void Start()
    {
        base.Start();

        healDebuff = GetComponent<Heal_Debuff>();
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        float healAmount = buffFlatValue * character.StatsManager.HealAndShieldPower.GetTotal();
        if (affectedUnit.BuffManager.IsAffectedByDebuffOfSameType(healDebuff))
        {
            healAmount *= ratioIfAffectedByHealDebuff;
        }

        affectedUnit.StatsManager.RestoreHealth(healAmount);
        affectedUnit.StatsManager.MovementSpeed.AddPercentBonus(buff.BuffValue);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatsManager.MovementSpeed.RemovePercentBonus(buff.BuffValue);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, buffPercentValue, buffDuration);
    }
}
