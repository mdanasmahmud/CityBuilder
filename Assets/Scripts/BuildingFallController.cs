using UnityEngine;

public class BuildingFallController : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float fallDuration = 0.3f;
    public GameObject smokeVFXPrefab; // Reference to the smoke VFX prefab

    private float fallTimer = 0f;
    private bool isFalling = false;

    void Start()
    {
        // Set the initial high position for the building
        startPosition = new Vector3(transform.position.x, 8, transform.position.z);
        endPosition = new Vector3(startPosition.x, 0.59f, startPosition.z); // Target position (ground level)
        transform.position = startPosition; // Set the building to the start position
    }

    void Update()
    {
        if (isFalling)
        {
            Fall();
        }
    }

    public void StartFalling()
    {
        isFalling = true;
        fallTimer = 0f; // Reset timer
    }

    void Fall()
    {
        fallTimer += Time.deltaTime;
        float progress = fallTimer / fallDuration;

        // Smoothly move the building downwards using Lerp
        transform.position = Vector3.Lerp(startPosition, endPosition, progress);

        // Stop falling when finished
        if (fallTimer >= fallDuration)
        {
            isFalling = false; // Stop falling after it reaches the ground
            ShowSmokeVFX(); // Show smoke VFX
        }
    }

    void ShowSmokeVFX()
    {
        if (smokeVFXPrefab != null)
        {
            // Instantiate the smoke VFX at the end position
            Instantiate(smokeVFXPrefab, endPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Smoke VFX Prefab is not assigned.");
        }
    }
}
