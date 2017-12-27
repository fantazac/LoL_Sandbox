using UnityEngine;
using System.Collections;

public class MovementCapsule : MonoBehaviour
{
    private float downSpeed = 0.06f;

    private void Start()
    {
        StartCoroutine(MakeCapsuleDisapear());
    }

    private IEnumerator MakeCapsuleDisapear()
    {
        while (transform.position.y > -1)
        {
            transform.position += Vector3.down * downSpeed;

            yield return null;
        }

        Destroy(gameObject);
    }
}
