using System.Text;
using UnityEngine;

public class StatsUIManager : MonoBehaviour
{
    private CharacterStats characterStats;

    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    private void OnGUI()
    {
        if (StaticObjects.OnlineMode)
        {
            GUILayout.Label("");//ping goes there in online mode
        }

        StringBuilder statsText = new StringBuilder();
        if (!Input.GetKey(KeyCode.C))
        {
            statsText.AppendFormat("HEALTH: {0} / {1}", characterStats.Health.GetCurrentValue(), characterStats.Health.GetTotal()).AppendLine();
            statsText.AppendFormat("{0}: {1} / {2}", characterStats.ResourceType, characterStats.Resource.GetCurrentValue(), characterStats.Resource.GetTotal()).AppendLine();

            statsText.AppendFormat("ATTACK DAMAGE: {0}", characterStats.AttackDamage.GetTotal()).AppendLine();
            statsText.AppendFormat("ABILITY POWER: {0}", characterStats.AbilityPower.GetTotal()).AppendLine();
            statsText.AppendFormat("ARMOR: {0}", characterStats.Armor.GetTotal()).AppendLine();
            statsText.AppendFormat("MAGIC RESISTANCE: {0}", characterStats.MagicResistance.GetTotal()).AppendLine();
            statsText.AppendFormat("ATTACK SPEED: {0:0.00}", characterStats.AttackSpeed.GetTotal()).AppendLine();
            statsText.AppendFormat("COOLDOWN REDUCTION: {0}%", characterStats.CooldownReduction.GetTotal()).AppendLine();
            statsText.AppendFormat("CRITICAL STRIKE CHANCE: {0}%", characterStats.CriticalStrikeChance.GetTotal()).AppendLine();
            statsText.AppendFormat("MOVEMENT SPEED: {0}", characterStats.MovementSpeed.GetTotal() * 100).AppendLine();

            statsText.AppendFormat("HEALTH REGENERATION: {0}", characterStats.HealthRegeneration.GetTotal()).AppendLine();
            statsText.AppendFormat("{0} REGENERATION: {1}", characterStats.ResourceType, characterStats.ResourceRegeneration.GetTotal()).AppendLine();
            statsText.AppendFormat("LETHALITY: {0} (Current value: {1})", characterStats.Lethality.GetTotal(), characterStats.Lethality.GetCurrentValue()).AppendLine();
            statsText.AppendFormat("ARMOR PENETRATION PERCENT: {0}%", characterStats.ArmorPenetrationPercent.GetTotal()).AppendLine();
            statsText.AppendFormat("MAGIC PENETRATION FLAT: {0} ", characterStats.MagicPenetrationFlat.GetTotal()).AppendLine();
            statsText.AppendFormat("MAGIC PENETRATION PERCENT: {0}%", characterStats.MagicPenetrationPercent.GetTotal()).AppendLine();
            statsText.AppendFormat("LIFE STEAL: {0}%", characterStats.LifeSteal.GetTotal()).AppendLine();
            statsText.AppendFormat("SPELL VAMP: {0}%", characterStats.SpellVamp.GetTotal()).AppendLine();
            statsText.AppendFormat("ATTACK RANGE: {0}", characterStats.AttackRange.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("TENACITY: {0}%", characterStats.Tenacity.GetTotal()).AppendLine();
        }
        else
        {
            statsText.AppendFormat("HEALTH: {0} / {1} (({2} + {3}) * {4}% * -{5}% - {6})",
                characterStats.Health.GetCurrentValue(), characterStats.Health.GetTotal(), characterStats.Health.GetBaseValue(),
                characterStats.Health.GetFlatBonus(), characterStats.Health.GetPercentBonus(), characterStats.Health.GetPercentMalus(),
                characterStats.Health.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("{0}: {1} / {2} (({3} + {4}) * {5}% * -{6}% - {7})", characterStats.ResourceType,
                characterStats.Resource.GetCurrentValue(), characterStats.Resource.GetTotal(), characterStats.Resource.GetBaseValue(),
                characterStats.Resource.GetFlatBonus(), characterStats.Resource.GetPercentBonus(), characterStats.Resource.GetPercentMalus(),
                characterStats.Resource.GetFlatMalus()).AppendLine();

            statsText.AppendFormat("ATTACK DAMAGE: {0} (({1} + {2}) * {3}% * -{4}% - {5})",
                characterStats.AttackDamage.GetTotal(), characterStats.AttackDamage.GetBaseValue(), characterStats.AttackDamage.GetFlatBonus(),
                characterStats.AttackDamage.GetPercentBonus(), characterStats.AttackDamage.GetPercentMalus(), characterStats.AttackDamage.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("ABILITY POWER: {0} (({1} + {2}) * {3}% * -{4}% - {5})",
                characterStats.AbilityPower.GetTotal(), characterStats.AbilityPower.GetBaseValue(), characterStats.AbilityPower.GetFlatBonus(),
                characterStats.AbilityPower.GetPercentBonus(), characterStats.AbilityPower.GetPercentMalus(), characterStats.AbilityPower.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("ARMOR: {0} (({1} + {2}) * {3}% * -{4}% - {5}) Takes {6}% reduced physical damage (Eff HP: {7}%)",
                characterStats.Armor.GetTotal(), characterStats.Armor.GetBaseValue(), characterStats.Armor.GetFlatBonus(),
                characterStats.Armor.GetPercentBonus(), characterStats.Armor.GetPercentMalus(), characterStats.Armor.GetFlatMalus(),
                (int)Mathf.Round(characterStats.Armor.GetPhysicalDamageReductionPercent() * 100), characterStats.Armor.GetPhysicalEffectiveHealthPercent() * 100).AppendLine();
            statsText.AppendFormat("MAGIC RESISTANCE: {0} (({1} + {2}) * {3}% * -{4}% - {5}) Takes {6}% reduced magic damage (Eff HP: {7}%)",
                characterStats.MagicResistance.GetTotal(), characterStats.MagicResistance.GetBaseValue(), characterStats.MagicResistance.GetFlatBonus(),
                characterStats.MagicResistance.GetPercentBonus(), characterStats.MagicResistance.GetPercentMalus(), characterStats.MagicResistance.GetFlatMalus(),
                (int)Mathf.Round(characterStats.MagicResistance.GetMagicDamageReductionPercent() * 100), characterStats.MagicResistance.GetMagicEffectiveHealthPercent() * 100).AppendLine();
            statsText.AppendFormat("ATTACK SPEED: {0} (({1} + {2}) * {3}% * -{4}% - {5})",
                characterStats.AttackSpeed.GetTotal(), characterStats.AttackSpeed.GetBaseValue(), characterStats.AttackSpeed.GetFlatBonus(),
                characterStats.AttackSpeed.GetPercentBonus(), characterStats.AttackSpeed.GetPercentMalus(), characterStats.AttackSpeed.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("COOLDOWN REDUCTION: {0}% ({1}% + {2}% - {3}%)",
                characterStats.CooldownReduction.GetTotal(), characterStats.CooldownReduction.GetBaseValue(),
                characterStats.CooldownReduction.GetFlatBonus(), characterStats.CooldownReduction.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("CRITICAL STRIKE CHANCE: {0}% ({1}% + {2}% - {3}%)",
                characterStats.CriticalStrikeChance.GetTotal(), characterStats.CriticalStrikeChance.GetBaseValue(),
                characterStats.CriticalStrikeChance.GetFlatBonus(), characterStats.CriticalStrikeChance.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("MOVEMENT SPEED: {0} (({1} + {2}) * {3}% * -{4}% - {5})",
                characterStats.MovementSpeed.GetTotal() * 100, characterStats.MovementSpeed.GetBaseValue(), characterStats.MovementSpeed.GetFlatBonus(),
                characterStats.MovementSpeed.GetPercentBonus(), characterStats.MovementSpeed.GetPercentMalus(), characterStats.MovementSpeed.GetFlatMalus()).AppendLine();

            statsText.AppendFormat("HEALTH REGENERATION: {0} (({1} + {2}) * {3}% * -{4}% - {5})",
                characterStats.HealthRegeneration.GetTotal(), characterStats.HealthRegeneration.GetBaseValue(), characterStats.HealthRegeneration.GetFlatBonus(),
                characterStats.HealthRegeneration.GetPercentBonus(), characterStats.HealthRegeneration.GetPercentMalus(), characterStats.HealthRegeneration.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("{0} REGENERATION: {1} (({2} + {3}) * {4}% * -{5}% - {6})", characterStats.ResourceType,
                characterStats.ResourceRegeneration.GetTotal(), characterStats.ResourceRegeneration.GetBaseValue(), characterStats.ResourceRegeneration.GetFlatBonus(),
                characterStats.ResourceRegeneration.GetPercentBonus(), characterStats.ResourceRegeneration.GetPercentMalus(), characterStats.ResourceRegeneration.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("LETHALITY: {0} (Current value: {1}) ({2} + {3} - {4})",
                characterStats.Lethality.GetTotal(), characterStats.Lethality.GetCurrentValue(),
                characterStats.Lethality.GetBaseValue(), characterStats.Lethality.GetFlatBonus(), characterStats.Lethality.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("ARMOR PENETRATION PERCENT: {0}% ({1}% + {2}% - {3}%)",
                characterStats.ArmorPenetrationPercent.GetTotal(), characterStats.ArmorPenetrationPercent.GetBaseValue(),
                characterStats.ArmorPenetrationPercent.GetFlatBonus(), characterStats.ArmorPenetrationPercent.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("MAGIC PENETRATION FLAT: {0} ({1} + {2} - {3})",
                characterStats.MagicPenetrationFlat.GetTotal(), characterStats.MagicPenetrationFlat.GetBaseValue(),
                characterStats.MagicPenetrationFlat.GetFlatBonus(), characterStats.MagicPenetrationFlat.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("MAGIC PENETRATION PERCENT: {0}% ({1}% + {2}% - {3}%)",
                characterStats.MagicPenetrationPercent.GetTotal(), characterStats.MagicPenetrationPercent.GetBaseValue(),
                characterStats.MagicPenetrationPercent.GetFlatBonus(), characterStats.MagicPenetrationPercent.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("LIFE STEAL: {0}% ({1}% + {2}% - {3}%)",
                characterStats.LifeSteal.GetTotal(), characterStats.LifeSteal.GetBaseValue(),
                characterStats.LifeSteal.GetFlatBonus(), characterStats.LifeSteal.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("SPELL VAMP: {0}% ({1}% + {2}% - {3}%)",
                characterStats.SpellVamp.GetTotal(), characterStats.SpellVamp.GetBaseValue(),
                characterStats.SpellVamp.GetFlatBonus(), characterStats.SpellVamp.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("ATTACK RANGE: {0} (({1} + {2}) * {3}% * -{4}% - {5})",
                characterStats.AttackRange.GetTotal() * 100, characterStats.AttackRange.GetBaseValue(), characterStats.AttackRange.GetFlatBonus(),
                characterStats.AttackRange.GetPercentBonus(), characterStats.AttackRange.GetPercentMalus(), characterStats.AttackRange.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("TENACITY: {0}% ({1}% + {2}% - {3}%)",
                characterStats.Tenacity.GetTotal(), characterStats.Tenacity.GetBaseValue(),
                characterStats.Tenacity.GetFlatBonus(), characterStats.Tenacity.GetFlatMalus()).AppendLine();
        }
        GUILayout.Label(statsText.ToString());
        GUILayout.Label("POSITION: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);
        GUILayout.Label("ROTATION: " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z + ", " + transform.rotation.w);
    }
}
