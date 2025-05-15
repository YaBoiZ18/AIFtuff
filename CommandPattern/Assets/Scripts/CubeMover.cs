using Assets.Scripts;
using Assets.Scripts.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    GridGameCubePlayer inputActions = null;
    public Vector3 latestMove = Vector3.zero;

    private Vector3 startingPosition = Vector3.zero;
    private Stack<Vector3> moveHistory = new Stack<Vector3>();
    private void Awake()
    {
        inputActions = new GridGameCubePlayer();
        inputActions.Player.Move.performed += Move_performed;

        GridGameEventBus.Subscribe(MovementEventType.RESTART, ResetPosition);
    }

    private void ResetPosition()
    {
        this.transform.position = startingPosition;
        // Clear movement history
        moveHistory.Clear();
    }

    private void Start()
    {
        startingPosition = this.transform.position;
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }


   
    /// <summary>
    /// Remap the X-Y input to the X, Z in a Vector3
    /// The context is negated (flipped) since the 
    /// camera is placed ahead of the grid. 
    /// So Left appears to Right, Down is up. Negating the vector
    /// solves this problem. 
    /// </summary>
    /// <param name="context"></param>
    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        Marker currentMarker = GameManager.Instance.CurrentMarker;
        Command currentCommand = null;

        Direction[] directions = new[] { Direction.LEFT, Direction.RIGHT, Direction.UP, Direction.DOWN };
        Dictionary<Vector2, Direction> Vector2ToDirection =
            new Dictionary<Vector2, Direction>
            {
                { Vector2.left, Direction.LEFT },
                { Vector2.right, Direction.RIGHT },
                { Vector2.up, Direction.UP },
                { Vector2.down, Direction.DOWN }

            };


        if (Vector2ToDirection.TryGetValue(direction, out Direction current))
        {
            switch (current)
            {
                // STEP 2:
                // UNCOMMENT this area after the MoveLeft, MoveRight, MoveUp and MoveDown classes
                // are created in the Moves.cs file.
                // Remember that all of these classes inherit from Command and they must
                // implement the CanExecute and Execute methods with the same signature 
                // as what is in the Command class.

                case Direction.LEFT:
                    currentCommand = new MoveLeft();
                    break;
                case Direction.RIGHT:
                    currentCommand = new MoveRight();
                    break;
                case Direction.UP:
                    currentCommand = new MoveUp();
                    break;
                case Direction.DOWN:
                    currentCommand = new MoveDown();
                    break;

                default:
                    break;
            }

            if (currentCommand != null && currentCommand.CanExecute(currentMarker.neighbors))
            {
                // Record current position in history
                moveHistory.Push(transform.position);

                latestMove = currentCommand.Execute(currentMarker.neighbors[current], transform.position);
                transform.Translate(latestMove);
            }
            else
            {
                CheckForAvailableMoves();
            }
        }


    }

    /// <summary>
    /// Checks whether there are any valid moves available for the player.
    /// Iterates through the current marker's neighbors to determine if there is at least one unvisited and non-barrier marker.
    /// Logs a message to the console if no valid moves remain.
    /// </summary>
    private void CheckForAvailableMoves()
    {
        Marker currentMarker = GameManager.Instance.CurrentMarker;
        bool hasMove = false;

        foreach (var neighbor in currentMarker.neighbors)
        {
            if (neighbor.Value != null && !neighbor.Value.HasBeenVisited && !GameManager.Instance.IsBarrier(neighbor.Value))
            {
                hasMove = true;
                break;
            }
        }

        if (!hasMove)
        {
            Debug.Log("No more possible moves");
        }
    }

    /// <summary>
    /// Reverts the player's position to the last recorded state, effectively undoing the most recent move.
    /// Updates the player's position, reactivates the marker at the previous position, and updates the current marker in the GameManager.
    /// If no moves are available to undo, logs a message indicating this.
    /// Publishes a NEXT_MOVE event to notify the system of the updated game state.
    /// </summary>
    public void UndoLastMove()
    {
        if (moveHistory.Count > 0)
        {
            // Get the last recorded position
            Vector3 previousPosition = moveHistory.Pop();

            // Move the player back to the previous position
            transform.position = previousPosition;

            // Find the marker at this position
            Marker marker = GameManager.Instance.FindMarkerAtPosition(previousPosition);
            if (marker != null)
            {
                // Reactivate the marker
                marker.ResetMaterial();
            }

            // Update the current marker in the GameManager
            GameManager.Instance.CurrentMarker = marker;

            // Notify the system of the move
            GridGameEventBus.Publish(MovementEventType.NEXT_MOVE);
        }
        else
        {
            Debug.Log("No moves to undo!");
        }
    }

}
