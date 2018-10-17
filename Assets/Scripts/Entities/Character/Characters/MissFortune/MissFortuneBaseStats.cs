public class MissFortuneBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()
    {
        BaseHealth = 541;
        BaseResource = 325.84f;

        BaseAttackDamage = 50;
        BaseArmor = 28;
        BaseMagicResistance = 30;
        BaseAttackSpeed = 0.625f;
        AttackDelay = -0.0473f;
        BaseMovementSpeed = 325;

        BaseHealthRegeneration = 3.75f;
        BaseResourceRegeneration = 8;
        BaseAttackRange = 550;

        HealthPerLevel = 91;
        ResourcePerLevel = 35;

        AttackDamagePerLevel = 2.7f;
        AttackSpeedPerLevel = 3;

        HealthRegenerationPerLevel = 0.65f;
        ResourceRegenerationPerLevel = 0.65f;
        ArmorPerLevel = 3;
        MagicResistancePerLevel = 0.5f;
    }
}
