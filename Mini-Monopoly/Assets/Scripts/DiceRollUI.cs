using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceRollUI : MonoBehaviour
{
    public TextMeshProUGUI diceText; // Assign in Inspector
    public Button rollButton; // Assign in Inspector

    void Start()
    {
        rollButton.onClick.AddListener(RollDice);
    }

    void RollDice()
    {
        int diceRoll = Random.Range(1, 7); // Roll 1-6
        diceText.text = "You rolled: " + diceRoll;
        rollButton.interactable = false; // Disable button during movement
        GameController.Instance.RollDice(diceRoll); // Notify GameController
    }

    public void EnableRollButton()
    {
        rollButton.interactable = true;
    }
}
