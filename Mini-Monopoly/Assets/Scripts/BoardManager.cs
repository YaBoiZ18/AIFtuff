using UnityEngine;

using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; // Singleton instance of the BoardManager
    public List<Transform> boardTiles = new List<Transform>(); // List to store all board tile positions

    private void Awake()
    {
        Instance = this; // Assign the singleton instance to this object
    }
}
