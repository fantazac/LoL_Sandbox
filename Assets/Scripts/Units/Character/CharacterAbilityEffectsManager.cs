using UnityEngine;

public class CharacterAbilityEffectsManager : MonoBehaviour
{
    private Character character;
    private PercentBonusOnlyStat spellVamp;

    public delegate void OnApplyAbilityEffectsHandler(Unit unitHit, float damage);
    public event OnApplyAbilityEffectsHandler OnApplyAbilityEffects;

    private void Start()
    {
        character = GetComponent<Character>();
        spellVamp = character.StatsManager.SpellVamp;
    }

    public void ApplyAbilityEffectsToUnitHit(Unit unitHit, float damage)
    {
        if (OnApplyAbilityEffects != null)
        {
            OnApplyAbilityEffects(unitHit, damage);
        }
        character.StatsManager.RestoreHealth(damage * spellVamp.GetTotal());
    }
}
