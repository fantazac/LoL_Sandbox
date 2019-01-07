using System;
using System.Collections.Generic;

public class MissFortune_P : PassiveTargeted
{
    private Unit lastUnitHit;

    public delegate void OnPassiveHitHandler();
    public event OnPassiveHitHandler OnPassiveHit;

    protected MissFortune_P()
    {
        abilityName = "Love Tap";

        abilityType = AbilityType.PASSIVE;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        damageType = DamageType.PHYSICAL;
        effectType = AbilityEffectType.SINGLE_TARGET;

        MaxLevel = 6;
        AbilityLevel = 1;

        totalADScaling = 0.5f;
        totalADScalingPerLevel = 0.1f;

        IsEnabled = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneP";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override void Start()
    {
        base.Start();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<MissFortune_P_Debuff>() };

        champion.LevelManager.OnLevelUp += OnCharacterLevelUp;
        champion.OnHitEffectsManager.OnApplyOnHitEffects += SetPassiveEffectOnUnitHit;

        lastUnitHit = champion;
    }

    public override void OnCharacterLevelUp(int level)
    {
        if (level == 4 || level == 7 || level == 9 || level == 11 || level == 13)
        {
            LevelUp();
        }
    }

    private void SetPassiveEffectOnUnitHit(Unit unitHit, float damage)
    {
        if (unitHit != lastUnitHit)
        {
            AbilityDebuffs[0].ConsumeBuff(lastUnitHit);
            AbilityDebuffs[0].AddNewBuffToAffectedUnit(unitHit);

            lastUnitHit = unitHit;

            float passiveDamage = GetAbilityDamage(unitHit);
            //if (unitHit is Minion)
            //{
            //    unitHit.Stats.Health.Reduce(ApplyResistanceToDamage(unitHit, character.Stats.AttackDamage.GetTotal() * totalADScaling * 0.5f));
            //}
            //else
            //{
            DamageUnit(unitHit, passiveDamage);
            //} 
            champion.StatsManager.RestoreHealth(passiveDamage * champion.StatsManager.LifeSteal.GetTotal());

            if (OnPassiveHit != null)
            {
                OnPassiveHit();
            }
        }
    }

    private void OnDestroy()
    {
        if (lastUnitHit != null)
        {
            AbilityDebuffs[0].ConsumeBuff(lastUnitHit);
        }
    }
}
