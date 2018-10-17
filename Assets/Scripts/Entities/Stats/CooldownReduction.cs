using UnityEngine;

public class CooldownReduction : Stat
{
    private const float COOLDOWN_REDUCTION_CAP = 40;

    public delegate void OnCooldownReductionChangedHandler(float cooldownReduction);
    public event OnCooldownReductionChangedHandler OnCooldownReductionChanged;

    public CooldownReduction() : base() { }

    public override void UpdateTotal()
    {
        total = Mathf.Clamp(percentBonus, 0, COOLDOWN_REDUCTION_CAP) * 0.01f;
        if (OnCooldownReductionChanged != null)
        {
            OnCooldownReductionChanged(total);
        }
    }
}
