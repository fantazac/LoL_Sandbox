﻿using UnityEngine;

public class BufferedAbilityManager : MonoBehaviour
{
    private Ability bufferedPositionTargetedAbility;
    private Vector3 bufferedPosition;

    private Ability bufferedUnitTargetedAbility;
    private Unit bufferedUnit;

    public Ability GetBufferedAbility()
    {
        return bufferedPositionTargetedAbility != null ? bufferedPositionTargetedAbility : bufferedUnitTargetedAbility;
    }

    public void ResetBufferedAbility()
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

    public void BufferPositionTargetedAbility(Ability positionTargetedAbility, Vector3 destination)
    {
        bufferedPositionTargetedAbility = positionTargetedAbility;
        bufferedPosition = destination;

        if (bufferedUnitTargetedAbility != null)
        {
            ResetBufferedUnitTargetedAbility();
        }
    }

    public void BufferUnitTargetedAbility(Ability unitTargetedAbility, Unit target)
    {
        bufferedUnitTargetedAbility = unitTargetedAbility;
        bufferedUnit = target;

        if (bufferedPositionTargetedAbility != null)
        {
            ResetBufferedPositionTargetedAbility();
        }
    }

    public void UseBufferedAbility()
    {
        if (bufferedPositionTargetedAbility != null)
        {
            Ability bufferedAbility = bufferedPositionTargetedAbility;
            ResetBufferedPositionTargetedAbility();
            bufferedAbility.UseAbility(bufferedPosition);
        }
        else if (bufferedUnitTargetedAbility != null)
        {
            Ability bufferedAbility = bufferedUnitTargetedAbility;
            ResetBufferedUnitTargetedAbility();
            bufferedAbility.UseAbility(bufferedUnit);
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
