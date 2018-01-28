using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionManager : MonoBehaviour
{
    private Character character;

    private bool bufferedPositionMovement;
    private bool bufferedUnitMovement;

    private Ability bufferedPositionTargetedAbility;
    private Vector3 bufferedPosition;

    private Ability bufferedUnitTargetedAbility;
    private Entity bufferedUnit;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    public void ResetBufferedAction()
    {
        if (bufferedPositionMovement)
        {
            ResetBufferedPositionMovement();
        }
        if (bufferedUnitMovement)
        {
            ResetBufferedUnitMovement();
        }
        if (bufferedPositionTargetedAbility != null)
        {
            ResetBufferedPositionTargetedAbility();
        }
        if (bufferedUnitTargetedAbility != null)
        {
            ResetBufferedUnitTargetedAbility();
        }
    }

    public void SetPositionMovementInQueue(Vector3 destination)
    {
        bufferedPositionMovement = true;
        bufferedPosition = destination;
        if (bufferedUnitMovement)
        {
            ResetBufferedUnitMovement();
        }
        if (bufferedPositionTargetedAbility != null)
        {
            ResetBufferedPositionTargetedAbility();
        }
        if (bufferedUnitTargetedAbility != null)
        {
            ResetBufferedUnitTargetedAbility();
        }
    }

    public void SetUnitMovementInQueue(Entity target)
    {
        bufferedUnitMovement = true;
        bufferedUnit = target;
        if (bufferedPositionMovement)
        {
            ResetBufferedPositionMovement();
        }
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
        if (bufferedPositionMovement)
        {
            ResetBufferedPositionMovement();
        }
        if (bufferedUnitMovement)
        {
            ResetBufferedUnitMovement();
        }
        if (bufferedUnitTargetedAbility != null)
        {
            ResetBufferedUnitTargetedAbility();
        }
    }

    public void SetUnitTargetedAbilityInQueue(Ability unitTargetedAbility, Entity target)
    {
        bufferedUnitTargetedAbility = unitTargetedAbility;
        bufferedUnit = target;
        if (bufferedPositionMovement)
        {
            ResetBufferedPositionMovement();
        }
        if (bufferedUnitMovement)
        {
            ResetBufferedUnitMovement();
        }
        if (bufferedPositionTargetedAbility != null)
        {
            ResetBufferedPositionTargetedAbility();
        }
    }

    public void UseBufferedAction()
    {
        if (bufferedPositionMovement)
        {
            character.CharacterMovement.SetMoveTowardsPoint(bufferedPosition);
            ResetBufferedPositionMovement();
        }
        else if (bufferedUnitMovement)
        {
            character.CharacterMovement.SetMoveTowardsTarget(bufferedUnit, character.CharacterStatsController.GetCurrentAttackRange());// TODO : Test behavior with Soraka W for allies
            ResetBufferedUnitMovement();
        }
        else if (bufferedPositionTargetedAbility != null)
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

    private void ResetBufferedPositionMovement()
    {
        bufferedPositionMovement = false;
    }

    private void ResetBufferedUnitMovement()
    {
        bufferedUnitMovement = false;
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
