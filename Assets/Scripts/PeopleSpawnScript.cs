using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PeopleSpawner : MonoBehaviour
{
    public GameObject[] peoplePrefabs;  // Array of people prefabs
    public Button spawnButton;  // Button for spawning people
    public TextMeshProUGUI levelText;  // Text to display the number of spawned people
    public WaypointManager waypointManager;  // Reference to the WaypointManager

    public GameObject moneyThrowerObject;  // Reference to an object with MoneyThrower component for settings

    private int peopleSpawned = 0;  // Counter to track the number of people spawned

    void Start()
    {
        // Set up the button listener to spawn a person when clicked
        spawnButton.onClick.AddListener(SpawnPerson);
    }

    void SpawnPerson()
    {
        // Select a random people prefab to spawn
        int prefabIndex = Random.Range(0, peoplePrefabs.Length);
        GameObject personPrefab = peoplePrefabs[prefabIndex];

        // Instantiate the person at a random spawn point from the WaypointManager
        Vector3 spawnPoint = waypointManager.cornerPoints[Random.Range(0, waypointManager.cornerPoints.Length)].transform.position;
        GameObject newPerson = Instantiate(personPrefab, spawnPoint, Quaternion.identity);

        // Attach the WaypointFollower script to the newly spawned person
        WaypointFollower follower = newPerson.AddComponent<WaypointFollower>();

        // Attach the MoneyThrower script to the newly spawned person
        MoneyThrower moneyThrower = newPerson.AddComponent<MoneyThrower>();
        moneyThrower.moneyPrefab = moneyThrowerObject.GetComponent<MoneyThrower>().moneyPrefab;
        moneyThrower.sourceObject = newPerson.transform;  // The person is the source
        moneyThrower.destinationObject = moneyThrowerObject.GetComponent<MoneyThrower>().destinationObject;  // Destination can be road or target
        moneyThrower.throwInterval = moneyThrowerObject.GetComponent<MoneyThrower>().throwInterval;

        // Increment the people spawned counter and update the UI text
        peopleSpawned++;
        UpdateLevelText();
    }

    void UpdateLevelText()
    {
        levelText.text = "Level " + peopleSpawned;
    }
}
