using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeed : MonoBehaviour
{
    [SerializeField]
    private float baseMovementSpeed;
    [SerializeField]
    private float currentMovementSpeed;
    [SerializeField]
    private float bonusMovementSpeed;

    private const float MOVEMENT_SPEED_MODIFIER = 0.01f;

    public void SetBaseMovementSpeed(float baseMovementSpeed)
    {
        this.baseMovementSpeed = baseMovementSpeed;
        currentMovementSpeed = baseMovementSpeed;//change this
    }

    public float GetBaseMovementSpeed()
    {
        return baseMovementSpeed;
    }

    public float GetCurrentMovementSpeed()
    {
        return currentMovementSpeed;
    }

    public float GetCurrentMovementSpeedForMovement()
    {
        return currentMovementSpeed * MOVEMENT_SPEED_MODIFIER;
    }

    public float GetBonusMovementSpeed()
    {
        return bonusMovementSpeed;
    }

    public string GetUIText()
    {
        return "MOVEMENT SPEED: " + GetCurrentMovementSpeed() + " (" + GetBaseMovementSpeed() + " + " + GetBonusMovementSpeed() + ")";
    }
}
