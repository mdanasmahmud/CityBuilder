using UnityEngine;
using UnityEngine.AI;

public class CarNavigation : MonoBehaviour
{
    public Transform destination;
    private NavMeshAgent navAgent;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        if (destination != null)
        {
            navAgent.SetDestination(destination.position);
        }
    }

    void Update()
    {
        if (navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
        {
            Debug.Log("Car has reached the destination");
        }
    }
}
