using UnityEngine;
using System.Collections;

public class MovementCapsule : MonoBehaviour
{
    private readonly float downSpeed;

    private MovementCapsule()
    {
        downSpeed = 0.06f;
    }

    private void Start()
    {
        StartCoroutine(MakeCapsuleDisappear());
    }

    private IEnumerator MakeCapsuleDisappear()
    {
        while (transform.position.y > -1)
        {
            transform.position += Vector3.down * downSpeed;

            yield return null;
        }

        Destroy(gameObject);
    }
}
