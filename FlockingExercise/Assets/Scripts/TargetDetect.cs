using UnityEngine;

public class TargetDetect : MonoBehaviour
{
    // Public reference to the FlockController, to be assigned in Unity Inspector
    public FlockController flockController;

    // Boolean flag to ensure only the first flock to hit the target is registered
    private bool targetHit = false;

    void OnTriggerEnter(Collider other)
    {
        // If a flock has already hit the target, exit early to prevent multiple triggers
        if (targetHit) return;

        // Check if the colliding object is a Flock instance
        Flock flock = other.GetComponent<Flock>();
        if (flock != null && flockController.flockList.Contains(flock))
        {
            // Mark that a flock has hit the target
            targetHit = true;
            Debug.Log(flock.name + " hit the target first!");

            // Stop movement for all flock instances
            StopAllFlocks();
        }
    }

    private void StopAllFlocks()
    {
        // Loop through all flocks in the FlockController's list
        foreach (Flock flock in flockController.flockList)
        {
            // Access the Rigidbody component to stop movement
            Rigidbody rb = flock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Set velocity and angular velocity to zero, effectively stopping movement
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
