public class DummyBaseStats : CharacterBaseStats
{
    protected override void SetBaseStats()
    {
        BaseHealth = 10000;

        BaseAttackDamage = 0;
        BaseArmor = 100;
        BaseMagicResistance = 100;
        BaseAttackSpeed = 0.625f;
        BaseMovementSpeed = 325;
    }
}
