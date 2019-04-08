using UnityEngine;

public class CooldownReduction : Stat
{
    private const float COOLDOWN_REDUCTION_CAP = 40;

    public delegate void OnCooldownReductionChangedHandler(float cooldownReduction);
    public event OnCooldownReductionChangedHandler OnCooldownReductionChanged;

    protected override void UpdateTotal()
    {
        total = Mathf.Clamp(percentBonus, 0, COOLDOWN_REDUCTION_CAP) * 0.01f;
        OnCooldownReductionChanged?.Invoke(total);
    }
}
