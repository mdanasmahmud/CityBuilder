using UnityEngine;
using System.Collections.Generic;

public class WaypointManager : MonoBehaviour
{
    public GameObject[] cornerPoints; // Assign your 4 corner points
    public float waypointSpacing = 2f; // Distance between generated waypoints
    public bool showGizmos = true;

    private List<Vector3> waypoints = new List<Vector3>();

    void Start()
    {
        if (cornerPoints.Length != 4)
        {
            Debug.LogError("Please assign exactly 4 corner points!");
            return;
        }

        GenerateWaypoints();
    }

    void GenerateWaypoints()
    {
        waypoints.Clear(); // Clear any existing waypoints

        // Generate waypoints between each pair of corner points
        for (int i = 0; i < cornerPoints.Length; i++)
        {
            Vector3 startPoint = cornerPoints[i].transform.position;
            Vector3 endPoint = cornerPoints[(i + 1) % cornerPoints.Length].transform.position;

            float distance = Vector3.Distance(startPoint, endPoint);
            int pointsToCreate = Mathf.Max(1, Mathf.FloorToInt(distance / waypointSpacing));

            for (int j = 0; j <= pointsToCreate; j++)
            {
                float t = j / (float)pointsToCreate;
                Vector3 point = Vector3.Lerp(startPoint, endPoint, t);
                waypoints.Add(point);
            }
        }
    }

    // Get the next waypoint sequentially, cycling through the list
    public Vector3 GetNextWaypoint(int currentWaypointIndex)
    {
        if (waypoints.Count == 0) return Vector3.zero;

        int nextIndex = (currentWaypointIndex + 1) % waypoints.Count;
        return waypoints[nextIndex];
    }

    public int GetWaypointCount()
    {
        return waypoints.Count;
    }

    void OnDrawGizmos()
    {
        if (!showGizmos || cornerPoints == null) return;

        // Draw corner points
        Gizmos.color = Color.red;
        foreach (var point in cornerPoints)
        {
            if (point != null)
                Gizmos.DrawWireSphere(point.transform.position, 0.5f);
        }

        // Draw lines between corners
        Gizmos.color = Color.yellow;
        if (cornerPoints.Length == 4)
        {
            for (int i = 0; i < cornerPoints.Length; i++)
            {
                if (cornerPoints[i] != null && cornerPoints[(i + 1) % cornerPoints.Length] != null)
                {
                    Gizmos.DrawLine(
                        cornerPoints[i].transform.position,
                        cornerPoints[(i + 1) % cornerPoints.Length].transform.position
                    );
                }
            }
        }

        // Draw generated waypoints in play mode
        if (Application.isPlaying && waypoints.Count > 0)
        {
            Gizmos.color = Color.blue;
            foreach (var point in waypoints)
            {
                Gizmos.DrawWireSphere(point, 0.2f);
            }
        }
    }
}
