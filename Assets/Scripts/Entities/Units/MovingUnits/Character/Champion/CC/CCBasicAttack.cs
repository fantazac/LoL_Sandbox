public class CCBasicAttack : EmpoweredBasicAttack
{
    protected CCBasicAttack()
    {
        delayPercentBeforeAttack = 0.1666f;
        speed = 1800;

        basicAttackPrefabPath = "BasicAttacksPrefabs/Characters/CC/CCBA";
        empoweredBasicAttackPrefabPath = "BasicAttacksPrefabs/Characters/CC/CCBAQ";
    }
}

