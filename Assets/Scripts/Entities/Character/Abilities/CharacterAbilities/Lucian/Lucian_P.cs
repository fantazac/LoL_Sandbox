using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_P : PassiveTargeted, CharacterAbility, PassiveCharacterAbility
{
    protected Lucian_P()
    {
        buffDuration = 3;
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

    protected override void AddNewBuffToEntityHit(Entity entityHit)
    {
        if (passive == null || passive.HasExpired())
        {
            passive = new Buff(this, character, buffDuration);
            entityHit.EntityBuffManager.ApplyBuff(passive, buffSprite);
        }
        else
        {
            passive.ResetDurationRemaining();
        }
    }
}
