using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityEffectsManager : MonoBehaviour
{
    private Character character;
    private SpellVamp spellVamp;

    public delegate void OnApplyAbilityEffectsHandler(Entity entityHit, float damage);
    public event OnApplyAbilityEffectsHandler OnApplyAbilityEffects;

    private void Start()
    {
        character = GetComponent<Character>();
        spellVamp = character.EntityStats.SpellVamp;
    }

    public void ApplyAbilityEffectsToEntityHit(Entity entityHit, float damage)
    {
        if (OnApplyAbilityEffects != null)
        {
            OnApplyAbilityEffects(entityHit, damage);
        }
        character.EntityStats.Health.Restore(damage * 0.01f * spellVamp.GetTotal());
    }
}
