﻿public class MissFortuneBasicAttack : BasicAttack
{
    protected MissFortuneBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 1800;//TODO: Test in game

        basicAttackPrefabPath = "BasicAttacksPrefabs/Characters/MissFortune/MissFortuneBA";
    }
}
