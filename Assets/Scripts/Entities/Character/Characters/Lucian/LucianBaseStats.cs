public class LucianBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()
    {
        BaseHealth = 571;
        BaseResource = 348.9f;

        BaseAttackDamage = 61;
        BaseAbilityPower = 0;
        BaseArmor = 28;
        BaseMagicResistance = 30;
        BaseAttackSpeed = 0.625f;
        AttackDelay = -0.02f;
        BaseCooldownReduction = 0;
        BaseCriticalStrikeChance = 0;
        BaseMovementSpeed = 335;

        BaseHealthRegeneration = 3.75f;
        BaseResourceRegeneration = 8.2f;
        BaseLethality = 0;
        BaseArmorPenetrationPercent = 0;
        BaseMagicPenetrationFlat = 0;
        BaseMagicPenetrationPercent = 0;
        BaseLifeSteal = 0;
        BaseSpellVamp = 0;
        BaseAttackRange = 500;
        BaseTenacity = 0;

        BaseCriticalStrikeDamage = 2f;
        BaseCriticalStrikeDamageReduction = 0;
        BasePhysicalDamageModifier = 1f;
        BaseMagicDamageModifier = 1f;
        BasePhysicalDamageReceivedModifier = 1f;
        BaseMagicDamageReceivedModifier = 1f;
        BaseHealAndShieldPower = 0;
        BaseSlowResistance = 0;

        HealthPerLevel = 86;
        ResourcePerLevel = 38;

        AttackDamagePerLevel = 2.75f;
        AttackSpeedPerLevel = 3.3f;

        HealthRegenerationPerLevel = 0.65f;
        ResourceRegenerationPerLevel = 0.7f;
        ArmorPerLevel = 3;
        MagicResistancePerLevel = 0.5f;
    }
}
