using UnityEngine;

public class NPCVision : MonoBehaviour
{
    public float detectionRange = 5f; // How far the NPC can see
    public LayerMask obstructionMask; // Define walls/obstacles here
    public Transform playerTransform;

    public bool CanSeePlayer()
    {
        if (playerTransform == null) return false;

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Perform a Raycast
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionMask))
            {
                return true; // No obstacle, NPC can see the player
            }
        }

        return false;
    }
}
