using UnityEngine;
using System.Collections;

public class Lucian_E : DirectionTargetedDash, CharacterAbility
{
    protected Lucian_E()
    {
        range = 425;
        //resourceCost = 40;// 40/30/20/10/0
        cooldown = 22;// 22/20/18/16/14
        minimumDistanceTravelled = 200;
        dashSpeed = 32;

        ResetBasicAttackCycleOnAbilityFinished = true;

        startCooldownOnAbilityCast = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Lucian/LucianE";
    }
}
