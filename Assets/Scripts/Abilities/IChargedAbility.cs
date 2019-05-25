using UnityEngine;

public interface IChargedAbility
{
    bool CanBeCast(Vector3 mousePosition);
    Vector3 GetDestination();
    void UseChargedAbility(Vector3 destination);
}
