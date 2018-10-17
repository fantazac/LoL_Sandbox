public class DummyBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()
    {
        BaseHealth = 10000;

        BaseAttackDamage = 0;
        BaseArmor = 100;
        BaseMagicResistance = 100;
        BaseAttackSpeed = 0.625f;
        AttackDelay = -0.05f;
        BaseMovementSpeed = 325;
    }
}
