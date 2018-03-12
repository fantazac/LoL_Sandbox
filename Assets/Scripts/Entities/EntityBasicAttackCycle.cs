using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBasicAttackCycle : MonoBehaviour
{
    private WaitForSeconds delayPostBasicAttack;

    public bool AttackSpeedCycleIsReady { get; private set; }

    private EntityBasicAttackCycle()
    {
        AttackSpeedCycleIsReady = true;
    }

    public void SetAttackSpeedCycleDuration(float postBasicAttackDuration)
    {
        delayPostBasicAttack = new WaitForSeconds(postBasicAttackDuration);
    }

    public void LockBasicAttack()
    {
        AttackSpeedCycleIsReady = false;
        StartCoroutine(CompleteBasicAttackCycle());
    }

    public void ResetBasicAttack()
    {
        AttackSpeedCycleIsReady = true;
        StopAllCoroutines();
    }

    private IEnumerator CompleteBasicAttackCycle()
    {
        yield return delayPostBasicAttack;

        AttackSpeedCycleIsReady = true;
    }
}
