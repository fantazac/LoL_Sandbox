using System.Collections;
using UnityEngine;

public class CharacterAutoAttack : MonoBehaviour
{
    private Character character;
    private AttackRange attackRange;
    private float biggerAttackRange;

    private IEnumerator currentAutoAttackCoroutine;

    private bool autoAttackEnabled;
    private AbilityAffectedUnitType affectedUnitType;

    private CharacterAutoAttack()
    {
        autoAttackEnabled = true;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;

        biggerAttackRange = 750 * StaticObjects.MultiplyingFactor;//TODO: try to figure out actual value
    }

    private void Start()
    {
        character = GetComponent<Character>();
        attackRange = character.EntityStats.AttackRange;
        EnableAutoAttack();
    }

    public void EnableAutoAttack(bool forceEnable = false)
    {
        StopAutoAttack();
        if (autoAttackEnabled || forceEnable)
        {
            currentAutoAttackCoroutine = AutoAttack();
            StartCoroutine(currentAutoAttackCoroutine);
        }
    }

    public void EnableAutoAttackWithBiggerRange(bool forceEnable = false)
    {
        StopAutoAttack();
        if (autoAttackEnabled || forceEnable)
        {
            currentAutoAttackCoroutine = AutoAttackWithBiggerRange();
            StartCoroutine(currentAutoAttackCoroutine);
        }
    }

    public void StopAutoAttack()
    {
        if (currentAutoAttackCoroutine != null)
        {
            StopCoroutine(currentAutoAttackCoroutine);
            currentAutoAttackCoroutine = null;
        }
    }

    private IEnumerator AutoAttack()
    {
        if (!character)
        {
            yield return null;
        }

        Vector3 groundPosition;

        Entity autoAttackTarget = null;
        while (true)
        {
            if (CanUseAutoAttack())
            {
                if (autoAttackTarget == null || Vector3.Distance(autoAttackTarget.transform.position, transform.position) > attackRange.GetTotal())
                {
                    autoAttackTarget = null;
                    float distance = float.MaxValue;
                    groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
                    foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, attackRange.GetTotal()))
                    {
                        Entity tempEntity = collider.GetComponentInParent<Entity>();
                        if (tempEntity != null && TargetIsValid.CheckIfTargetIsValid(tempEntity, affectedUnitType, character.Team))
                        {
                            float tempDistance = Vector3.Distance(tempEntity.transform.position, transform.position);
                            if (tempDistance < distance)
                            {
                                autoAttackTarget = tempEntity;
                                distance = tempDistance;
                            }
                        }
                    }
                }

                if (autoAttackTarget != null)
                {
                    character.EntityBasicAttack.UseBasicAttackFromAutoAttack(autoAttackTarget);
                }
            }

            yield return null;
        }
    }

    private IEnumerator AutoAttackWithBiggerRange()
    {
        if (!character)
        {
            yield return null;
        }

        Entity autoAttackTarget = null;
        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        while (true)
        {
            if (CanUseAutoAttack())
            {
                float distance = float.MaxValue;
                foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, biggerAttackRange))
                {
                    Entity tempEntity = collider.GetComponentInParent<Entity>();
                    if (tempEntity != null && TargetIsValid.CheckIfTargetIsValid(tempEntity, affectedUnitType, character.Team))
                    {
                        float tempDistance = Vector3.Distance(tempEntity.transform.position, transform.position);
                        if (tempDistance < distance)
                        {
                            autoAttackTarget = tempEntity;
                            distance = tempDistance;
                        }
                    }
                }

                if (autoAttackTarget != null)
                {
                    character.EntityBasicAttack.SetupBasicAttack(autoAttackTarget, false);
                    break;
                }
            }

            yield return null;
        }

        currentAutoAttackCoroutine = null;
    }

    protected bool CanUseAutoAttack()
    {
        return character.EntityStatusManager && character.CharacterAbilityManager.CanUseBasicAttacks() && !character.EntityDisplacementManager.IsBeingDisplaced &&
                character.EntityStatusManager.CanUseBasicAttacks() && (!character.CharacterMovement.IsMoving() || !character.EntityStatusManager.CanUseMovement()) &&
                !character.EntityBasicAttack.AttackIsInQueue && character.EntityBasicAttackCycle.AttackSpeedCycleIsReady;
    }
}
