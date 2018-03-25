using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    private CharacterLevelManager characterLevelManager;
    private CharacterStats characterStats;
    private bool showStats;

    private float regenerationInterval;
    private WaitForSeconds delayRegeneration;

    private CharacterStatsManager()
    {
        regenerationInterval = 0.5f;
        delayRegeneration = new WaitForSeconds(regenerationInterval);
    }

    private void OnEnable()
    {
        characterLevelManager = GetComponent<CharacterLevelManager>();
        characterStats = GetComponent<CharacterStats>();
        showStats = !StaticObjects.OnlineMode || GetComponent<PhotonView>().isMine;
    }

    private void Start()
    {
        characterLevelManager.OnLevelUp += characterStats.Health.OnLevelUp;
        characterLevelManager.OnLevelUp += characterStats.Resource.OnLevelUp;
        characterLevelManager.OnLevelUp += characterStats.AttackDamage.OnLevelUp;
        characterLevelManager.OnLevelUp += characterStats.Armor.OnLevelUp;
        characterLevelManager.OnLevelUp += characterStats.MagicResistance.OnLevelUp;
        characterLevelManager.OnLevelUp += characterStats.AttackSpeed.OnLevelUp;
        characterLevelManager.OnLevelUp += characterStats.HealthRegeneration.OnLevelUp;
        characterLevelManager.OnLevelUp += characterStats.ResourceRegeneration.OnLevelUp;
        characterLevelManager.OnLevelUp += characterStats.Lethality.OnLevelUp;

        if (characterStats.Health != null && characterStats.HealthRegeneration != null)
        {
            if (characterStats.Resource != null && characterStats.ResourceRegeneration != null)
            {
                StartCoroutine(HealthRegenerationAndResourceRegeneration());
            }
            else
            {
                StartCoroutine(HealthRegeneration());
            }
        }
    }

    private IEnumerator HealthRegeneration()
    {
        Health health = characterStats.Health;
        HealthRegeneration healthRegen = characterStats.HealthRegeneration;

        while (true)
        {
            RegenerateHealth(health, healthRegen);

            yield return delayRegeneration;
        }
    }

    private IEnumerator HealthRegenerationAndResourceRegeneration()
    {
        Health health = characterStats.Health;
        HealthRegeneration healthRegen = characterStats.HealthRegeneration;
        Resource resource = characterStats.Resource;
        ResourceRegeneration resourceRegen = characterStats.ResourceRegeneration;

        while (true)
        {
            RegenerateHealth(health, healthRegen);
            RegenerateResource(resource, resourceRegen);

            yield return delayRegeneration;
        }
    }

    private void RegenerateHealth(Health health, HealthRegeneration healthRegen)
    {
        if (health.GetTotal() > health.GetCurrentValue())
        {
            health.Restore(healthRegen.GetTotal() * 0.1f);
        }
    }

    private void RegenerateResource(Resource resource, ResourceRegeneration resourceRegen)
    {
        if (resource.GetTotal() > resource.GetCurrentValue())
        {
            resource.Restore(resourceRegen.GetTotal() * 0.1f);
        }
    }

    private void OnGUI()
    {
        if (showStats)
        {
            if (StaticObjects.OnlineMode)
            {
                GUILayout.Label("");//ping goes there in online
            }
            GUILayout.Label(characterStats.Health.GetUIText());
            GUILayout.Label(characterStats.Resource.GetUIText());

            GUILayout.Label(characterStats.AttackDamage.GetUIText());
            GUILayout.Label(characterStats.AbilityPower.GetUIText());
            GUILayout.Label(characterStats.Armor.GetUIText());
            GUILayout.Label(characterStats.MagicResistance.GetUIText());
            GUILayout.Label(characterStats.AttackSpeed.GetUIText());
            GUILayout.Label(characterStats.CooldownReduction.GetUIText());
            GUILayout.Label(characterStats.CriticalStrikeChance.GetUIText());
            GUILayout.Label(characterStats.MovementSpeed.GetUIText());

            GUILayout.Label(characterStats.HealthRegeneration.GetUIText());
            GUILayout.Label(characterStats.ResourceRegeneration.GetUIText());
            GUILayout.Label(characterStats.Lethality.GetUIText());
            GUILayout.Label(characterStats.ArmorPenetrationPercent.GetUIText());
            GUILayout.Label(characterStats.MagicPenetrationFlat.GetUIText());
            GUILayout.Label(characterStats.MagicPenetrationPercent.GetUIText());
            GUILayout.Label(characterStats.AttackRange.GetUIText());
            GUILayout.Label(characterStats.LifeSteal.GetUIText());
            GUILayout.Label(characterStats.SpellVamp.GetUIText());
            GUILayout.Label(characterStats.Tenacity.GetUIText());

            GUILayout.Label("POSITION: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);
            GUILayout.Label("ROTATION: " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z + ", " + transform.rotation.w);
        }
    }
}
