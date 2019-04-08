using UnityEngine;

public class BufferedAbilityManager : MonoBehaviour
{
    private Ability bufferedPositionTargetedAbility;
    private Vector3 bufferedPosition;

    private Ability bufferedUnitTargetedAbility;
    private Unit bufferedUnit;

    private Ability bufferedAutoTargetedAbility;

    public Ability GetBufferedAbility()
    {
        if (bufferedPositionTargetedAbility)
        {
            return bufferedPositionTargetedAbility;
        }

        return bufferedUnitTargetedAbility ? bufferedUnitTargetedAbility : bufferedAutoTargetedAbility;
    }

    public void ResetBufferedAbility()
    {
        if (bufferedPositionTargetedAbility)
        {
            ResetBufferedPositionTargetedAbility();
        }

        if (bufferedUnitTargetedAbility)
        {
            ResetBufferedUnitTargetedAbility();
        }

        if (bufferedAutoTargetedAbility)
        {
            ResetBufferedAutoTargetedAbility();
        }
    }

    public void BufferPositionTargetedAbility(Ability positionTargetedAbility, Vector3 destination)
    {
        bufferedPositionTargetedAbility = positionTargetedAbility;
        bufferedPosition = destination;

        if (bufferedUnitTargetedAbility)
        {
            ResetBufferedUnitTargetedAbility();
        }

        if (bufferedAutoTargetedAbility)
        {
            ResetBufferedAutoTargetedAbility();
        }
    }

    public void BufferUnitTargetedAbility(Ability unitTargetedAbility, Unit target)
    {
        bufferedUnitTargetedAbility = unitTargetedAbility;
        bufferedUnit = target;

        if (bufferedPositionTargetedAbility)
        {
            ResetBufferedPositionTargetedAbility();
        }

        if (bufferedAutoTargetedAbility)
        {
            ResetBufferedAutoTargetedAbility();
        }
    }

    public void BufferAutoTargetedAbility(Ability autoTargetedAbility)
    {
        bufferedAutoTargetedAbility = autoTargetedAbility;

        if (bufferedPositionTargetedAbility)
        {
            ResetBufferedPositionTargetedAbility();
        }

        if (bufferedUnitTargetedAbility)
        {
            ResetBufferedUnitTargetedAbility();
        }
    }

    public void UseBufferedAbility()
    {
        if (bufferedPositionTargetedAbility)
        {
            Ability bufferedAbility = bufferedPositionTargetedAbility;
            ResetBufferedPositionTargetedAbility();
            ((IDestinationTargeted)bufferedAbility).UseAbility(bufferedPosition);
        }
        else if (bufferedUnitTargetedAbility)
        {
            Ability bufferedAbility = bufferedUnitTargetedAbility;
            ResetBufferedUnitTargetedAbility();
            ((IUnitTargeted)bufferedAbility).UseAbility(bufferedUnit);
        }
        else if (bufferedAutoTargetedAbility)
        {
            Ability bufferedAbility = bufferedAutoTargetedAbility;
            ResetBufferedAutoTargetedAbility();
            ((IAutoTargeted)bufferedAbility).UseAbility();
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

    private void ResetBufferedAutoTargetedAbility()
    {
        bufferedAutoTargetedAbility = null;
    }
}
