using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxOffset = -0.15f;
    private Camera cam;
    private Vector2 startPos;
    private Vector2 Travel => (Vector2)cam.transform.position - startPos;

    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = startPos + Travel * parallaxOffset;
    }
}
