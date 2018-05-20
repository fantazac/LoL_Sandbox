using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAutoAttack : MonoBehaviour
{
    private Character character;
    private AttackRange attackRange;
    private float biggerAttackRangeRatio;

    private bool autoAttackEnabled;
    private AbilityAffectedUnitType affectedUnitType;

    private CharacterAutoAttack()
    {
        autoAttackEnabled = true;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;

        biggerAttackRangeRatio = 1.15f;//TODO: try to figure out actual value
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
            StartCoroutine(AutoAttack());
        }
    }

    public void EnableAutoAttackWithBiggerRange(bool forceEnable = false)
    {
        StopAutoAttack();
        if (autoAttackEnabled || forceEnable)
        {
            StartCoroutine(AutoAttackWithBiggerRange());
        }
    }

    public void StopAutoAttack()
    {
        StopAllCoroutines();
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
            if (character.EntityStatusManager && !character.CharacterAbilityManager.IsUsingAbilityPreventingBasicAttacks() && character.EntityStatusManager.CanUseBasicAttacks() &&
                (!character.CharacterMovement.IsMoving() || !character.EntityStatusManager.CanUseMovement()) &&
                !character.EntityBasicAttack.AttackIsInQueue && character.EntityBasicAttackCycle.AttackSpeedCycleIsReady)
            {
                if (autoAttackTarget == null || Vector3.Distance(autoAttackTarget.transform.position, transform.position) > attackRange.GetTotal())
                {
                    autoAttackTarget = null;
                    float distance = float.MaxValue;
                    groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
                    foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, attackRange.GetTotal()))
                    {
                        Entity tempEntity = collider.GetComponent<Entity>();
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
            if (character.EntityStatusManager && !character.CharacterAbilityManager.IsUsingAbilityPreventingBasicAttacks() && character.EntityStatusManager.CanUseBasicAttacks() &&
                (!character.CharacterMovement.IsMoving() || !character.EntityStatusManager.CanUseMovement()) &&
                !character.EntityBasicAttack.AttackIsInQueue && character.EntityBasicAttackCycle.AttackSpeedCycleIsReady)
            {
                float distance = float.MaxValue;
                foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, attackRange.GetTotal() * biggerAttackRangeRatio))
                {
                    Entity tempEntity = collider.GetComponent<Entity>();
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
    }
}
