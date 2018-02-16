using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucianBasicAttack : CharacterBasicAttack
{
    protected LucianBasicAttack()
    {
        delayPercentBeforeAttack = 0.15f;
        speed = 2500;
    }
}
