using System;
using System.Collections.Generic;
using UnityEngine;

public class CC_Q : SelfTargeted
{
    protected CC_Q()
    {
        abilityName = "CC BTW";

        abilityType = AbilityType.PASSIVE;
        affectedTeams = AffectedTeams.GetEnemyTeams(champion.Team);
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        damageType = DamageType.PHYSICAL;
        effectType = AbilityEffectType.SINGLE_TARGET;

        MaxLevel = 1;

        damage = 500;
        baseCooldown = 5.5f;
        resourceCost = 0;

        ResetBasicAttackCycleOnAbilityCast = true;

        affectedByCooldownReduction = true;
    }

    protected override void Awake()
    {
        base.Awake();

        GetComponent<CCBasicAttack>().SetBasicAttackEmpoweringAbility(this);
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/CC/CCQ";
    }

    protected override void Start()
    {
        AbilitiesToDisableWhileActive.Add(this);

        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<CC_Q_Buff>() };
        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<CC_Q_Debuff>() };

        AbilityBuffs[0].OnAbilityBuffRemoved += RemoveBuffFromAffectedUnit;
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        UseResource();
        AddNewBuffToAffectedUnit(champion);
    }

    private void AddNewBuffToAffectedUnit(Unit affectedUnit)
    {
        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);
    }

    private void RemoveBuffFromAffectedUnit(Unit affectedUnit)
    {
        FinishAbilityCast();
    }

    public override void OnEmpoweredBasicAttackHit(Unit unitHit, bool isACriticalStrike)
    {
        AbilityBuffs[0].ConsumeBuff(champion);

        DamageUnit(unitHit, GetAbilityDamage(unitHit));
        AbilityDebuffs[0].AddNewBuffToAffectedUnit(unitHit);
    }
}
