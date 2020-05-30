public class EzrealBaseStats : CharacterBaseStats
{
    protected override void SetBaseStats()
    {
        BaseHealth = 530;
        HealthPerLevel = 88;

        BaseHealthRegeneration = 4;
        HealthRegenerationPerLevel = 0.55f;

        BaseResource = 375;
        ResourcePerLevel = 50;

        BaseResourceRegeneration = 8.5f;
        ResourceRegenerationPerLevel = 0.65f;

        BaseAttackRange = 550;

        BaseAttackDamage = 60;
        AttackDamagePerLevel = 2.5f;

        BaseAttackSpeed = 0.625f;
        AttackSpeedPerLevel = 2.5f;

        BaseArmor = 22;
        ArmorPerLevel = 3.5f;

        BaseMagicResistance = 30;
        MagicResistancePerLevel = 0.5f;

        BaseMovementSpeed = 325;
    }
}
