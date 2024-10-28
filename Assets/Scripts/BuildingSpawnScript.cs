using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PoliceBuildingManager : MonoBehaviour
{
    public GameObject[] policeBuildingPrefabs;  // Array of police buildings for each level
    public TextMeshProUGUI policeLevelText;  // Text to display current level
    public Button policeButton;  // Reference to the Police button
    public GameObject smokeVFXPrefab;  // Reference to the smoke VFX prefab
    public Transform cameraTransform;  // Reference to the camera's transform
    public float cameraMoveSpeed = 2f;  // Speed at which the camera moves

    private Vector3 originalCameraOffset;  // To store the initial camera offset
    private int currentLevel = 0;
    private bool isCameraMoving = false;
    private Vector3 targetCameraPosition;
    private MobileCameraController mobileCameraController;  // Reference to the MobileCameraController script

    void Start()
    {
        // Calculate the original offset from the camera to the world origin
        originalCameraOffset = cameraTransform.position - Vector3.zero;  // Assuming original offset from (0,0,0)

        UpdateLevelText();
        policeButton.onClick.AddListener(OnPoliceButtonClick);  // Add listener to the button

        // Get the MobileCameraController component
        mobileCameraController = cameraTransform.GetComponent<MobileCameraController>();
    }

    public void OnPoliceButtonClick()
    {
        if (currentLevel < policeBuildingPrefabs.Length)
        {
            currentLevel++;
            UpdateLevelText();  // Update the level text after incrementing the level
            Vector3 buildingPosition = policeBuildingPrefabs[currentLevel - 1].transform.position;
            MoveCameraToBuilding(buildingPosition, currentLevel);
            policeButton.interactable = false;  // Disable the button
        }
    }

    void MoveCameraToBuilding(Vector3 buildingPosition, int level)
    {
        // Maintain the original camera offset relative to the new building position
        targetCameraPosition = buildingPosition + originalCameraOffset;
        isCameraMoving = true;

        // Disable the MobileCameraController script
        if (mobileCameraController != null)
        {
            mobileCameraController.enabled = false;
        }

        // Coroutine to wait for the camera to move before spawning the building
        StartCoroutine(WaitAndSpawnBuilding(level));
    }

    System.Collections.IEnumerator WaitAndSpawnBuilding(int level)
    {
        // Wait until the camera reaches the target position
        while (Vector3.Distance(cameraTransform.position, targetCameraPosition) > 0.1f)
        {
            yield return null;  // Wait for the next frame
        }

        // Once the camera has moved, spawn the building
        SpawnPoliceBuilding(level);

        // Update the target position in the MobileCameraController script
        if (mobileCameraController != null)
        {
            mobileCameraController.ResetCamera(targetCameraPosition);
            mobileCameraController.enabled = true;
        }

        policeButton.interactable = true;  // Re-enable the button
    }

    void Update()
    {
        // Smoothly move the camera toward the target position
        if (isCameraMoving)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCameraPosition, Time.deltaTime * cameraMoveSpeed);

            // Check if the camera has reached the target
            if (Vector3.Distance(cameraTransform.position, targetCameraPosition) < 0.1f)
            {
                isCameraMoving = false;  // Stop moving the camera
            }
        }
    }

    void SpawnPoliceBuilding(int level)
    {
        if (level <= policeBuildingPrefabs.Length)
        {
            // Spawn the new building at its predefined location
            GameObject newBuilding = Instantiate(policeBuildingPrefabs[level - 1]);

            // Attach the BuildingFallController script to the new building
            BuildingFallController fallController = newBuilding.AddComponent<BuildingFallController>();
            fallController.smokeVFXPrefab = smokeVFXPrefab;  // Pass the smoke VFX prefab
            fallController.StartFalling();
        }
    }

    void UpdateLevelText()
    {
        policeLevelText.text = "Level " + currentLevel;
    }
}
