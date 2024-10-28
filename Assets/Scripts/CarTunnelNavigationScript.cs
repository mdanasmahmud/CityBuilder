using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CarTunnelNavigation : MonoBehaviour
{
    public Transform[] spawnPoints;  // Array to hold the spawn points for the tunnels
    public Transform[] destinationPoints;  // Array to hold the destination points for the tunnels
    public GameObject[] carPrefabs;  // Array of car prefabs
    public Button[] carButtons;  // Buttons for spawning cars
    public TextMeshProUGUI[] levelTexts;  // Array of TextMeshPro components for levels
    public GameObject moneyThrowerObject;  // GameObject that has the MoneyThrower script

    private List<GameObject> activeCars = new List<GameObject>();  // List to track active cars
    private Dictionary<GameObject, NavMeshAgent> carAgents = new Dictionary<GameObject, NavMeshAgent>();  // Track car agents
    private Dictionary<GameObject, float> carStopTimes = new Dictionary<GameObject, float>();  // Track stop times
    private Dictionary<int, int> carLevels = new Dictionary<int, int>();  // Track levels for each button
    private int maxCarsOnStreet = 0;  // Track the max number of cars that should remain on the street

    void Start()
    {
        // Initialize car levels
        for (int i = 0; i < carButtons.Length; i++)
        {
            carLevels[i] = 0;  // Start levels at 0
        }

        // Add button listeners for spawning cars based on the clicked button
        for (int i = 0; i < carButtons.Length; i++)
        {
            int carIndex = i;  // Cache the index for the button
            carButtons[i].onClick.AddListener(() =>
            {
                maxCarsOnStreet++;  // Increase the max number of cars when a button is pressed
                SpawnCar(carIndex); // Spawn the car
                UpdateLevelText(carIndex);  // Update the level text
            });
        }
    }

    void Update()
    {
        // Check all active cars to see if they have reached their destination or have been stopped for too long
        for (int i = activeCars.Count - 1; i >= 0; i--)
        {
            GameObject car = activeCars[i];
            NavMeshAgent navAgent = carAgents[car];

            if (navAgent != null)
            {
                if (navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
                {
                    // Car has reached its destination
                    if (!carStopTimes.ContainsKey(car))
                    {
                        carStopTimes[car] = Time.time;  // Start tracking stop time
                    }
                    else if (Time.time - carStopTimes[car] > 3f)
                    {
                        // Car has been stopped for more than 3 seconds
                        DestroyCarAndRespawn(car);
                    }
                }
                else
                {
                    // Car is moving, reset stop time
                    if (carStopTimes.ContainsKey(car))
                    {
                        carStopTimes.Remove(car);
                    }
                }
            }
        }

        // Make sure there are always the correct number of cars on the street
        MaintainCarCount();
    }

    void SpawnCar(int carIndex)
    {
        // Select a random spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Select a different random destination point
        int destinationIndex;
        do
        {
            destinationIndex = Random.Range(0, destinationPoints.Length);
        } while (destinationIndex == spawnIndex);  // Ensure destination is different from spawn point

        Transform destinationPoint = destinationPoints[destinationIndex];

        // Spawn the car from the selected prefab
        GameObject car = Instantiate(carPrefabs[carIndex], spawnPoint.position, spawnPoint.rotation);
        car.tag = "Car"; // Ensure the car has the "Car" tag
        activeCars.Add(car);  // Add the car to the active cars list

        // Get the NavMeshAgent component and set the destination
        NavMeshAgent navAgent = car.GetComponent<NavMeshAgent>();
        navAgent.SetDestination(destinationPoint.position);

        // Track the NavMeshAgent for this car
        carAgents.Add(car, navAgent);

        // Set up the MoneyThrower script on the car
        MoneyThrower moneyThrower = car.AddComponent<MoneyThrower>();
        moneyThrower.moneyPrefab = moneyThrowerObject.GetComponent<MoneyThrower>().moneyPrefab;
        moneyThrower.sourceObject = car.transform;
        moneyThrower.destinationObject = moneyThrowerObject.GetComponent<MoneyThrower>().destinationObject;
        moneyThrower.throwInterval = moneyThrowerObject.GetComponent<MoneyThrower>().throwInterval;
    }

    public void DestroyCarAndRespawn(GameObject car)
    {
        // Get the car index (the prefab index) from its name
        string carName = car.name.Replace("(Clone)", "").Trim();
        int carIndex = GetCarPrefabIndex(carName);

        // Remove the car from active cars and destroy it
        activeCars.Remove(car);
        carAgents.Remove(car);
        carStopTimes.Remove(car);  // Remove from stop times tracking
        Destroy(car);

        // Respawn the car immediately after destruction
        SpawnCar(carIndex);
    }

    void MaintainCarCount()
    {
        // Ensure that there are always maxCarsOnStreet cars active
        while (activeCars.Count < maxCarsOnStreet)
        {
            // Randomly pick a car prefab to spawn again if one is missing
            int randomCarIndex = Random.Range(0, carPrefabs.Length);
            SpawnCar(randomCarIndex);
        }
    }

    int GetCarPrefabIndex(string carName)
    {
        for (int i = 0; i < carPrefabs.Length; i++)
        {
            if (carPrefabs[i].name == carName)
                return i;
        }
        return 0;  // Default to 0 if the name isn't found
    }

    void UpdateLevelText(int carIndex)
    {
        // Increment the level for the clicked button
        carLevels[carIndex]++;
        // Update the corresponding TextMeshPro component
        levelTexts[carIndex].text = "Level " + carLevels[carIndex];
    }
}



