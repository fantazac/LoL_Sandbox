using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Q : SelfTargeted
{
    protected CC_Q()
    {
        abilityName = "CC BTW";

        abilityType = AbilityType.Passive;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        damageType = DamageType.PHYSICAL;
        effectType = AbilityEffectType.SINGLE_TARGET;

        MaxLevel = 1;

        damage = 500;
        baseCooldown = 5.5f;
        resourceCost = 0;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/CC/CCQ";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<CC_Q_Buff>() };
        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<CC_Q_Debuff>() };

        AbilityBuffs[0].OnAbilityBuffRemoved += RemoveBuffFromEntityHit;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        UseResource();
        AddNewBuffToEntityHit(character);

        FinishAbilityCast();
    }

    private void SetAbilityEffectOnEntityHit(Entity entityHit, float damage)
    {
        RemoveBuffFromEntityHit(character);
        AbilityBuffs[0].ConsumeBuff(character);

        entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        AbilityDebuffs[0].AddNewBuffToAffectedEntity(entityHit);
    }

    private void AddNewBuffToEntityHit(Entity entityHit)
    {
        character.CharacterOnHitEffectsManager.OnApplyOnHitEffects += SetAbilityEffectOnEntityHit;
        AbilityBuffs[0].AddNewBuffToAffectedEntity(character);
    }

    private void RemoveBuffFromEntityHit(Entity entityHit)
    {
        character.CharacterOnHitEffectsManager.OnApplyOnHitEffects -= SetAbilityEffectOnEntityHit;
    }
}
