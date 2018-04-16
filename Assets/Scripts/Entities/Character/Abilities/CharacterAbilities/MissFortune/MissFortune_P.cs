using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_P : PassiveTargeted, PassiveCharacterAbility
{
    private Entity lastEntityHit;

    public delegate void OnPassiveHitHandler();
    public event OnPassiveHitHandler OnPassiveHit;

    protected MissFortune_P()
    {
        abilityName = "Love Tap";

        abilityType = AbilityType.Passive;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
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

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<MissFortune_P_Buff>() };

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
        character.EntityBasicAttack.OnBasicAttackHit += OnBasicAttackHit;
    }

    public override void OnCharacterLevelUp(int level)
    {
        if (level == 4 || level == 7 || level == 9 || level == 11 || level == 13)
        {
            LevelUp();
        }
    }

    private void OnBasicAttackHit(Entity entityHit)
    {
        if (entityHit != lastEntityHit)
        {
            AbilityBuffs[0].ConsumeBuff(lastEntityHit);
            AbilityBuffs[0].AddNewBuffToEntityHit(entityHit);

            lastEntityHit = entityHit;

            //if (entityHit is Minion)
            //{
            //    entityHit.EntityStats.Health.Reduce(ApplyResistanceToDamage(entityHit, character.EntityStats.AttackDamage.GetTotal() * totalADScaling * 0.5f));
            //}
            //else
            //{
            entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
            //} 

            if (OnPassiveHit != null)
            {
                OnPassiveHit();
            }
        }
    }

    private void OnDestroy()
    {
        if (lastEntityHit != null)
        {
            AbilityBuffs[0].ConsumeBuff(lastEntityHit);
        }
    }
}
