using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsController : CharacterBase
{
    private CharacterStats characterStats;

    protected void OnEnable()
    {
        characterStats = GetComponent<CharacterStats>();
        EntityBaseStats characterBaseStats = GetComponent<EntityBaseStats>();

        characterStats.SetBaseStats(characterBaseStats);
    }

    protected override void Start()
    {
        base.Start();
    }

    public float GetCurrentMovementSpeed()
    {
        return characterStats.MovementSpeed.GetCurrentMovementSpeedForMovement();
    }

    private void OnGUI()
    {
        if (!StaticObjects.OnlineMode || PhotonView.isMine)
        {
            if (StaticObjects.OnlineMode)
            {
                GUILayout.Label("");//ping goes there in online
            }
            GUILayout.Label("HEALTH: " + characterStats.Health.GetCurrentHealth() + " / " + characterStats.Health.GetMaximumHealth() +
                " (" + characterStats.Health.GetBaseHealth() + " + " + characterStats.Health.GetBonusHealth() + ")");
            GUILayout.Label(characterStats.Resource.GetResourceType() + ": " + characterStats.Resource.GetCurrentResource() +
                " / " + characterStats.Resource.GetMaximumResource() + " (" + characterStats.Resource.GetBaseResource() +
                " + " + characterStats.Resource.GetBonusResource() + ")");

            GUILayout.Label("ATTACK DAMAGE: " + characterStats.AttackDamage.GetCurrentAttackDamage() + " (" + characterStats.AttackDamage.GetBaseAttackDamage() +
                " + " + characterStats.AttackDamage.GetBonusAttackDamage() + ")");
            GUILayout.Label("ABILITY POWER: " + characterStats.AbilityPower.GetCurrentAbilityPower() + " (" + characterStats.AbilityPower.GetBaseAbilityPower() +
                " + " + characterStats.AbilityPower.GetBonusAbilityPower() + ")");
            GUILayout.Label("ARMOR: " + characterStats.Armor.GetCurrentArmor() + " (" + characterStats.Armor.GetBaseArmor() +
                " + " + characterStats.Armor.GetBonusArmor() + ") - Takes " + (int)characterStats.Armor.GetPhysicalDamageReductionPercent() + "% reduced physical damage");
            GUILayout.Label("MAGIC RESISTANCE: " + characterStats.MagicResistance.GetCurrentMagicResistance() + " (" + characterStats.MagicResistance.GetBaseMagicResistance() +
               " + " + characterStats.MagicResistance.GetBonusMagicResistance() + ") - Takes " + (int)characterStats.MagicResistance.GetMagicDamageReductionPercent() + "% reduced magic damage");
            GUILayout.Label("ATTACK SPEED: " + characterStats.AttackSpeed.GetCurrentAttackSpeed().ToString("0.00") + " (" + characterStats.AttackSpeed.GetBaseAttackSpeed() +
               " + " + characterStats.AttackSpeed.GetBonusAttackSpeedFlat() + " (" + characterStats.AttackSpeed.GetBonusAttackSpeedPercent() + "%))");
            GUILayout.Label("COOLDOWN REDUCTION: " + characterStats.CooldownReduction.GetCurrentCooldownReduction() + "% (" + characterStats.CooldownReduction.GetBaseCooldownReduction() +
               " + " + characterStats.CooldownReduction.GetBonusCooldownReduction() + ")");
            GUILayout.Label("CRITICAL STRIKE CHANCE: " + characterStats.CriticalStrikeChance.GetCurrentCriticalStrikeChance() + "% (" + characterStats.CriticalStrikeChance.GetBaseCriticalStrikeChance() +
               " + " + characterStats.CriticalStrikeChance.GetBonusCriticalStrikeChance() + ")");
            GUILayout.Label("MOVEMENT SPEED: " + characterStats.MovementSpeed.GetCurrentMovementSpeed() + " (" + characterStats.MovementSpeed.GetBaseMovementSpeed() +
                " + " + characterStats.MovementSpeed.GetBonusMovementSpeed() + ")");

            GUILayout.Label("HEALTH REGENERATION: " + characterStats.HealthRegenaration.GetCurrentHealthRegeneration() + " (" + characterStats.HealthRegenaration.GetBaseHealthRegeneration() +
                " + ((" + characterStats.HealthRegenaration.GetBaseHealthRegeneration() + " + " + characterStats.HealthRegenaration.GetBonusHealthRegenerationFlat() +
                ") * " + characterStats.HealthRegenaration.GetBonusHealthRegenerationPercent() + "%))");
            GUILayout.Label(characterStats.Resource.GetResourceType() + " REGENERATION: " + characterStats.ResourceRegeneration.GetCurrentResourceRegeneration() +
                " (" + characterStats.ResourceRegeneration.GetBaseResourceRegeneration() + " + ((" + characterStats.ResourceRegeneration.GetBaseResourceRegeneration() +
                " + " + characterStats.ResourceRegeneration.GetBonusResourceRegenerationFlat() + ") * " + characterStats.ResourceRegeneration.GetBonusResourceRegenerationPercent() + "%))");
            GUILayout.Label("ARMOR PENETRATION: " + characterStats.ArmorPenetration.GetCurrentArmorPenetrationFlat() + " (" + characterStats.ArmorPenetration.GetBaseArmorPenetrationFlat() +
                " + " + characterStats.ArmorPenetration.GetBonusArmorPenetrationFlat() + ") | " + characterStats.ArmorPenetration.GetCurrentArmorPenetrationPercent() +
                "% (" + characterStats.ArmorPenetration.GetBaseArmorPenetrationPercent() + "% + " + characterStats.ArmorPenetration.GetBonusArmorPenetrationPercent() + "%)");
            GUILayout.Label("MAGIC PENETRATION: " + characterStats.MagicPenetration.GetCurrentMagicPenetrationFlat() + " (" + characterStats.MagicPenetration.GetBaseMagicPenetrationFlat() +
                " + " + characterStats.MagicPenetration.GetBonusMagicPenetrationFlat() + ") | " + characterStats.MagicPenetration.GetCurrentMagicPenetrationPercent() +
                "% (" + characterStats.MagicPenetration.GetBaseMagicPenetrationPercent() + "% + " + characterStats.MagicPenetration.GetBonusMagicPenetrationPercent() + "%)");
            GUILayout.Label("ATTACK RANGE: " + characterStats.AttackRange.GetCurrentAttackRange() + " (" + characterStats.AttackRange.GetBaseAttackRange() +
                " + " + characterStats.AttackRange.GetBonusAttackRange() + ")");
            GUILayout.Label("LIFE STEAL: " + characterStats.LifeSteal.GetCurrentLifeSteal() + "% (" + characterStats.LifeSteal.GetBaseLifeSteal() +
                "% + " + characterStats.LifeSteal.GetBonusLifeSteal() + "%)");
            GUILayout.Label("SPELL VAMP: " + characterStats.SpellVamp.GetCurrentSpellVamp() + "% (" + characterStats.SpellVamp.GetBaseSpellVamp() +
                "% + " + characterStats.SpellVamp.GetBonusSpellVamp() + "%)");
            GUILayout.Label("TENACITY: " + characterStats.Tenacity.GetCurrentTenacity() + "% (" + characterStats.Tenacity.GetBaseTenacity() +
                "% + " + characterStats.Tenacity.GetBonusTenacity() + "%)");
            GUILayout.Label("POSITION: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);
            GUILayout.Label("ROTATION: " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z + ", " + transform.rotation.w);

        }
    }
}
