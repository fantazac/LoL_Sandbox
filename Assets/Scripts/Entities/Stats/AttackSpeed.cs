using UnityEngine;

public class AttackSpeed : Stat
{
    private const float ATTACK_SPEED_CAP = 2.5f;

    private EntityBasicAttack entityBasicAttack;
    private float multiplicativePercentBonus;

    public AttackSpeed(float initialBaseValue) : base(initialBaseValue) { }
    public AttackSpeed(float initialBaseValue, float perLevelValue, float attackDelay) : base(initialBaseValue / (1 + attackDelay), perLevelValue) { }

    public void SetEntityBasicAttack(EntityBasicAttack entityBasicAttack)
    {
        if (entityBasicAttack)
        {
            this.entityBasicAttack = entityBasicAttack;
            entityBasicAttack.ChangeAttackSpeedCycleDuration(total);
        }
    }

    public override void UpdateTotal()
    {
        total = Mathf.Clamp(initialBaseValue * (1 + (percentBonus * 0.01f)) * (1 - (percentMalus * 0.01f)) * (1 + (multiplicativePercentBonus * 0.01f)), 0, ATTACK_SPEED_CAP);
        if (entityBasicAttack)
        {
            entityBasicAttack.ChangeAttackSpeedCycleDuration(total);
        }
    }

    public override void OnLevelUp(int level)
    {
        if (level > 1)
        {
            AddPercentBonus(calculateStatTotalLevelBonus(level));
        }
        else
        {
            base.OnLevelUp(level);
        }
    }

    protected override float calculateStatTotalLevelBonus(int level)
    {
        return perLevelValue * (0.65f + 0.035f * level);
    }

    protected override void CalculatePercentMalusIncrease(float percentMalus)
    {
        this.percentMalus = 100 - (100 - this.percentMalus) * (100 - percentMalus) * 0.01f;
    }

    protected override void CalculatePercentMalusReduction(float percentMalus)
    {
        this.percentMalus = 100 - (100 - this.percentMalus) / ((100 - percentMalus) * 0.01f);
    }

    public float GetMultiplicativePercentBonus()
    {
        return multiplicativePercentBonus;
    }

    private void AddMultiplicativePercentBonus(float multiplicativePercentBonus)
    {
        this.multiplicativePercentBonus += multiplicativePercentBonus;
        UpdateTotal();
    }

    private void RemoveMultiplicativePercentBonus(float multiplicativePercentBonus)
    {
        this.multiplicativePercentBonus -= multiplicativePercentBonus;
        UpdateTotal();
    }
}
