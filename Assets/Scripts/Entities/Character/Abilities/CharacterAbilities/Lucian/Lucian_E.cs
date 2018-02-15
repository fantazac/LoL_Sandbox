using UnityEngine;
using System.Collections;

public class Lucian_E : DirectionTargetedDash, CharacterAbility
{
    protected Lucian_E()
    {
        range = 425;
        cooldown = 6;
        minimumDistanceTravelled = 100;
        dashSpeed = 32;

        startCooldownOnAbilityCast = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Lucian/LucianE";
    }
}
