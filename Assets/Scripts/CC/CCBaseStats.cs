public class CCBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()
    {
        BaseHealth = 500;
        BaseResource = 300;

        BaseAttackDamage = 60;
        BaseArmor = 30;
        BaseMagicResistance = 30;
        BaseAttackSpeed = 0.625f;
        AttackDelay = 0;
        BaseMovementSpeed = 325;

        BaseHealthRegeneration = 8;
        BaseResourceRegeneration = 8;
        BaseAttackRange = 550;

        HealthPerLevel = 80;
        ResourcePerLevel = 40;

        AttackDamagePerLevel = 2;
        AttackSpeedPerLevel = 2;

        HealthRegenerationPerLevel = 0.5f;
        ResourceRegenerationPerLevel = 0.5f;
        ArmorPerLevel = 3;
        MagicResistancePerLevel = 0.5f;
    }
}
