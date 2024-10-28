using UnityEngine;
using System.Collections;

public class MoneyThrower : MonoBehaviour
{
    public GameObject moneyPrefab;     // Prefab of the money note to be thrown
    public Transform sourceObject;     // Source object (e.g. car)
    public Transform destinationObject; // Destination object (e.g. Tax building)
    public float throwInterval = 1f;   // Time interval between each throw

    private float nextThrowTime = 0f;

    void Update()
    {
        // Check if the source and destination objects are assigned
        if (sourceObject == null || destinationObject == null)
        {
            return; // Skip the update if references are not assigned
        }

        // Check if it's time to throw the money
        if (Time.time >= nextThrowTime)
        {
            ThrowMoney();
            nextThrowTime = Time.time + throwInterval; // Set the next throw time
        }
    }

    void ThrowMoney()
    {
        // Check if the source and destination objects are assigned
        if (sourceObject == null || destinationObject == null)
        {
            return; // Skip the throw if references are not assigned
        }

        // Instantiate the money note at the source object's position with a +90 degree rotation on the X-axis
        GameObject money = Instantiate(moneyPrefab, sourceObject.position, Quaternion.Euler(90, 0, 0));

        // Start the coroutine to move the money to the destination
        StartCoroutine(MoveMoneyToDestination(money, destinationObject.position));
    }

    IEnumerator MoveMoneyToDestination(GameObject money, Vector3 targetPos)
    {
        float duration = 2f; // Duration of the movement in seconds
        float elapsedTime = 0f;
        Vector3 startPos = money.transform.position;

        while (elapsedTime < duration)
        {
            // Calculate the new position using Slerp for a smooth arc-like motion
            money.transform.position = Vector3.Slerp(startPos, targetPos, elapsedTime / duration);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the money reaches the exact target position
        money.transform.position = targetPos;

        // Destroy the money object after it reaches the destination
        Destroy(money);
    }
}
