using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarPrefab;

    private List<Character> characters;
    private List<HealthBar> healthBars;

    private HealthBarManager()
    {
        characters = new List<Character>();
        healthBars = new List<HealthBar>();
    }

    private void Start()
    {
        foreach (Character character in FindObjectsOfType<Character>())
        {
            SetupHealthBarForCharacter(character);
        }
    }

    public void SetupHealthBarForCharacter(Character character)
    {
        characters.Add(character);

        HealthBar healthBar = Instantiate(healthBarPrefab).GetComponent<HealthBar>();
        healthBar.SetupHealthBar(character);
        healthBar.transform.SetParent(transform, false);
        healthBars.Add(healthBar);
    }

    public void RemoveHealthBarOfDeletedCharacter(Character character)
    {
        foreach (HealthBar healthBar in healthBars)
        {
            if (healthBar.GetCharacter() == character)
            {
                healthBars.Remove(healthBar);
                characters.Remove(character);
                Destroy(healthBar.gameObject);
                break;
            }
        }
    }
}
