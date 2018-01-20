using UnityEngine;
using System.Collections;
using System;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private int distanceFromBorderToStartMoving = 60;

    [SerializeField]
    private int speed = 28;

    private int screenWidth;
    private int screenHeight;

    private CharacterInput characterInput;

    private bool cameraLockedOnCharacter = true;
    private bool cameraFollowsCharacter = false;

    private Vector3 initialPosition;

    private Vector3 mousePositionOnFrame;

    private void Start()
    {
        characterInput = StaticObjects.Character.GetComponent<CharacterInput>();
        characterInput.OnPressedY += SetCameraLock;
        characterInput.OnPressedSpace += SetCameraOnCharacter;
        characterInput.OnReleasedSpace += SetCameraFree;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        initialPosition = transform.position;
        transform.position += StaticObjects.Character.transform.position;
        StaticObjects.Character.GetComponent<CharacterMovement>().CharacterMoved += CharacterMoved;

        CharacterMoved();
    }

    private void Update()
    {
        if (!CameraShouldFollowCharacter())
        {
            mousePositionOnFrame = Input.mousePosition;

            if (mousePositionOnFrame.x > (screenWidth - distanceFromBorderToStartMoving))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else if (mousePositionOnFrame.x < distanceFromBorderToStartMoving)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }

            if (mousePositionOnFrame.y > (screenHeight - distanceFromBorderToStartMoving))
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            else if (mousePositionOnFrame.y < distanceFromBorderToStartMoving)
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
            }
        }
    }

    private void SetCameraOnCharacter()
    {
        cameraFollowsCharacter = true;
        CharacterMoved();
    }

    private void SetCameraFree()
    {
        cameraFollowsCharacter = false;
    }

    private void SetCameraLock()
    {
        cameraLockedOnCharacter = !cameraLockedOnCharacter;
        CharacterMoved();
    }

    private void CharacterMoved()
    {
        if (CameraShouldFollowCharacter())
        {
            transform.position = StaticObjects.Character.transform.position + initialPosition;
        }
    }

    private bool CameraShouldFollowCharacter()
    {
        return cameraLockedOnCharacter || cameraFollowsCharacter;
    }
}
