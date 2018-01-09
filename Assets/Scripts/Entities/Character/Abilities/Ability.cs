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

    public abstract void OnPressedInput(Vector3 mousePosition);
    protected abstract void UseAbility(Vector3 destination = default(Vector3));
}

public interface CharacterAbility { }
public interface OtherAbility { }
