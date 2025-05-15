using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script is a modification of the implementation of flocking behaviors
/// </summary>
public class FlockController : MonoBehaviour
{
    // Variables for controlling flock movement behavior
    public float minVelocity = 1;
    public float maxVelocity = 8;
    public float centerWeight = 1;
    public float velocityWeight = 1;
    public float separationWeight = 1;
    public float followWeight = 1;
    public float randomizeWeight = 1;

    // Flock prefab and target transform (assigned in the Unity Inspector)
    public Flock prefab;
    public Transform target;

    // Flag to determine if the race should continue
    public bool continueRace = true;

    // Stores the original positions of all flock members
    private Vector3[] origPositions;

    // Stores the computed flock center and velocity
    internal Vector3 flockCenter;
    internal Vector3 flockVelocity;

    // List of all flock instances
    public List<Flock> flockList = new List<Flock>();

    // Player input component for handling user interactions
    private PlayerInput playerInput;

    void Start()
    {
        // Ensure the FlockController starts at the world origin
        transform.position = Vector3.zero;

        // Add a PlayerInput component dynamically
        playerInput = gameObject.AddComponent<PlayerInput>();

        // Ensure a target has been assigned, otherwise log an error
        if (target == null)
        {
            Debug.LogError("Target is not assigned in the Inspector!");
            return;
        }

        // Store original positions of flock members for resetting
        origPositions = new Vector3[flockList.Count];
        for (int i = 0; i < flockList.Count; i++)
        {
            flockList[i].controller = this; // Assign controller reference
            origPositions[i] = flockList[i].transform.position;
        }
    }

    void Update()
    {
        // Stop updating if the race has ended
        if (!continueRace) return;

        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        // Compute the center and average velocity of the flock
        foreach (Flock flock in flockList)
        {
            center += flock.transform.localPosition;
            velocity += flock.GetComponent<Rigidbody>().linearVelocity;
        }

        flockCenter = center / flockList.Count;
        flockVelocity = velocity / flockList.Count;
    }

    /// <summary>
    /// Resets all flock members to their original positions when clicked
    /// </summary>
    public void OnClick()
    {
        // Loop through and reset all flock positions
        for (int i = 0; i < flockList.Count; i++)
        {
            flockList[i].transform.position = origPositions[i];
        }

        // Restart the race
        continueRace = true;
    }
}
