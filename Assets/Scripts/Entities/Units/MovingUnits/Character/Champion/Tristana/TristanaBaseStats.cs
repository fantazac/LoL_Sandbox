public class TristanaBaseStats : CharacterBaseStats
{
    protected override void SetBaseStats()
    {
        BaseHealth = 559;
        HealthPerLevel = 88;

        BaseHealthRegeneration = 3.75f;
        HealthRegenerationPerLevel = 0.65f;

        BaseResource = 250;
        ResourcePerLevel = 32;

        BaseResourceRegeneration = 7.2f;
        ResourceRegenerationPerLevel = 0.45f;

        BaseAttackRange = 525;

        BaseAttackDamage = 61;
        AttackDamagePerLevel = 3.3f;

        BaseAttackSpeed = 0.679f;
        AttackSpeedPerLevel = 1.5f;

        BaseArmor = 26;
        ArmorPerLevel = 3;

        BaseMagicResistance = 30;
        MagicResistancePerLevel = 0.5f;

        BaseMovementSpeed = 325;
    }
}
