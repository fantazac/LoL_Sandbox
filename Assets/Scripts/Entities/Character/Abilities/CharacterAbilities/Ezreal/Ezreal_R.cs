﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_R : SkillShot, CharacterAbility
{
    protected Ezreal_R()
    {
        range = (float)Range.GLOBAL;
        speed = 2000;
        damage = 300;
        castTime = 1;
        delayCastTime = new WaitForSeconds(castTime);

        CanStopMovement = true;
        HasCastTime = true;
    }
}
