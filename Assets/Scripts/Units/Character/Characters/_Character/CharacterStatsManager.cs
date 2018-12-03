using System.Collections;
using UnityEngine;

public abstract class CharacterStatsManager : StatsManager
{
    private float regenerationPercentPerTick;
    private float regenerationInterval;
    private WaitForSeconds delayRegeneration;

    protected CharacterStatsManager()
    {
        regenerationInterval = 0.5f;
        regenerationPercentPerTick = regenerationInterval / 5f;
        delayRegeneration = new WaitForSeconds(regenerationInterval);
    }

    protected override void InitializUnitStats(BaseStats baseStats)
    {
        InitializeCharacterStats((CharacterBaseStats)baseStats);
    }

    protected virtual void InitializeCharacterStats(CharacterBaseStats characterBaseStats)
    {
        Health = new Health(characterBaseStats.BaseHealth, characterBaseStats.HealthPerLevel);

        AttackDamage = new AttackDamage(characterBaseStats.BaseAttackDamage, characterBaseStats.AttackDamagePerLevel);
        AbilityPower = new AbilityPower();
        Armor = new Resistance(characterBaseStats.BaseArmor, characterBaseStats.ArmorPerLevel);
        MagicResistance = new Resistance(characterBaseStats.BaseMagicResistance, characterBaseStats.MagicResistancePerLevel);
        AttackSpeed = new AttackSpeed(characterBaseStats.BaseAttackSpeed, characterBaseStats.AttackSpeedPerLevel);
        CooldownReduction = new CooldownReduction();
        CriticalStrikeChance = new CriticalStrikeChance();
        MovementSpeed = new MovementSpeed(characterBaseStats.BaseMovementSpeed);

        HealthRegeneration = new ResourceRegeneration(characterBaseStats.BaseHealthRegeneration, characterBaseStats.HealthRegenerationPerLevel);
        Lethality = new Lethality();
        ArmorPenetrationPercent = new ResistancePenetrationPercent();
        MagicPenetrationFlat = new ResistancePenetrationFlat();
        MagicPenetrationPercent = new ResistancePenetrationPercent();
        LifeSteal = new PercentBonusOnlyStat();
        SpellVamp = new PercentBonusOnlyStat();
        AttackRange = new AttackRange(characterBaseStats.BaseAttackRange);
        Tenacity = new Tenacity();

        CriticalStrikeDamage = new CriticalStrikeDamage();
        CriticalStrikeDamageReduction = new PercentBonusOnlyStat();
        PhysicalDamageModifier = new DamageModifier();
        MagicDamageModifier = new DamageModifier();
        PhysicalDamageReceivedModifier = new DamageModifier();
        MagicDamageReceivedModifier = new DamageModifier();
        HealAndShieldPower = new PercentBonusOnlyStat();
        SlowResistance = new SlowResistance();

        GrievousWounds = new GrievousWounds();

        ExtraAdjustments();
    }

    protected void Start()
    {
        CharacterLevelManager characterLevelManager = GetComponent<CharacterLevelManager>();

        if (characterLevelManager)
        {
            characterLevelManager.OnLevelUp += Health.OnLevelUp;
            characterLevelManager.OnLevelUp += Resource.OnLevelUp;
            characterLevelManager.OnLevelUp += AttackDamage.OnLevelUp;
            characterLevelManager.OnLevelUp += Armor.OnLevelUp;
            characterLevelManager.OnLevelUp += MagicResistance.OnLevelUp;
            characterLevelManager.OnLevelUp += AttackSpeed.OnLevelUp;
            characterLevelManager.OnLevelUp += HealthRegeneration.OnLevelUp;
            characterLevelManager.OnLevelUp += ResourceRegeneration.OnLevelUp;
            characterLevelManager.OnLevelUp += Lethality.OnLevelUp;
        }

        if (Health != null && HealthRegeneration != null)
        {
            if (Resource != null && ResourceRegeneration != null)
            {
                StartCoroutine(RegenerateHealthAndResource());
            }
            else
            {
                StartCoroutine(RegenerateHealth());
            }
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            UpdateHealth();

            yield return delayRegeneration;
        }
    }

    private IEnumerator RegenerateHealthAndResource()
    {
        while (true)
        {
            UpdateHealth();
            UpdateResource();

            yield return delayRegeneration;
        }
    }

    private void UpdateHealth()
    {
        if (Health.GetTotal() > Health.GetCurrentValue())
        {
            RestoreHealth(HealthRegeneration.GetTotal() * regenerationPercentPerTick);
        }
    }

    private void UpdateResource()
    {
        if (Resource.GetTotal() > Resource.GetCurrentValue())
        {
            Resource.Restore(ResourceRegeneration.GetTotal() * regenerationPercentPerTick);
        }
    }
}
