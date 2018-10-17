public class LucianBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()
    {
        BaseHealth = 571;
        BaseResource = 348.9f;

        BaseAttackDamage = 61;
        BaseArmor = 28;
        BaseMagicResistance = 30;
        BaseAttackSpeed = 0.625f;
        AttackDelay = -0.02f;
        BaseMovementSpeed = 335;

        BaseHealthRegeneration = 3.75f;
        BaseResourceRegeneration = 8.2f;
        BaseAttackRange = 500;

        HealthPerLevel = 86;
        ResourcePerLevel = 38;

        AttackDamagePerLevel = 2.75f;
        AttackSpeedPerLevel = 3.3f;

        HealthRegenerationPerLevel = 0.65f;
        ResourceRegenerationPerLevel = 0.7f;
        ArmorPerLevel = 3;
        MagicResistancePerLevel = 0.5f;
    }
}
