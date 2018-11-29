public class MissFortuneBaseStats : CharacterBaseStats
{
    protected override void SetBaseStats()
    {
        BaseHealth = 541;
        HealthPerLevel = 91;

        BaseHealthRegeneration = 3.75f;
        HealthRegenerationPerLevel = 0.65f;

        BaseResource = 325.84f;
        ResourcePerLevel = 35;

        BaseResourceRegeneration = 8.042f;
        ResourceRegenerationPerLevel = 0.65f;

        BaseAttackRange = 550;

        BaseAttackDamage = 50;
        AttackDamagePerLevel = 2.7f;

        BaseAttackSpeed = 0.656f;
        AttackSpeedPerLevel = 3;

        BaseArmor = 28;
        ArmorPerLevel = 3;

        BaseMagicResistance = 30;
        MagicResistancePerLevel = 0.5f;

        BaseMovementSpeed = 325;
    }
}
