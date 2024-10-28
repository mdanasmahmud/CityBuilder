using UnityEngine;

public class MobileCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float smoothness = 10f;

    [Header("Boundaries")]
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;
    [SerializeField] private float minZ = -50f;
    [SerializeField] private float maxZ = 50f;

    private Camera cam;
    private Vector3 targetPosition;
    private Vector2 lastPanPosition;

    private void Start()
    {
        cam = GetComponent<Camera>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        HandleTouchInput();
        UpdateCameraTransform();
    }

    private void HandleTouchInput()
    {
        // Single touch - panning
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // Calculate delta movement in screen space (Vector2)
                Vector2 touchDeltaPosition = touch.position - lastPanPosition;

                // Convert screen movement to world space movement
                Vector3 move = new Vector3(-touchDeltaPosition.x, 0, -touchDeltaPosition.y) * panSpeed * Time.deltaTime;
                move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * move;

                targetPosition += move;
                targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
                targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

                lastPanPosition = touch.position;
            }
        }
    }

    private void UpdateCameraTransform()
    {
        // Smoothly interpolate position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);
    }

    // Optional: Reset camera to initial position
    public void ResetCamera(Vector3 position)
    {
        targetPosition = position;
    }
}
