public class EzrealBaseStats : CharacterBaseStats
{
    protected override void SetBaseStats()
    {
        BaseHealth = 491;
        HealthPerLevel = 86;

        BaseHealthRegeneration = 4;
        HealthRegenerationPerLevel = 0.55f;

        BaseResource = 360.6f;
        ResourcePerLevel = 42;

        BaseResourceRegeneration = 8.092f;
        ResourceRegenerationPerLevel = 0.65f;

        BaseAttackRange = 550;

        BaseAttackDamage = 60;
        AttackDamagePerLevel = 2.5f;

        BaseAttackSpeed = 0.625f;
        AttackDelay = 0;
        AttackSpeedPerLevel = 1.5f;

        BaseArmor = 22;
        ArmorPerLevel = 3.5f;

        BaseMagicResistance = 30;
        MagicResistancePerLevel = 0.5f;

        BaseMovementSpeed = 325;
    }
}
