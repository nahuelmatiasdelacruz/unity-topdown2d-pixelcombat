using UnityEngine;
public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string entranceName;

    void Start()
    {
        if(entranceName == SceneManagement.Instance.SceneEntrance)
        {
            PlayerController.Instance.transform.position = transform.position;
            CameraController.Instance.SetPlayerControllerCameraFollow();
        }
        UIFade.Instance.FadeToClear();
    }
}
