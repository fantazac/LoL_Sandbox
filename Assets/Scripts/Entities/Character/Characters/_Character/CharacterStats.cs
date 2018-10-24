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
        Armor = new Resistance(characterBaseStats.BaseArmor, characterBaseStats.ArmorPerLevel);
        MagicResistance = new Resistance(characterBaseStats.BaseMagicResistance, characterBaseStats.MagicResistancePerLevel);
        AttackSpeed = new AttackSpeed(characterBaseStats.BaseAttackSpeed, characterBaseStats.AttackSpeedPerLevel, characterBaseStats.AttackDelay);
        CooldownReduction = new CooldownReduction();
        CriticalStrikeChance = new CriticalStrikeChance();
        MovementSpeed = new MovementSpeed(characterBaseStats.BaseMovementSpeed);

        HealthRegeneration = new ResourceRegeneration(characterBaseStats.BaseHealthRegeneration, characterBaseStats.HealthRegenerationPerLevel);
        Lethality = new Lethality();
        ArmorPenetrationPercent = new ResistancePenetrationPercent();
        MagicPenetrationFlat = new ResistancePenetrationFlat();
        MagicPenetrationPercent = new ResistancePenetrationPercent();
        LifeSteal = new LifeSteal();
        SpellVamp = new SpellVamp();
        AttackRange = new AttackRange(characterBaseStats.BaseAttackRange);
        Tenacity = new Tenacity();

        CriticalStrikeDamage = new CriticalStrikeDamage();
        CriticalStrikeDamageReduction = new CriticalStrikeDamageReduction();
        PhysicalDamageModifier = new DamageModifier();
        MagicDamageModifier = new DamageModifier();
        PhysicalDamageReceivedModifier = new DamageModifier();
        MagicDamageReceivedModifier = new DamageModifier();
        HealAndShieldPower = new HealAndShieldPower();
        SlowResistance = new SlowResistance();

        //set extra character stats

        ExtraAdjustments();
    }
}