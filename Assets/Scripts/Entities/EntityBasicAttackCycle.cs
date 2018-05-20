using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBasicAttackCycle : MonoBehaviour
{
    private WaitForSeconds delayPostBasicAttack;

    private IEnumerator basicAttackCycleCoroutine;

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
        basicAttackCycleCoroutine = CompleteBasicAttackCycle();//Should not have to cancel current coroutine since you cannot basic attack while it's active
        StartCoroutine(basicAttackCycleCoroutine);
    }

    public void ResetBasicAttack()
    {
        AttackSpeedCycleIsReady = true;
        if (basicAttackCycleCoroutine != null)
        {
            StopCoroutine(basicAttackCycleCoroutine);
            basicAttackCycleCoroutine = null;
        }
    }

    private IEnumerator CompleteBasicAttackCycle()
    {
        yield return delayPostBasicAttack;

        AttackSpeedCycleIsReady = true;
        basicAttackCycleCoroutine = null;
    }
}
