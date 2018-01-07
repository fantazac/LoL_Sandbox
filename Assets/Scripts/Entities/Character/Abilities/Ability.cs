using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Character character;

    public bool CanStopMovement { get; protected set; }
    public bool OfflineOnly { get; protected set; }

    protected virtual void Start()
    {
        character = GetComponent<Character>();
    }

    public abstract void OnPressedInput();
    protected abstract void UseAbility();
}

public interface CharacterAbility { }
public interface OtherAbility { }
