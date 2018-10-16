public abstract class CharacterStats : EntityStats
{
    //extra stats characters have that other entities don't

    protected override void InitializeEntityStats(EntityBaseStats entityBaseStats)
    {
        InitializeCharacterStats((CharacterBaseStats)entityBaseStats);
    }

    protected virtual void InitializeCharacterStats(CharacterBaseStats characterBaseStats)
    {
        Health = new Health(characterBaseStats.BaseHealth, characterBaseStats.HealthPerLevel);

        AttackDamage = new AttackDamage(characterBaseStats.BaseAttackDamage, characterBaseStats.AttackDamagePerLevel);
        AbilityPower = new AbilityPower();
        Armor = new Armor(characterBaseStats.BaseArmor, characterBaseStats.ArmorPerLevel);
        MagicResistance = new MagicResistance(characterBaseStats.BaseMagicResistance, characterBaseStats.MagicResistancePerLevel);
        AttackSpeed = new AttackSpeed(characterBaseStats.BaseAttackSpeed, characterBaseStats.AttackSpeedPerLevel, characterBaseStats.AttackDelay);
        CooldownReduction = new CooldownReduction();
        CriticalStrikeChance = new CriticalStrikeChance();
        MovementSpeed = new MovementSpeed(characterBaseStats.BaseMovementSpeed);

        HealthRegeneration = new HealthRegeneration(characterBaseStats.BaseHealthRegeneration, characterBaseStats.HealthRegenerationPerLevel);
        Lethality = new Lethality();
        ArmorPenetrationPercent = new ArmorPenetrationPercent();
        MagicPenetrationFlat = new MagicPenetrationFlat();
        MagicPenetrationPercent = new MagicPenetrationPercent();
        LifeSteal = new LifeSteal();
        SpellVamp = new SpellVamp();
        AttackRange = new AttackRange(characterBaseStats.BaseAttackRange);
        Tenacity = new Tenacity();

        CriticalStrikeDamage = new CriticalStrikeDamage(characterBaseStats.BaseCriticalStrikeDamage);
        CriticalStrikeDamageReduction = new CriticalStrikeDamageReduction();
        PhysicalDamageModifier = new PhysicalDamageModifier();
        MagicDamageModifier = new MagicDamageModifier();
        PhysicalDamageReceivedModifier = new PhysicalDamageReceivedModifier();
        MagicDamageReceivedModifier = new MagicDamageReceivedModifier();

        //set extra character stats

        ExtraAdjustments();
    }
}