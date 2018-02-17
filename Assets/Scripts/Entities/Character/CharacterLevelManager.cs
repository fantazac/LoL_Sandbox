using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelManager : MonoBehaviour
{
    private Character character;

    public int Level { get; private set; }

    private const int MAX_LEVEL = 18;

    public delegate void OnLevelUpHandler(int level);
    public event OnLevelUpHandler OnLevelUp;

    private CharacterLevelManager()
    {
        Level = 1;
    }

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void LevelUp()
    {
        if (Level < MAX_LEVEL)
        {
            character.LevelUIManager.SetLevel(++Level);
            if (OnLevelUp != null)
            {
                OnLevelUp(Level);
            }
        }
    }
}
