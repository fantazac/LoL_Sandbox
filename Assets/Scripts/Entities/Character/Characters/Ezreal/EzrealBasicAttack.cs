using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzrealBasicAttack : CharacterBasicAttack
{
    protected EzrealBasicAttack()
    {
        delayPercentBeforeAttack = 0.2f;
        speed = 2000;
    }
}
