public class CCBaseStats : CharacterBaseStats
{
    protected override void SetBaseStats()
    {
        BaseHealth = 500;
        HealthPerLevel = 80;

        BaseHealthRegeneration = 8;
        HealthRegenerationPerLevel = 0.5f;

        BaseResource = 300;
        ResourcePerLevel = 40;

        BaseResourceRegeneration = 8;
        ResourceRegenerationPerLevel = 0.5f;

        BaseAttackRange = 550;

        BaseAttackDamage = 60;
        AttackDamagePerLevel = 2;

        BaseAttackSpeed = 0.658f;
        AttackSpeedPerLevel = 2;

        BaseArmor = 30;
        ArmorPerLevel = 3;

        BaseMagicResistance = 30;
        MagicResistancePerLevel = 0.5f;

        BaseMovementSpeed = 325;
    }
}
