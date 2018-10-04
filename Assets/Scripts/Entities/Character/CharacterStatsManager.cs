using System.Collections;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    private CharacterLevelManager characterLevelManager;
    private CharacterStats characterStats;

    private float regenerationInterval;
    private WaitForSeconds delayRegeneration;

    private CharacterStatsManager()
    {
        regenerationInterval = 0.5f;
        delayRegeneration = new WaitForSeconds(regenerationInterval);
    }

    private void Start()
    {
        characterLevelManager = GetComponent<CharacterLevelManager>();
        characterStats = GetComponent<CharacterStats>();

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
}
