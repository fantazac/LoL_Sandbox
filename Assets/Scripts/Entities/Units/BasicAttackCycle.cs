using System.Collections;
using UnityEngine;

public class BasicAttackCycle : MonoBehaviour
{
    private float delayPostBasicAttackDuration;

    private IEnumerator basicAttackCycleCoroutine;

    public bool AttackSpeedCycleIsReady { get; private set; }

    private BasicAttackCycle()
    {
        AttackSpeedCycleIsReady = true;
    }

    public void SetAttackSpeedCycleDuration(float postBasicAttackDuration)
    {
        delayPostBasicAttackDuration = postBasicAttackDuration;
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
        float currentDelay = 0;
        while (currentDelay < delayPostBasicAttackDuration)
        {
            yield return null;

            currentDelay += Time.deltaTime;
        }

        AttackSpeedCycleIsReady = true;
        basicAttackCycleCoroutine = null;
    }
}
