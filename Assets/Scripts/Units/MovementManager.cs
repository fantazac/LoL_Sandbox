using UnityEngine;

public abstract class MovementManager : MonoBehaviour
{
    protected virtual void Awake() { }
    protected virtual void Start() { }

    public abstract void StopMovement(bool same = true);
}
