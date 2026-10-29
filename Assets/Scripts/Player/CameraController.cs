using UnityEngine;
using Unity.Cinemachine;
public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCameraBase cinemachineVirtualCamera;

    void Start()
    {
        SetPlayerControllerCameraFollow();
    }

    public void SetPlayerControllerCameraFollow()
    {
        cinemachineVirtualCamera = FindAnyObjectByType<CinemachineVirtualCameraBase>();
        cinemachineVirtualCamera.Follow = FindAnyObjectByType<PlayerController>().transform;
    }
}
