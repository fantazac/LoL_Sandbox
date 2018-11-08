public class VarusBaseStats : CharacterBaseStats
{
    protected override void SetBaseStats()
    {
        BaseHealth = 499;
        HealthPerLevel = 89;

        BaseHealthRegeneration = 3.5f;
        HealthRegenerationPerLevel = 0.55f;

        BaseResource = 360.48f;
        ResourcePerLevel = 33;

        BaseResourceRegeneration = 7.34f;
        ResourceRegenerationPerLevel = 0.8f;

        BaseAttackRange = 575;

        BaseAttackDamage = 59;
        AttackDamagePerLevel = 3.11f;

        BaseAttackSpeed = 0.625f;
        AttackDelay = -0.05f;
        AttackSpeedPerLevel = 3;

        BaseArmor = 27;
        ArmorPerLevel = 3.4f;

        BaseMagicResistance = 30;
        MagicResistancePerLevel = 0.5f;

        BaseMovementSpeed = 330;
    }
}
