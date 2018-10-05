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
            statsText.AppendFormat("HEALTH: {0} / {1} (({2} + {3}) + {4}%)",
                characterStats.Health.GetCurrentValue(), characterStats.Health.GetTotal(), characterStats.Health.GetCurrentBaseValue(),
                characterStats.Health.GetFlatBonus(), characterStats.Health.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("{0}: {1} / {2} (({3} + {4}) + {5}%)", characterStats.ResourceType,
                characterStats.Resource.GetCurrentValue(), characterStats.Resource.GetTotal(), characterStats.Resource.GetCurrentBaseValue(),
                characterStats.Resource.GetFlatBonus(), characterStats.Resource.GetPercentBonus()).AppendLine();

            statsText.AppendFormat("ATTACK DAMAGE: {0} (({1} + {2}) + {3}% - {4})",
                characterStats.AttackDamage.GetTotal(), characterStats.AttackDamage.GetCurrentBaseValue(), characterStats.AttackDamage.GetFlatBonus(),
                characterStats.AttackDamage.GetPercentBonus(), characterStats.AttackDamage.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("ABILITY POWER: {0} (({1} + {2}) + {3}%)",
                characterStats.AbilityPower.GetTotal(), characterStats.AbilityPower.GetCurrentBaseValue(), characterStats.AbilityPower.GetFlatBonus(),
                characterStats.AbilityPower.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("ARMOR: {0} ((({1} + {2}) + {3}% - {4}) - {5}%)  Takes {6}% reduced physical damage (Eff HP: {7}%)",
                characterStats.Armor.GetTotal(), characterStats.Armor.GetCurrentBaseValue(), characterStats.Armor.GetFlatBonus(),
                characterStats.Armor.GetPercentBonus(), characterStats.Armor.GetFlatMalus(), characterStats.Armor.GetPercentMalus(),
                (int)Mathf.Round(characterStats.Armor.GetPhysicalDamageReductionPercent() * 100), characterStats.Armor.GetPhysicalEffectiveHealthPercent() * 100).AppendLine();
            statsText.AppendFormat("MAGIC RESISTANCE: {0} ((({1} + {2}) + {3}% - {4}) - {5}%) Takes {6}% reduced magic damage (Eff HP: {7}%)",
                characterStats.MagicResistance.GetTotal(), characterStats.MagicResistance.GetCurrentBaseValue(), characterStats.MagicResistance.GetFlatBonus(),
                characterStats.MagicResistance.GetPercentBonus(), characterStats.MagicResistance.GetFlatMalus(), characterStats.MagicResistance.GetPercentMalus(),
                (int)Mathf.Round(characterStats.MagicResistance.GetMagicDamageReductionPercent() * 100), characterStats.MagicResistance.GetMagicEffectiveHealthPercent() * 100).AppendLine();
            statsText.AppendFormat("ATTACK SPEED: {0} ({1} + {2}% - {3}% + {4}%)",
                characterStats.AttackSpeed.GetTotal(), characterStats.AttackSpeed.GetInitialBaseValue(),
                characterStats.AttackSpeed.GetPercentBonus(), characterStats.AttackSpeed.GetPercentMalus(),
                characterStats.AttackSpeed.GetMultiplicativePercentBonus()).AppendLine();
            statsText.AppendFormat("COOLDOWN REDUCTION: {0}% ({1}% + {2}%)",
                characterStats.CooldownReduction.GetTotal(), characterStats.CooldownReduction.GetCurrentBaseValue(),
                characterStats.CooldownReduction.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("CRITICAL STRIKE CHANCE: {0}% ({1}% + {2}%)",
                characterStats.CriticalStrikeChance.GetTotal(), characterStats.CriticalStrikeChance.GetCurrentBaseValue(),
                characterStats.CriticalStrikeChance.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("MOVEMENT SPEED: {0} (({1} + {2}) + {3}% - {4}% + {5}%)",
                characterStats.MovementSpeed.GetTotal() * 100, characterStats.MovementSpeed.GetCurrentBaseValue(), characterStats.MovementSpeed.GetFlatBonus(),
                characterStats.MovementSpeed.GetPercentBonus(), characterStats.MovementSpeed.GetBiggestSlow(),
                characterStats.MovementSpeed.GetMultiplicativePercentBonus()).AppendLine();

            statsText.AppendFormat("HEALTH REGENERATION: {0} (({1} + {2}) + {3}% - {4}%)",
                characterStats.HealthRegeneration.GetTotal(), characterStats.HealthRegeneration.GetCurrentBaseValue(), characterStats.HealthRegeneration.GetFlatBonus(),
                characterStats.HealthRegeneration.GetPercentBonus(), characterStats.HealthRegeneration.GetPercentMalus()).AppendLine();
            statsText.AppendFormat("{0} REGENERATION: {1} (({2} + {3}) + {4}%)", characterStats.ResourceType,
                characterStats.ResourceRegeneration.GetTotal(), characterStats.ResourceRegeneration.GetCurrentBaseValue(), characterStats.ResourceRegeneration.GetFlatBonus(),
                characterStats.ResourceRegeneration.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("LETHALITY: {0} (Current value: {1}) ({2} + {3})",
                characterStats.Lethality.GetTotal(), characterStats.Lethality.GetCurrentValue(),
                characterStats.Lethality.GetCurrentBaseValue(), characterStats.Lethality.GetFlatBonus()).AppendLine();
            statsText.AppendFormat("ARMOR PENETRATION PERCENT: {0}% ({1}% -> {2}%)",
                characterStats.ArmorPenetrationPercent.GetTotal(), characterStats.ArmorPenetrationPercent.GetCurrentBaseValue(),
                characterStats.ArmorPenetrationPercent.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("MAGIC PENETRATION FLAT: {0} ({1} + {2})",
                characterStats.MagicPenetrationFlat.GetTotal(), characterStats.MagicPenetrationFlat.GetCurrentBaseValue(),
                characterStats.MagicPenetrationFlat.GetFlatBonus()).AppendLine();
            statsText.AppendFormat("MAGIC PENETRATION PERCENT: {0}% ({1}% -> {2}%)",
                characterStats.MagicPenetrationPercent.GetTotal(), characterStats.MagicPenetrationPercent.GetCurrentBaseValue(),
                characterStats.MagicPenetrationPercent.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("LIFE STEAL: {0}% ({1}% + {2}%)",
                characterStats.LifeSteal.GetTotal(), characterStats.LifeSteal.GetCurrentBaseValue(),
                characterStats.LifeSteal.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("SPELL VAMP: {0}% ({1}% + {2}%)",
                characterStats.SpellVamp.GetTotal(), characterStats.SpellVamp.GetCurrentBaseValue(),
                characterStats.SpellVamp.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("ATTACK RANGE: {0} (({1} + {2}) + {3}%)",
                characterStats.AttackRange.GetTotal() * 100, characterStats.AttackRange.GetCurrentBaseValue(), characterStats.AttackRange.GetFlatBonus(),
                characterStats.AttackRange.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("TENACITY: {0}% (({1}% -> {2}%) - {3}%)",
                characterStats.Tenacity.GetTotal(), characterStats.Tenacity.GetCurrentBaseValue(),
                characterStats.Tenacity.GetPercentBonus(), characterStats.Tenacity.GetPercentMalus()).AppendLine();
        }
        GUILayout.Label(statsText.ToString());
        GUILayout.Label("POSITION: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);
        GUILayout.Label("ROTATION: " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z + ", " + transform.rotation.w);
    }
}
