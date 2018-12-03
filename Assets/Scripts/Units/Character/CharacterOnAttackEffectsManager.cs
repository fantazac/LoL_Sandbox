using UnityEngine;

public class CharacterOnAttackEffectsManager : MonoBehaviour
{
    public delegate void OnApplyOnAttackEffectsHandler(Unit unitHit);
    public event OnApplyOnAttackEffectsHandler OnApplyOnAttackEffects;

    public void ApplyOnAttackEffectsToUnitHit(Unit unitHit)
    {
        if (OnApplyOnAttackEffects != null)
        {
            OnApplyOnAttackEffects(unitHit);
        }
    }
}
