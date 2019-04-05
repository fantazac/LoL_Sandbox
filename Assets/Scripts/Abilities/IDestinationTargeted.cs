using UnityEngine;

public interface IDestinationTargeted
{
    bool CanBeCast(Vector3 destination);
    void UseAbility(Vector3 destination);
    Vector3 GetDestination();
}
