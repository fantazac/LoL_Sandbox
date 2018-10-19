using UnityEngine;

public class CC_Q : SelfTargeted
{
    protected CC_Q()
    {
        abilityName = "CC BTW";

        abilityType = AbilityType.PASSIVE;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
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
        AbilitiesToDisableWhileActive = new Ability[] { this };

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
    }

    private void AddNewBuffToEntityHit(Entity entityHit)
    {
        AbilityBuffs[0].AddNewBuffToAffectedEntity(character);
    }

    private void RemoveBuffFromEntityHit(Entity entityHit)
    {
        FinishAbilityCast();
    }

    public override void OnEmpoweredBasicAttackHit(Entity entityHit, bool isACriticalStrike)
    {
        AbilityBuffs[0].ConsumeBuff(character);

        entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        AbilityDebuffs[0].AddNewBuffToAffectedEntity(entityHit);
    }
}
