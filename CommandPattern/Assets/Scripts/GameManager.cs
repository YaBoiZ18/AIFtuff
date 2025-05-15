using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading;

namespace Assets.Scripts
{
    public enum Direction { LEFT, RIGHT, UP, DOWN }

    public class GameManager : Singleton<GameManager>
    {
        public int maxCol = 5;
        public int maxRow = 5;

        public List<Marker> allMarkers = new List<Marker>();

        public List<BoundaryCheck> m_barriers = null;

        private Dictionary<Vector2, Marker> m_Markers = null;
        /// <summary>
        /// Initialized in CreateMakerDictionary
        /// Updated by the OnTriggerEnter in Marker.
        /// </summary>
        public Marker CurrentMarker { get; set; }

        /// <summary>
        /// Step 4:
        /// What occurs in this method.
        /// HINT:
        /// 1. You may have to look at the methods it calls before you can answer this question.
        /// 2. This should be an overview. The details of what occurs in each method should be 
        /// written above those places.
        /// 
        /// Initializing data structures that the game will use for navigation and movement. As well as creating the barriers. 
        /// </summary>
        void Start()
        {
            CreateMarkerDictionary();
            CreateNeighborsForAllMarkers();

            CreateBarriersList();
        }

        private void Awake()
        {
            allMarkers.Clear();
            Marker[] markers = FindObjectsOfType<Marker>();
            allMarkers.AddRange(markers);
        }

        /// <summary>
        /// Step 5.
        /// Explain the purpose of this method.
        /// 
        /// The purpose is to identify all game objects in the scene that are tagged as 
        /// "Barrier" and then create a list of components related to these barriers. 
        /// </summary>
        private void CreateBarriersList()
        {
            GameObject[] allBarriers =
            GameObject.FindGameObjectsWithTag("Barrier");

            m_barriers = new List<BoundaryCheck>();
            foreach (GameObject barrier in allBarriers)
            {

                BoundaryCheck boundaryCheckComponent =
                barrier.GetComponent<BoundaryCheck>();
                m_barriers.Add(boundaryCheckComponent);
            }
        }


        /// <summary
        /// Step 6.
        /// Explain the purpose of this method.
        /// 
        /// The method's purpose is to locate all game objects in the scene tagged as "Marker" 
        /// and store them in a dictionary for quick lookup. This dictionary uses the Vector2
        /// as the key, and the Marker component as the value. 
        /// The method also sets the initial CurrentMarker to the marker at the "Lower Left" position, which is identified by having the rowCol 
        /// property equal to Vector2.zero.
        /// </summary>
        private void CreateMarkerDictionary()
        {
            GameObject[] allMarkers =
            GameObject.FindGameObjectsWithTag("Marker");

            m_Markers = new Dictionary<Vector2, Marker>();
            foreach (GameObject marker in allMarkers)
            {

                Marker markerComponent =
                marker.GetComponent<Marker>();
                m_Markers.Add(markerComponent.rowCol, markerComponent);

                // Set the initial CurrentMarker to the
                // Lower Left marker.
                if (markerComponent.rowCol == Vector2.zero)
                {
                    CurrentMarker = markerComponent;
                }

            }


        }

        /// <summary
        /// Step 7.
        /// Explain the purpose of this method.
        /// HINT: Look at the next method, CreateNeighbors, before
        /// completing this.
        /// Like Start, this should be an overview.
        /// 
        /// The method is responsible for setting up the neighboring markers
        /// for each marker in the grid. By iterating over all markers and
        /// utilizing the CreateNeighbors() method, this function organizes 
        /// each marker’s relationships with its adjacent markers in the grid.
        /// </summary>

        void CreateNeighborsForAllMarkers()
        {

            foreach (KeyValuePair<Vector2, Marker> pair in m_Markers.
                OrderBy(o => o.Value.rowCol.x).ThenBy(o => o.Value.rowCol.y))
            {
                CreateNeighbors(pair);
            }


        }

        /// <summary>
        /// Step 8.
        /// Explain the purpose of this method.
        /// 
        /// The method is designed to determine and assign the neighboring markers for a given marker in a grid. 
        /// It creates a dictionary that maps the directions to the corresponding neighboring Marker. 
        /// </summary>
        /// <param name="pair">The Key Value from the current dictionary item</param>
        /// <returns></returns>
        private Dictionary<Direction, Marker> CreateNeighbors(KeyValuePair<Vector2, Marker> pair)
        {
            Dictionary<Direction, Marker> neighbors = new Dictionary<Direction, Marker>();
            int x = (int)pair.Key.x;
            int y = (int)pair.Key.y;
            if (y > 0)
                neighbors[Direction.LEFT] = m_Markers[new Vector2(x, y - 1)];
            else
                neighbors[Direction.LEFT] = null;

            if (y < maxCol)
                neighbors[Direction.RIGHT] = m_Markers[new Vector2(x, y + 1)];
            else
                neighbors[Direction.RIGHT] = null;

            if (x > 0)
                neighbors[Direction.DOWN] = m_Markers[new Vector2(x - 1, y)];
            else
                neighbors[Direction.DOWN] = null;

            if (x < maxRow)
                neighbors[Direction.UP] = m_Markers[new Vector2(x + 1, y)];
            else
                neighbors[Direction.UP] = null;

            pair.Value.neighbors = neighbors;
            return neighbors;
        }


        /// <summary>
        /// Step 9.
        /// Explain the purpose of this method.
        /// 
        /// The IsBarrier method checks whether a given Marker is blocked by a barrier in the game grid. 
        /// It evaluates whether the current marker is located within the boundaries of any barrier in the game. 
        /// The method returns true if a barrier is found at the given marker's location, and false otherwise.
        /// </summary>
        /// <param name="current">The marker that is being considered as a potential next move.</param>
        /// <returns></returns>
        internal bool IsBarrier(Marker current)
        {
            bool fndsBarrier = false;

            foreach (BoundaryCheck barrierCheck in m_barriers)
            {
                if (barrierCheck.IsWithinBoundaries(current))
                {
                    fndsBarrier = true;
                    break;
                }
            }

            return fndsBarrier;
        }

        /// <summary>
        /// Finds a valid teleport destination by iterating through all markers in the game.
        /// A marker is considered valid if it is not a barrier and is not the player's current marker.
        /// Returns the first valid marker found or null if no valid destination exists.
        /// </summary>
        /// <returns>A valid marker for teleportation or null if none are available.</returns>
        public Marker FindTeleportDestination() 
        {
            foreach (var marker in m_Markers.Values)
            {
                if (!IsBarrier(marker) && marker != CurrentMarker) // Check if the marker is not blocked and different from current position
                {
                    return marker;
                }
            }
            return null; // No valid teleport location found
        }

        /// <summary>
        /// Teleports the player to the specified target marker.
        /// Updates the player's current marker to the target marker and recalculates neighbors for all markers 
        /// to reflect the new position after teleportation.
        /// Does nothing if the target marker is null.
        /// </summary>
        /// <param name="targetMarker">The marker to teleport the player to.</param>
        public void TeleportTo(Marker targetMarker) 
        {
            if (targetMarker != null)
            {
                // Update the player's position to the marker's position (could be handled differently depending on your game logic)
                CurrentMarker = targetMarker;

                // Recalculate the neighbors for the new position after teleportation.
                CreateNeighborsForAllMarkers(); // Ensure neighbors are updated based on the new position
            }
        }

        /// <summary>
        /// Finds and returns the marker located at the specified position.
        /// Compares the distance of each marker's position to the given position within a small tolerance 
        /// to account for floating-point inaccuracies.
        /// Returns null if no marker is found at the specified position.
        /// </summary>
        /// <param name="position">The position to search for a corresponding marker.</param>
        /// <returns>The marker located at the given position, or null if none is found.</returns>
        public Marker FindMarkerAtPosition(Vector3 position) 
        {
            // Iterate over all markers in the game to find the one at the given position
            foreach (Marker marker in allMarkers)
            {
                if (Vector3.Distance(marker.transform.position, position) < 0.1f) // Small tolerance for floating-point comparison
                {
                    return marker;
                }
            }
            return null; // No marker found at the position
        }
    }
}