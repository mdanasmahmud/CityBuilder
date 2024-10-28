using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    public Transform target;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);  // Move to the target position
    }
}
