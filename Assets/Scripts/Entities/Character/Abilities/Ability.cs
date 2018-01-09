using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Character character;
    protected float castTime;
    protected WaitForSeconds delayCastTime;

    public bool CanStopMovement { get; protected set; }
    public bool OfflineOnly { get; protected set; }
    public bool HasCastTime { get; protected set; }

    public delegate void OnAbilityCastHandler();
    public event OnAbilityCastHandler OnAbilityCast;

    public delegate void OnAbilityCastFinishedHandler();
    public event OnAbilityCastFinishedHandler OnAbilityCastFinished;

    protected virtual void Start()
    {
        character = GetComponent<Character>();
    }

    public abstract void OnPressedInput(Vector3 mousePosition);
    protected abstract void UseAbility(Vector3 destination = default(Vector3));

    protected void StartAbilityCast()
    {
        if(OnAbilityCast != null)
        {
            OnAbilityCast();
        }
    }

    protected void FinishAbilityCast()
    {
        if (OnAbilityCastFinished != null)
        {
            OnAbilityCastFinished();
        }
    }

    protected virtual IEnumerator AbilityWithoutCastTime()
    {
        yield return null;
    }

    protected virtual IEnumerator AbilityWithCastTime()
    {
        yield return null;
    }
}

public interface CharacterAbility { }
public interface OtherAbility { }
