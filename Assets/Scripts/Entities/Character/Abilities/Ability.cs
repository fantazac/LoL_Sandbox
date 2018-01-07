using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Character character;

    public bool CanStopMovement { get; protected set; }

    protected virtual void Start()
    {
        character = GetComponent<Character>();
    }

    protected abstract void PressedInput();
    protected abstract void UseAbility();
}
