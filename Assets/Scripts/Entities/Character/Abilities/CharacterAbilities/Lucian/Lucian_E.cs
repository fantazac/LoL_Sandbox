using UnityEngine;
using System.Collections;

public class Lucian_E : DirectionTargetedDash, CharacterAbility
{
    protected Lucian_E()
    {
        abilityName = "Relentless Pursuit";

        abilityType = AbilityType.Dash;

        MaxLevel = 5;

        range = 425;
        resourceCost = 40;// 40/30/20/10/0
        resourceCostPerLevel = -10;
        baseCooldown = 22;// 22/20/18/16/14
        baseCooldownPerLevel = -2;
        minimumDistanceTravelled = 200;
        dashSpeed = 32;

        ResetBasicAttackCycleOnAbilityFinished = true;

        affectedByCooldownReduction = true;
        startCooldownOnAbilityCast = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianE";
    }
}
