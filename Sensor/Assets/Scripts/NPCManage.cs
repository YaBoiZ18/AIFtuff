using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    public List<AudioListener> audioListeners = new List<AudioListener>(); // List of NPC AudioListeners
    public int SwitchEveryMS = 1000; // Time in milliseconds
    private float adjMS;
    private float accumulatedTime = 0f;
    private int currentIndex = 0;

    void Start()
    {
        if (audioListeners.Count == 0) 
        {
            Debug.LogError("No AudioListeners assinged to NPCManager!");
            return;
        }

        adjMS = SwitchEveryMS / 1000f;

        // Disable all listeners except the first one
        for (int i = 0; i < audioListeners.Count; i++) 
        {
            audioListeners[i].enabled = (i == 0);
        }
    }

    // Update is called once per frame
   void FixedUpdate()
    {
        if (audioListeners.Count <= 1)
            return;

        accumulatedTime += Time.fixedDeltaTime;
        if (accumulatedTime >= adjMS)
        {
            accumulatedTime = 0f;

            // Disable current listener
            audioListeners[currentIndex].enabled = false;

            // Move to next NPC
            currentIndex = (currentIndex + 1) % audioListeners.Count;

            // Enable new listener
            audioListeners[currentIndex].enabled = true;
        }

    }
}
