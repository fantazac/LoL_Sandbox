using UnityEngine;

public class CharacterOnHitEffectsManager : MonoBehaviour
{
    private Character character;
    private LifeSteal lifeSteal;

    public delegate void OnApplyOnHitEffectsHandler(Entity entityHit, float damage);
    public event OnApplyOnHitEffectsHandler OnApplyOnHitEffects;

    private void Start()
    {
        character = GetComponent<Character>();
        lifeSteal = character.EntityStats.LifeSteal;
    }

    public void ApplyOnHitEffectsToEntityHit(Entity entityHit, float damage)
    {
        if (OnApplyOnHitEffects != null)
        {
            OnApplyOnHitEffects(entityHit, damage);
        }
        character.EntityStats.Health.Restore(damage * 0.01f * lifeSteal.GetTotal());
    }
}
