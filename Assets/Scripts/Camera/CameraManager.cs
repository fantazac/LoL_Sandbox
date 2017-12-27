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

    private Character character;

    private bool cameraLockedOnCharacter = true;
    private bool cameraFollowsCharacter = false;

    private Vector3 initialPosition;

    private Vector3 mousePositionOnFrame;

    private void Start()
    {
        character = StaticObjects.Character;
        character.CharacterInput.OnPressedY += SetCameraLock;
        character.CharacterInput.OnPressedSpace += SetCameraOnCharacter;
        character.CharacterInput.OnReleasedSpace += SetCameraFree;
        
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        initialPosition = transform.position;
        transform.position += character.transform.position;
        character.CharacterMovement.CharacterMoved += CharacterMoved;

        CharacterMoved();
    }

    private void Update()
    {
        if (!CameraShouldFollowCharacter())
        {
            mousePositionOnFrame = Input.mousePosition;

            if(mousePositionOnFrame.x > (screenWidth - distanceFromBorderToStartMoving))
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
            transform.position = character.transform.position + initialPosition;
        }
    }

    private bool CameraShouldFollowCharacter()
    {
        return cameraLockedOnCharacter || cameraFollowsCharacter;
    }
}
