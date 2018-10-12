using UnityEngine;

public class CharacterOnAttackEffectsManager : MonoBehaviour
{
    public delegate void OnApplyOnAttackEffectsHandler(Entity entityHit, float damage);
    public event OnApplyOnAttackEffectsHandler OnApplyOnAttackEffects;

    public void ApplyOnAttackEffectsToEntityHit(Entity entityHit, float damage)
    {
        if (OnApplyOnAttackEffects != null)
        {
            OnApplyOnAttackEffects(entityHit, damage);
        }
    }
}
