using UnityEngine;

public class AttackSpeed : MultiplicativeBonusStat
{
    private const float ATTACK_SPEED_CAP = 2.5f;

    private BasicAttack basicAttack;

    public AttackSpeed(float initialBaseValue) : base(initialBaseValue) { }
    public AttackSpeed(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    public void SetBasicAttack(BasicAttack basicAttack)
    {
        if (!basicAttack) return;
        
        this.basicAttack = basicAttack;
        basicAttack.ChangeAttackSpeedCycleDuration(total, false);
    }

    protected override void UpdateTotal()
    {
        float oldTotal = total;
        total = Mathf.Clamp(initialBaseValue * (1 + (percentBonus * 0.01f)) * (1 - (percentMalus * 0.01f)) * (1 + (multiplicativePercentBonus * 0.01f)), 0, ATTACK_SPEED_CAP);
        if (basicAttack)
        {
            basicAttack.ChangeAttackSpeedCycleDuration(total, oldTotal < total);
        }
    }

    public float GetAttackSpeedBonus()
    {
        if (total - initialBaseValue <= 0)
        {
            return 0;
        }
        
        return (total / initialBaseValue) - 1;
    }

    public override void OnLevelUp(int level)
    {
        if (level > 1)
        {
            AddPercentBonus(CalculateStatTotalLevelBonus(level));
        }
        else
        {
            base.OnLevelUp(level);
        }
    }

    protected override float CalculateStatTotalLevelBonus(int level)
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
