using UnityEngine;

using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public int currentTileIndex = 0;
    public float moveSpeed = 5f;
    private bool isMoving = false;

    public void Move(int steps)
    {
        if (!isMoving)
        {
            StartCoroutine(MovePlayer(steps));
        }
    }

    IEnumerator MovePlayer(int steps)
    {
        isMoving = true;
        for (int i = 0; i < steps; i++)
        {
            currentTileIndex++;
            if (currentTileIndex >= BoardManager.Instance.boardTiles.Count)
            {
                currentTileIndex = 0; // Loop around board
            }

            Vector3 targetPos = BoardManager.Instance.boardTiles[currentTileIndex].position;
            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(0.2f); // Pause between steps
        }

        isMoving = false;
        GameController.Instance.ChangeState(GameState.InteractWithPosition);

        FindObjectOfType<DiceRollUI>().EnableRollButton(); // Enable rolling again
    }
}
