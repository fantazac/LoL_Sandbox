public class CCBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()
    {
        BaseHealth = 500;
        BaseResource = 300;

        BaseAttackDamage = 60;
        BaseAbilityPower = 0;
        BaseArmor = 30;
        BaseMagicResistance = 30;
        BaseAttackSpeed = 0.625f;
        AttackDelay = 0;
        BaseCooldownReduction = 0;
        BaseCriticalStrikeChance = 0;
        BaseMovementSpeed = 325;

        BaseHealthRegeneration = 8f;
        BaseResourceRegeneration = 8f;
        BaseLethality = 0;
        BaseArmorPenetrationPercent = 0;
        BaseMagicPenetrationFlat = 0;
        BaseMagicPenetrationPercent = 0;
        BaseLifeSteal = 0;
        BaseSpellVamp = 0;
        BaseAttackRange = 550;
        BaseTenacity = 0;

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
