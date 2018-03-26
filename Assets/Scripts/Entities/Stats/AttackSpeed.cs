using UnityEngine;

public class AttackSpeed : Stat
{
    private EntityBasicAttack entityBasicAttack;

    private const float ATTACK_SPEED_CAP = 2.5f;

    public void SetEntityBasicAttack()
    {
        entityBasicAttack = GetComponent<EntityBasicAttack>();
    }

    public override void OnLevelUp(int level)
    {
        AddPercentBonus((BASE_PERCENTAGE_ON_LEVEL_UP + (ADDITIVE_PERCENTAGE_PER_LEVEL * level)) * perLevelValue);
    }

    public override void UpdateTotal()
    {
        total = Mathf.Clamp((baseValue + flatBonus) * (1 + (percentBonus * 0.01f)) * (1 - (percentMalus * 0.01f)) - flatMalus, 0, ATTACK_SPEED_CAP);
        entityBasicAttack.ChangeAttackSpeedCycleDuration(total);
    }

    public override string GetUIText()
    {
        return "ATTACK SPEED: " + GetTotal().ToString("0.00") + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
