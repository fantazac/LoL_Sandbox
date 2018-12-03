using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private int distanceFromBorderToStartMoving = 60;

    [SerializeField]
    private int speed = 28;

    private int screenWidth;
    private int screenHeight;

    private InputManager inputManager;

    private bool cameraLockedOnChampion = true;
    private bool cameraFollowsChampion = false;

    private Vector3 initialPosition;

    private Vector3 mousePositionOnFrame;

    private void Start()
    {
        inputManager = StaticObjects.Champion.InputManager;
        inputManager.OnPressedY += SetCameraLock;
        inputManager.OnPressedSpace += SetCameraOnChampion;
        inputManager.OnReleasedSpace += SetCameraFree;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        initialPosition = transform.position;
        transform.position += StaticObjects.Champion.transform.position;
        StaticObjects.Champion.ChampionMovementManager.ChampionMoved += ChampionMoved;

        ChampionMoved();
    }

    private void Update()
    {
        if (!CameraShouldFollowChampion())
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

    private void SetCameraOnChampion()
    {
        cameraFollowsChampion = true;
        ChampionMoved();
    }

    private void SetCameraFree()
    {
        cameraFollowsChampion = false;
    }

    private void SetCameraLock()
    {
        cameraLockedOnChampion = !cameraLockedOnChampion;
        ChampionMoved();
    }

    private void ChampionMoved()
    {
        if (CameraShouldFollowChampion())
        {
            transform.position = StaticObjects.Champion.transform.position + initialPosition;
        }
    }

    private bool CameraShouldFollowChampion()
    {
        return cameraLockedOnChampion || cameraFollowsChampion;
    }
}
