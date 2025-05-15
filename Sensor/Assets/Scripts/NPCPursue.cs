using UnityEngine;
using UnityEngine.AI;

public class NPCPursue : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Transform playerTransform;
    private NPCAudioDetector audioDetector;
    private NPCVision vision;
    private bool hasHeardPlayer = false; // Track if NPC has heard the player

    void Start()
    {
        audioDetector = GetComponent<NPCAudioDetector>();
        vision = GetComponent<NPCVision>();

        if (audioDetector != null)
            playerTransform = audioDetector.playerTransform;
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Update NPC's awareness of the player
        if (audioDetector.canBeHeard)
        {
            hasHeardPlayer = true; // NPC gets alerted
        }

        bool seesPlayer = vision.CanSeePlayer();

        // NPC moves only if it has heard the player AND still hears/sees them
        if (hasHeardPlayer && (audioDetector.canBeHeard || seesPlayer))
        {
            ChasePlayer();
        }
        else
        {
            StopChasing(); // Stop moving when the player is no longer detected
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void StopChasing()
    {
        hasHeardPlayer = false; // NPC resets if it loses track of the player
    }
}
