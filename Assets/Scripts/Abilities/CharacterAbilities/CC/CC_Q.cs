using System;
using System.Collections.Generic;

public class CC_Q : SelfTargeted, IBasicAttackEmpoweringAbility
{
    protected CC_Q()
    {
        abilityName = "CC BTW";

        abilityType = AbilityType.PASSIVE;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        damageType = DamageType.PHYSICAL;
        effectType = AbilityEffectType.SINGLE_TARGET;

        MaxLevel = 1;

        damage = 500;
        baseCooldown = 5.5f;
        resourceCost = 0;

        resetBasicAttackCycleOnAbilityCast = true;

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

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override void Start()
    {
        abilitiesToDisableWhileActive.Add(this);

        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<CC_Q_Buff>() };
        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<CC_Q_Debuff>() };

        AbilityBuffs[0].OnAbilityBuffRemoved += RemoveBuffFromAffectedUnit;
    }

    public override void UseAbility()
    {
        StartAbilityCast();

        UseResource();
        AddNewBuffToAffectedUnit();
    }

    private void AddNewBuffToAffectedUnit()
    {
        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);
    }

    private void RemoveBuffFromAffectedUnit(Unit affectedUnit)
    {
        FinishAbilityCast();
    }

    public void OnEmpoweredBasicAttackHit(Unit unitHit, bool isACriticalStrike = false)
    {
        AbilityBuffs[0].ConsumeBuff(champion);

        DamageUnit(unitHit, GetAbilityDamage(unitHit));
        AbilityDebuffs[0].AddNewBuffToAffectedUnit(unitHit);
    }
}
