using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_P : PassiveTargeted, CharacterAbility, PassiveCharacterAbility
{
    private Ability lucianE;

    protected Lucian_P()
    {
        abilityName = "Lightslinger";

        abilityType = AbilityType.Passive;

        damage = 0.5f;

        buffDuration = 3;
    }

    protected override void Awake()
    {
        base.Awake();

        lucianE = GetComponent<Lucian_E>();
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianP";
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianP_Buff";
    }

    protected override void Start()
    {
        base.Start();

        foreach (Ability ability in GetComponents<CharacterAbility>())
        {
            if (!(ability is PassiveCharacterAbility))
            {
                ability.OnAbilityFinished += PassiveEffect;
            }
        }

        character.CharacterLevelManager.OnLevelUp += OnLevelUp;
    }

    public override void UseAbility(Entity target)
    {
        if (target is Character)
        {
            lucianE.ReduceCooldown(2);
        }
        else
        {
            lucianE.ReduceCooldown(1);
        }
    }

    public override void OnLevelUp(int level)
    {
        if (level == 7)
        {
            damage = 0.55f;
        }
        else if (level == 13)
        {
            damage = 0.6f;
        }
    }

    public void OnPassiveHit(Entity entityHit)
    {
        //if (entityHit is Minion)
        //{
        //    entityHit.EntityStats.Health.Reduce(character.EntityStats.AttackDamage.GetTotal());
        //}
        //else
        //{
        entityHit.EntityStats.Health.Reduce(character.EntityStats.AttackDamage.GetTotal() * damage);
        //} 
        UseAbility(entityHit);
    }
}
