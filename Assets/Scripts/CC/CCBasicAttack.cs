using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCBasicAttack : CharacterBasicAttack
{
    protected CCBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 1800;

        basicAttackPrefabPath = "BasicAttacks/Characters/CC/CCBA";
    }
}

