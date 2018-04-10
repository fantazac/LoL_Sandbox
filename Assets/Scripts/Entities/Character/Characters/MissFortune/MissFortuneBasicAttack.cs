using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortuneBasicAttack : CharacterBasicAttack
{
    protected MissFortuneBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 1800;//TODO: Test in game
    }
}
