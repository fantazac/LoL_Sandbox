using UnityEngine;

public class CharacterAbilityEffectsManager : MonoBehaviour
{
    private Character character;
    private PercentBonusOnlyStat spellVamp;

    public delegate void OnApplyAbilityEffectsHandler(Entity entityHit, float damage);
    public event OnApplyAbilityEffectsHandler OnApplyAbilityEffects;

    private void Start()
    {
        character = GetComponent<Character>();
        spellVamp = character.StatsManager.SpellVamp;
    }

    public void ApplyAbilityEffectsToEntityHit(Entity entityHit, float damage)
    {
        if (OnApplyAbilityEffects != null)
        {
            OnApplyAbilityEffects(entityHit, damage);
        }
        character.StatsManager.RestoreHealth(damage * spellVamp.GetTotal());
    }
}
