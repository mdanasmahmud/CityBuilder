using UnityEngine;

public class TunnelTrigger : MonoBehaviour
{
    public CarTunnelNavigation carTunnelNavigation; // Reference to the CarTunnelNavigation script

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is a car
        if (other.CompareTag("Car"))
        {
            // Call the method to destroy the car
            carTunnelNavigation.DestroyCarAndRespawn(other.gameObject);
            Debug.Log("Car entered tunnel trigger!");
        }
    }
}
