public class EzrealBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()
    {
        BaseHealth = 491;
        BaseResource = 360.6f;

        BaseAttackDamage = 60;
        BaseArmor = 22;
        BaseMagicResistance = 30;
        BaseAttackSpeed = 0.625f;
        AttackDelay = 0;
        BaseMovementSpeed = 325;

        BaseHealthRegeneration = 4;
        BaseResourceRegeneration = 8.092f;
        BaseAttackRange = 550;

        HealthPerLevel = 86;
        ResourcePerLevel = 42;

        AttackDamagePerLevel = 2.5f;
        AttackSpeedPerLevel = 1.5f;

        HealthRegenerationPerLevel = 0.55f;
        ResourceRegenerationPerLevel = 0.65f;
        ArmorPerLevel = 3.5f;
        MagicResistancePerLevel = 0.5f;
    }
}
