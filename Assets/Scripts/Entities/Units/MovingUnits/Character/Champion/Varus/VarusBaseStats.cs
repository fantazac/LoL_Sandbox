public class VarusBaseStats : CharacterBaseStats
{
    protected override void SetBaseStats()
    {
        BaseHealth = 530;
        HealthPerLevel = 91;

        BaseHealthRegeneration = 3.5f;
        HealthRegenerationPerLevel = 0.55f;

        BaseResource = 360;
        ResourcePerLevel = 33;

        BaseResourceRegeneration = 8;
        ResourceRegenerationPerLevel = 0.8f;

        BaseAttackRange = 575;

        BaseAttackDamage = 61;
        AttackDamagePerLevel = 3.11f;

        BaseAttackSpeed = 0.658f;
        AttackSpeedPerLevel = 3;

        BaseArmor = 27;
        ArmorPerLevel = 3.4f;

        BaseMagicResistance = 30;
        MagicResistancePerLevel = 0.5f;

        BaseMovementSpeed = 330;
    }
}
