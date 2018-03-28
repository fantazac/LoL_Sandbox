using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarPrefab;

    private List<Character> characters;
    private List<GameObject> healthBars;

    private HealthBarManager()
    {
        characters = new List<Character>();
        healthBars = new List<GameObject>();
    }

    private void Start()
    {
        foreach (Character character in FindObjectsOfType<Character>())
        {
            characters.Add(character);

            GameObject healthBar = Instantiate(healthBarPrefab);
            healthBar.GetComponent<HealthBar>().SetupHealthBar(character);
            healthBar.transform.SetParent(transform, false);
            healthBars.Add(healthBar);
        }
    }
}
