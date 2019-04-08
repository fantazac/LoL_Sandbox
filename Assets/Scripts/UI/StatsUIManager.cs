using System.Text;
using UnityEngine;

public class StatsUIManager : MonoBehaviour
{
    private CharacterStatsManager characterStatsManager;

    private void Start()
    {
        characterStatsManager = GetComponent<CharacterStatsManager>();
    }

    private void OnGUI()
    {
        if (StaticObjects.OnlineMode)
        {
            GUILayout.Label(""); //ping goes there in online mode
        }

        StringBuilder statsText = new StringBuilder();
        if (!Input.GetKey(KeyCode.C))
        {
            /*statsText.AppendFormat("HEALTH: {0} / {1}", characterStatsManager.Health.GetCurrentValue(), characterStatsManager.Health.GetTotal()).AppendLine();
            statsText.AppendFormat("{0}: {1} / {2}", characterStatsManager.ResourceType, characterStatsManager.Resource.GetCurrentValue(), characterStatsManager.Resource.GetTotal()).AppendLine();

            statsText.AppendFormat("ATTACK DAMAGE: {0}", characterStatsManager.AttackDamage.GetTotal()).AppendLine();
            statsText.AppendFormat("ABILITY POWER: {0}", characterStatsManager.AbilityPower.GetTotal()).AppendLine();
            statsText.AppendFormat("ARMOR: {0}", characterStatsManager.Armor.GetTotal()).AppendLine();
            statsText.AppendFormat("MAGIC RESISTANCE: {0}", characterStatsManager.MagicResistance.GetTotal()).AppendLine();
            statsText.AppendFormat("ATTACK SPEED: {0:0.00}", characterStatsManager.AttackSpeed.GetTotal()).AppendLine();
            statsText.AppendFormat("COOLDOWN REDUCTION: {0}%", characterStatsManager.CooldownReduction.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("CRITICAL STRIKE CHANCE: {0}%", characterStatsManager.CriticalStrikeChance.GetTotal()).AppendLine();
            statsText.AppendFormat("MOVEMENT SPEED: {0}", characterStatsManager.MovementSpeed.GetTotal() * 100).AppendLine();

            statsText.AppendFormat("HEALTH REGENERATION: {0}", characterStatsManager.HealthRegeneration.GetTotal()).AppendLine();
            statsText.AppendFormat("{0} REGENERATION: {1}", characterStatsManager.ResourceType, characterStatsManager.ResourceRegeneration.GetTotal()).AppendLine();
            statsText.AppendFormat("LETHALITY: {0} (Current value: {1})", characterStatsManager.Lethality.GetTotal(), characterStatsManager.Lethality.GetCurrentValue()).AppendLine();
            statsText.AppendFormat("ARMOR PENETRATION PERCENT: {0}%", characterStatsManager.ArmorPenetrationPercent.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("MAGIC PENETRATION FLAT: {0} ", characterStatsManager.MagicPenetrationFlat.GetTotal()).AppendLine();
            statsText.AppendFormat("MAGIC PENETRATION PERCENT: {0}%", characterStatsManager.MagicPenetrationPercent.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("LIFE STEAL: {0}%", characterStatsManager.LifeSteal.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("SPELL VAMP: {0}%", characterStatsManager.SpellVamp.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("ATTACK RANGE: {0}", characterStatsManager.AttackRange.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("TENACITY: {0}%", characterStatsManager.Tenacity.GetTotal() * 100).AppendLine();

            statsText.AppendFormat("CRITICAL STRIKE DAMAGE: {0}%", characterStatsManager.CriticalStrikeDamage.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("CRITICAL STRIKE DAMAGE REDUCTION: {0}%", characterStatsManager.CriticalStrikeDamageReduction.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("PHYSICAL DAMAGE MODIFIER: {0}%", characterStatsManager.PhysicalDamageModifier.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("MAGIC DAMAGE MODIFIER: {0}%", characterStatsManager.MagicDamageModifier.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("PHYSICAL DAMAGE RECEIVED MODIFIER: {0}%", characterStatsManager.PhysicalDamageReceivedModifier.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("MAGIC DAMAGE RECEIVED MODIFIER: {0}%", characterStatsManager.MagicDamageReceivedModifier.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("HEAL AND SHIELD POWER: {0}%", characterStatsManager.HealAndShieldPower.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("SLOW RESISTANCE: {0}%", characterStatsManager.SlowResistance.GetTotal() * 100).AppendLine();*/
        }
        else
        {
            statsText.AppendFormat("HEALTH: {0} / {1} (({2} + {3}) + {4}%)",
                characterStatsManager.Health.GetCurrentValue(), characterStatsManager.Health.GetTotal(), characterStatsManager.Health.GetCurrentBaseValue(),
                characterStatsManager.Health.GetFlatBonus(), characterStatsManager.Health.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("{0}: {1} / {2} (({3} + {4}) + {5}%)", characterStatsManager.ResourceType,
                characterStatsManager.Resource.GetCurrentValue(), characterStatsManager.Resource.GetTotal(), characterStatsManager.Resource.GetCurrentBaseValue(),
                characterStatsManager.Resource.GetFlatBonus(), characterStatsManager.Resource.GetPercentBonus()).AppendLine();

            statsText.AppendFormat("ATTACK DAMAGE: {0} (({1} + {2}) + {3}% - {4})",
                characterStatsManager.AttackDamage.GetTotal(), characterStatsManager.AttackDamage.GetCurrentBaseValue(), characterStatsManager.AttackDamage.GetFlatBonus(),
                characterStatsManager.AttackDamage.GetPercentBonus(), characterStatsManager.AttackDamage.GetFlatMalus()).AppendLine();
            statsText.AppendFormat("ABILITY POWER: {0} ({1} + {2}%)",
                characterStatsManager.AbilityPower.GetTotal(), characterStatsManager.AbilityPower.GetFlatBonus(),
                characterStatsManager.AbilityPower.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("ARMOR: {0} ((({1} + {2}) + {3}% - {4}) - {5}%)  Takes {6}% reduced physical damage (Eff HP: {7}%)",
                characterStatsManager.Armor.GetTotal(), characterStatsManager.Armor.GetCurrentBaseValue(), characterStatsManager.Armor.GetFlatBonus(),
                characterStatsManager.Armor.GetPercentBonus(), characterStatsManager.Armor.GetFlatMalus(), characterStatsManager.Armor.GetPercentMalus(),
                (int)Mathf.Round(characterStatsManager.Armor.GetDamageReductionPercent() * 100), characterStatsManager.Armor.GetEffectiveHealthPercent() * 100).AppendLine();
            statsText.AppendFormat("MAGIC RESISTANCE: {0} ((({1} + {2}) + {3}% - {4}) - {5}%) Takes {6}% reduced magic damage (Eff HP: {7}%)",
                characterStatsManager.MagicResistance.GetTotal(), characterStatsManager.MagicResistance.GetCurrentBaseValue(),
                characterStatsManager.MagicResistance.GetFlatBonus(),
                characterStatsManager.MagicResistance.GetPercentBonus(), characterStatsManager.MagicResistance.GetFlatMalus(),
                characterStatsManager.MagicResistance.GetPercentMalus(),
                (int)Mathf.Round(characterStatsManager.MagicResistance.GetDamageReductionPercent() * 100),
                characterStatsManager.MagicResistance.GetEffectiveHealthPercent() * 100).AppendLine();
            statsText.AppendFormat("ATTACK SPEED: {0} ({1} + {2}% - {3}% + {4}%)",
                characterStatsManager.AttackSpeed.GetTotal(), characterStatsManager.AttackSpeed.GetInitialBaseValue(),
                characterStatsManager.AttackSpeed.GetPercentBonus(), characterStatsManager.AttackSpeed.GetPercentMalus(),
                characterStatsManager.AttackSpeed.GetMultiplicativePercentBonus()).AppendLine();
            statsText.AppendFormat("COOLDOWN REDUCTION: {0}%", characterStatsManager.CooldownReduction.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("CRITICAL STRIKE CHANCE: {0}%", characterStatsManager.CriticalStrikeChance.GetTotal()).AppendLine();
            statsText.AppendFormat("MOVEMENT SPEED: {0} (({1} + {2}) + {3}% - ({4}% * {5}%) + {6}%)",
                characterStatsManager.MovementSpeed.GetTotal() * 100, characterStatsManager.MovementSpeed.GetCurrentBaseValue(),
                characterStatsManager.MovementSpeed.GetFlatBonus(),
                characterStatsManager.MovementSpeed.GetPercentBonus(), characterStatsManager.MovementSpeed.GetBiggestSlow(),
                characterStatsManager.SlowResistance.GetTotal() * 100,
                characterStatsManager.MovementSpeed.GetMultiplicativePercentBonus()).AppendLine();

            statsText.AppendFormat("HEALTH REGENERATION: {0} (({1} + {2}) + {3}% - {4}%)",
                characterStatsManager.HealthRegeneration.GetTotal(), characterStatsManager.HealthRegeneration.GetCurrentBaseValue(),
                characterStatsManager.HealthRegeneration.GetFlatBonus(),
                characterStatsManager.HealthRegeneration.GetPercentBonus(), characterStatsManager.HealthRegeneration.GetPercentMalus()).AppendLine();
            statsText.AppendFormat("{0} REGENERATION: {1} (({2} + {3}) + {4}%)", characterStatsManager.ResourceType,
                characterStatsManager.ResourceRegeneration.GetTotal(), characterStatsManager.ResourceRegeneration.GetCurrentBaseValue(),
                characterStatsManager.ResourceRegeneration.GetFlatBonus(),
                characterStatsManager.ResourceRegeneration.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("LETHALITY: {0} (Current value: {1})", characterStatsManager.Lethality.GetTotal(), characterStatsManager.Lethality.GetCurrentValue())
                .AppendLine();
            statsText.AppendFormat("ARMOR PENETRATION PERCENT: {0}%", characterStatsManager.ArmorPenetrationPercent.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("MAGIC PENETRATION FLAT: {0}", characterStatsManager.MagicPenetrationFlat.GetTotal()).AppendLine();
            statsText.AppendFormat("MAGIC PENETRATION PERCENT: {0}%", characterStatsManager.MagicPenetrationPercent.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("LIFE STEAL: {0}%", characterStatsManager.LifeSteal.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("SPELL VAMP: {0}%", characterStatsManager.SpellVamp.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("ATTACK RANGE: {0} (({1} + {2}) + {3}%)",
                characterStatsManager.AttackRange.GetTotal() * 100, characterStatsManager.AttackRange.GetCurrentBaseValue(), characterStatsManager.AttackRange.GetFlatBonus(),
                characterStatsManager.AttackRange.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("TENACITY: {0}%", characterStatsManager.Tenacity.GetTotal() * 100).AppendLine();

            statsText.AppendFormat("CRITICAL STRIKE DAMAGE: {0}% ({1}% + {2}%)",
                characterStatsManager.CriticalStrikeDamage.GetTotal() * 100, characterStatsManager.CriticalStrikeDamage.GetCurrentBaseValue() * 100,
                characterStatsManager.CriticalStrikeDamage.GetPercentBonus()).AppendLine();
            statsText.AppendFormat("CRITICAL STRIKE DAMAGE REDUCTION: {0}%", characterStatsManager.CriticalStrikeDamageReduction.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("PHYSICAL DAMAGE MODIFIER: {0}% ({1}% * {2}%)",
                characterStatsManager.PhysicalDamageModifier.GetTotal() * 100, characterStatsManager.PhysicalDamageModifier.GetPercentBonus(),
                characterStatsManager.PhysicalDamageModifier.GetPercentMalus()).AppendLine();
            statsText.AppendFormat("MAGIC DAMAGE MODIFIER: {0}% ({1}% * {2}%)",
                characterStatsManager.MagicDamageModifier.GetTotal() * 100, characterStatsManager.MagicDamageModifier.GetPercentBonus(),
                characterStatsManager.MagicDamageModifier.GetPercentMalus()).AppendLine();
            statsText.AppendFormat("PHYSICAL DAMAGE RECEIVED MODIFIER: {0}% ({1}% * {2}%)",
                characterStatsManager.PhysicalDamageReceivedModifier.GetTotal() * 100, characterStatsManager.PhysicalDamageReceivedModifier.GetPercentBonus(),
                characterStatsManager.PhysicalDamageReceivedModifier.GetPercentMalus()).AppendLine();
            statsText.AppendFormat("MAGIC DAMAGE RECEIVED MODIFIER: {0}% ({1}% * {2}%)",
                characterStatsManager.MagicDamageReceivedModifier.GetTotal() * 100, characterStatsManager.MagicDamageReceivedModifier.GetPercentBonus(),
                characterStatsManager.MagicDamageReceivedModifier.GetPercentMalus()).AppendLine();
            statsText.AppendFormat("HEAL AND SHIELD POWER: {0}%", characterStatsManager.HealAndShieldPower.GetTotal() * 100).AppendLine();
            statsText.AppendFormat("SLOW RESISTANCE: {0}%", characterStatsManager.SlowResistance.GetTotal() * 100).AppendLine();

            GUILayout.Label(statsText.ToString());
            GUILayout.Label("POSITION: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);
            GUILayout.Label("ROTATION: " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z + ", " + transform.rotation.w);
        }
    }
}
