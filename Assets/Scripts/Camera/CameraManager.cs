using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private int distanceFromBorderToStartMoving = 60;

    [SerializeField]
    private int speed = 28;

    private int screenWidth;
    private int screenHeight;

    private InputManager characterInputManager;

    private bool cameraLockedOnCharacter = true;
    private bool cameraFollowsCharacter = false;

    private Vector3 initialPosition;

    private Vector3 mousePositionOnFrame;

    private void Start()
    {
        characterInputManager = StaticObjects.Champion.InputManager;
        characterInputManager.OnPressedY += SetCameraLock;
        characterInputManager.OnPressedSpace += SetCameraOnCharacter;
        characterInputManager.OnReleasedSpace += SetCameraFree;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        initialPosition = transform.position;
        transform.position += StaticObjects.Champion.transform.position;
        StaticObjects.Champion.MovementManager.CharacterMoved += CharacterMoved;

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
            transform.position = StaticObjects.Champion.transform.position + initialPosition;
        }
    }

    private bool CameraShouldFollowCharacter()
    {
        return cameraLockedOnCharacter || cameraFollowsCharacter;
    }
}
