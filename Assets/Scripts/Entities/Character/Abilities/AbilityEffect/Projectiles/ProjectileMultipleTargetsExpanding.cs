using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMultipleTargetsExpanding : ProjectileMultipleTargets
{
    private float initialXScale;
    private float finalXScale;
    private float toGrow;

    protected void Start()
    {
        initialXScale = transform.localScale.x;
        finalXScale = (range * 0.5f);
        toGrow = finalXScale - initialXScale;

        StartCoroutine(ExpandWidth());
    }

    private IEnumerator ExpandWidth()
    {
        while (transform.localScale.x < finalXScale)
        {
            yield return null;

            float xValue = (Vector3.Distance(transform.position, initialPosition) / range) * toGrow + initialXScale;
            transform.localScale = Vector3.right * xValue + Vector3.up * transform.localScale.y + Vector3.forward * transform.localScale.z;
        }
    }
}
