﻿using UnityEngine;

public class CharacterOnHitEffectsManager : MonoBehaviour
{
    private Character character;
    private PercentBonusOnlyStat lifeSteal;

    public delegate void OnApplyOnHitEffectsHandler(Entity entityHit, float damage);
    public event OnApplyOnHitEffectsHandler OnApplyOnHitEffects;

    private void Start()
    {
        character = GetComponent<Character>();
        lifeSteal = character.StatsManager.LifeSteal;
    }

    public void ApplyOnHitEffectsToEntityHit(Entity entityHit, float damage)
    {
        if (OnApplyOnHitEffects != null)
        {
            OnApplyOnHitEffects(entityHit, damage);
        }
        character.StatsManager.RestoreHealth(damage * lifeSteal.GetTotal());
    }
}
