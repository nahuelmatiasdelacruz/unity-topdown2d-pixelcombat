using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollow : MonoBehaviour
{
    void Update()
    {
        FaceMouse();
    }
    private void FaceMouse()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector3 mousePositionInCamera = new Vector3 (mouseScreenPosition.x, mouseScreenPosition.y, -Camera.main.transform.position.z);

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePositionInCamera);

        Vector2 direction = mouseWorldPosition - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
