using UnityEngine;

public class CooldownReduction : Stat
{
    private const float COOLDOWN_REDUCTION_CAP = 40;

    public delegate void OnCooldownReductionChangedHandler(float cooldownReduction);
    public event OnCooldownReductionChangedHandler OnCooldownReductionChanged;

    public override void UpdateTotal()
    {
        total = Mathf.Clamp(baseValue + flatBonus - flatMalus, 0, COOLDOWN_REDUCTION_CAP);
        if (OnCooldownReductionChanged != null)
        {
            OnCooldownReductionChanged(total);
        }
    }
}
