public class TristanaBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()//TODO: change base stats setup to this isntead of previous one (matches league wiki, easier to read)
    {
        BaseHealth = 559;
        HealthPerLevel = 88;

        BaseHealthRegeneration = 3.75f;
        HealthRegenerationPerLevel = 0.65f;

        BaseResource = 246.76f;
        ResourcePerLevel = 32;

        BaseResourceRegeneration = 7.206f;
        ResourceRegenerationPerLevel = 0.45f;

        BaseAttackRange = 525;

        BaseAttackDamage = 61;
        AttackDamagePerLevel = 3.11f;

        BaseAttackSpeed = 0.625f;
        AttackDelay = -0.0473f;
        AttackSpeedPerLevel = 1.5f;

        BaseArmor = 26;
        ArmorPerLevel = 3;

        BaseMagicResistance = 30;
        MagicResistancePerLevel = 0.5f;

        BaseMovementSpeed = 325;
    }
}
