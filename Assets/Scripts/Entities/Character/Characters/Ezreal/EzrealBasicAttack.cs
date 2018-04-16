using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzrealBasicAttack : CharacterBasicAttack
{
    protected EzrealBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 1600;

        basicAttackPrefabPath = "BasicAttacks/Characters/Ezreal/EzrealBA";
    }
}

