using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionManager : MonoBehaviour
{
    private Ability bufferedPositionTargetedAbility;
    private Vector3 bufferedPosition;

    private Ability bufferedUnitTargetedAbility;
    private Entity bufferedUnit;

    public void ResetBufferedAction()
    {
        if (bufferedPositionTargetedAbility != null)
        {
            ResetBufferedPositionTargetedAbility();
        }
        if (bufferedUnitTargetedAbility != null)
        {
            ResetBufferedUnitTargetedAbility();
        }
    }

    public void SetPositionTargetedAbilityInQueue(Ability positionTargetedAbility, Vector3 destination)
    {
        bufferedPositionTargetedAbility = positionTargetedAbility;
        bufferedPosition = destination;

        if (bufferedUnitTargetedAbility != null)
        {
            ResetBufferedUnitTargetedAbility();
        }
    }

    public void SetUnitTargetedAbilityInQueue(Ability unitTargetedAbility, Entity target)
    {
        bufferedUnitTargetedAbility = unitTargetedAbility;
        bufferedUnit = target;

        if (bufferedPositionTargetedAbility != null)
        {
            ResetBufferedPositionTargetedAbility();
        }
    }

    public void UseBufferedAction()
    {
        if (bufferedPositionTargetedAbility != null)
        {
            bufferedPositionTargetedAbility.UseAbility(bufferedPosition);
            ResetBufferedPositionTargetedAbility();
        }
        else if (bufferedUnitTargetedAbility != null)
        {
            bufferedUnitTargetedAbility.UseAbility(bufferedUnit);
            ResetBufferedUnitTargetedAbility();
        }
    }

    private void ResetBufferedPositionTargetedAbility()
    {
        bufferedPositionTargetedAbility = null;
    }

    private void ResetBufferedUnitTargetedAbility()
    {
        bufferedUnitTargetedAbility = null;
    }
}
