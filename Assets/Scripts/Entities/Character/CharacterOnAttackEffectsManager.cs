using UnityEngine;

public class CharacterOnAttackEffectsManager : MonoBehaviour
{
    public delegate void OnApplyOnAttackEffectsHandler(Entity entityHit);
    public event OnApplyOnAttackEffectsHandler OnApplyOnAttackEffects;

    public void ApplyOnAttackEffectsToEntityHit(Entity entityHit)
    {
        if (OnApplyOnAttackEffects != null)
        {
            OnApplyOnAttackEffects(entityHit);
        }
    }
}
