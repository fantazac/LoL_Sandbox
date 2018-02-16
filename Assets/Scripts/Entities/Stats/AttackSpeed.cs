public class AttackSpeed : Stat
{
    private EntityBasicAttack entityBasicAttack;

    public void SetEntityBasicAttack()
    {
        entityBasicAttack = GetComponent<EntityBasicAttack>();
    }

    public override void UpdateTotal()
    {
        total = (baseValue + flatBonus) * (1 + (percentBonus * 0.01f)) * (1 - (percentMalus * 0.01f)) - flatMalus;
        entityBasicAttack.ChangeAttackSpeedCycleDuration(total);
    }

    public override string GetUIText()
    {
        return "ATTACK SPEED: " + GetTotal().ToString("0.00") + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
