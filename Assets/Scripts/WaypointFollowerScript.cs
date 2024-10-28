using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float waypointReachedDistance = 0.1f;

    private WaypointManager waypointManager;
    private Vector3 currentTarget;
    private bool hasTarget = false;
    private int currentWaypointIndex = -1;  // Track the current waypoint index

    void Start()
    {
        waypointManager = FindObjectOfType<WaypointManager>();
        if (waypointManager == null)
        {
            Debug.LogError("No WaypointManager found in the scene!");
            return;
        }
        SetNewTarget();
    }

    void Update()
    {
        if (!hasTarget) return;

        // Check if we've reached the current target
        if (Vector3.Distance(transform.position, currentTarget) < waypointReachedDistance)
        {
            SetNewTarget();
        }

        // Move towards target
        Vector3 direction = (currentTarget - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotate towards movement direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void SetNewTarget()
    {
        // Update to the next waypoint index
        currentWaypointIndex = (currentWaypointIndex + 1) % waypointManager.GetWaypointCount();
        currentTarget = waypointManager.GetNextWaypoint(currentWaypointIndex);
        hasTarget = true;
    }
}
