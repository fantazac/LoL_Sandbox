﻿public class LucianBaseStats : CharacterBaseStats
{
    protected override void SetBaseStats()
    {
        BaseHealth = 571;
        HealthPerLevel = 86;

        BaseHealthRegeneration = 3.75f;
        HealthRegenerationPerLevel = 0.65f;

        BaseResource = 348.88f;
        ResourcePerLevel = 38;

        BaseResourceRegeneration = 8.176f;
        ResourceRegenerationPerLevel = 0.7f;

        BaseAttackRange = 500;

        BaseAttackDamage = 61;
        AttackDamagePerLevel = 2.75f;

        BaseAttackSpeed = 0.638f;
        AttackSpeedPerLevel = 3.3f;

        BaseArmor = 28;
        ArmorPerLevel = 3;

        BaseMagicResistance = 30;
        MagicResistancePerLevel = 0.5f;

        BaseMovementSpeed = 335;
    }
}
