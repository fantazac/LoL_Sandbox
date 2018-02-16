using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_P : PassiveTargeted, CharacterAbility, PassiveCharacterAbility
{
    private Ability lucianE;

    protected Lucian_P()
    {
        damage = 0.5f;// 0.5f/0.55f/0.6f

        buffDuration = 3;
    }

    protected override void Awake()
    {
        base.Awake();

        lucianE = GetComponent<Lucian_E>();
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Lucian/LucianP";
        buffSpritePath = "Sprites/CharacterAbilities/Lucian/LucianP_Buff";
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
