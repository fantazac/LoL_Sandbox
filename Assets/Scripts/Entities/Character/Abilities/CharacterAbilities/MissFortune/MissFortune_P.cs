using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_P : PassiveTargeted, CharacterAbility, PassiveCharacterAbility
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

        totalADScaling = 0.5f;
        totalADScalingPerLevel = 0.1f;

        IsEnabled = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneP";
        buffSpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneP_Buff";
    }

    protected override void Start()
    {
        base.Start();

        character.CharacterLevelManager.OnLevelUp += OnCharacterLevelUp;
        character.EntityBasicAttack.OnBasicAttackHit += OnBasicAttackHit;
    }

    public override void OnCharacterLevelUp(int level)
    {
        if (level == 4 || level == 7 || level == 9 || level == 11 || level == 13)
        {
            LevelUpAbilityStats();
        }
    }

    protected override void AddNewBuffToEntityHit(Entity entityHit)
    {
        Buff buff = entityHit.EntityBuffManager.GetBuff(this);
        if (buff == null)
        {
            buff = new Buff(this, entityHit, false);
            entityHit.EntityBuffManager.ApplyBuff(buff, buffSprite);
        }
    }

    public override void ApplyBuffToEntityHit(Entity entityHit, int currentStacks)
    {
        EntitiesAffectedByBuff.Add(entityHit);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit, int currentStacks)
    {
        EntitiesAffectedByBuff.Remove(entityHit);
    }

    private void OnBasicAttackHit(Entity entityHit)
    {
        if (entityHit != lastEntityHit)
        {
            ConsumeBuff(entityHit);
            AddNewBuffToEntityHit(entityHit);

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
            ConsumeBuff(lastEntityHit);
        }
    }
}
