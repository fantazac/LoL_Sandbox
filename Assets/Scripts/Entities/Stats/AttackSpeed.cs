using UnityEngine;

public class AttackSpeed : MultiplicativeBonusStat
{
    private static float ATTACK_SPEED_CAP = 2.5f;

    private EntityBasicAttack entityBasicAttack;

    public AttackSpeed(float initialBaseValue) : base(initialBaseValue) { }
    public AttackSpeed(float initialBaseValue, float perLevelValue, float attackDelay) : base(initialBaseValue / (1 + attackDelay), perLevelValue) { }

    public void SetEntityBasicAttack(EntityBasicAttack entityBasicAttack)
    {
        if (entityBasicAttack)
        {
            this.entityBasicAttack = entityBasicAttack;
            entityBasicAttack.ChangeAttackSpeedCycleDuration(total, false);
        }
    }

    public override void UpdateTotal()
    {
        float oldTotal = total;
        total = Mathf.Clamp(initialBaseValue * (1 + (percentBonus * 0.01f)) * (1 - (percentMalus * 0.01f)) * (1 + (multiplicativePercentBonus * 0.01f)), 0, ATTACK_SPEED_CAP);
        if (entityBasicAttack)
        {
            entityBasicAttack.ChangeAttackSpeedCycleDuration(total, oldTotal < total);
        }
    }

    public float GetAttackSpeedBonus()
    {
        if (total - currentBaseValue <= 0)
        {
            return 0;
        }
        return (total / currentBaseValue) - 1;
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
}
