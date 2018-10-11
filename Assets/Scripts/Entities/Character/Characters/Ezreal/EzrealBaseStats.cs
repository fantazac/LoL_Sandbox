public class EzrealBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()
    {
        BaseHealth = 491f;
        BaseResource = 360.6f;

        BaseAttackDamage = 60;
        BaseAbilityPower = 0;
        BaseArmor = 22;
        BaseMagicResistance = 30;
        BaseAttackSpeed = 0.625f;
        AttackDelay = 0;
        BaseCooldownReduction = 0;
        BaseCriticalStrikeChance = 0;
        BaseMovementSpeed = 325;

        BaseHealthRegeneration = 4f;
        BaseResourceRegeneration = 8.092f;
        BaseLethality = 0;
        BaseArmorPenetrationPercent = 0;
        BaseMagicPenetrationFlat = 0;
        BaseMagicPenetrationPercent = 0;
        BaseLifeSteal = 0;
        BaseSpellVamp = 0;
        BaseAttackRange = 550;
        BaseTenacity = 0;

        BaseCriticalStrikeDamage = 2f;

        HealthPerLevel = 86;
        ResourcePerLevel = 42;

        AttackDamagePerLevel = 2.5f;
        AttackSpeedPerLevel = 1.5f;

        HealthRegenerationPerLevel = 0.55f;
        ResourceRegenerationPerLevel = 0.65f;
        ArmorPerLevel = 3.5f;
        MagicResistancePerLevel = 0.5f;
    }
}
