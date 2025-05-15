using UnityEngine;

public enum GameState // Game states
{
    StartGame,           
    PlayRound,          
    RollDice,            
    MoveToNextPosition,  
    InteractWithPosition,
    ActionState          
}

public class GameController : MonoBehaviour
{
    // Current game state
    public GameState currentState;

    // Singleton instance of GameController
    public static GameController Instance;

    // Reference to the player movement script
    public PlayerMovement player;

    // Last dice roll result
    private int lastDiceRoll = 0;

    private void Awake()
    {
        // Assign the singleton instance to this object
        Instance = this;
    }

    public void Start()
    {
        // Set initial game state to StartGame
        ChangeState(GameState.StartGame);
    }

    public void Update()
    {
        // Handle different game states in the update loop
        switch (currentState)
        {
            case GameState.StartGame:
                StartGame();
                break;
            case GameState.PlayRound:
                PlayRound();
                break;
            case GameState.RollDice:
                // Waiting for dice roll input from UI
                break;
            case GameState.MoveToNextPosition:
                MoveToNextPosition();
                break;
            case GameState.InteractWithPosition:
                InteractWithPosition();
                break;
            case GameState.ActionState:
                PerformAction();
                break;
        }
    }

    // Change the current game state to a new one
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    // Called when the game starts, transitioning to the PlayRound state
    public void StartGame()
    {
        Debug.Log("Game Started!");
        ChangeState(GameState.PlayRound);
    }

    // Called when it's the player's turn to play a round
    public void PlayRound()
    {
        Debug.Log("Playing Round...");
        ChangeState(GameState.RollDice);
    }

    // Rolls the dice and transitions to the MoveToNextPosition state
    public void RollDice(int diceRoll)
    {
        lastDiceRoll = diceRoll;
        Debug.Log("Rolled: " + diceRoll);
        ChangeState(GameState.MoveToNextPosition);
    }

    // Moves the player to the next position based on the dice roll
    public void MoveToNextPosition()
    {
        Debug.Log("Moving...");
        player.Move(lastDiceRoll);
    }

    // Interacts with the tile the player has landed on (Not in use currently)
    public void InteractWithPosition()
    {
        Debug.Log("Interacting with Tile...");
        ChangeState(GameState.ActionState);
    }

    // Performs the action associated with the current position (Not in use currently)
    public void PerformAction()
    {
        Debug.Log("Performing Action...");
        ChangeState(GameState.PlayRound);
    }
}