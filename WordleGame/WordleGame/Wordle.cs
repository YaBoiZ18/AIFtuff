using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleGame
{
    internal class Wordle
    {
        // Data members for the game
        private readonly string secretWord = "CRANE"; // Secret word (5 letters)
        private int attemptCount = 0;
        private bool winner = false;
        private List<string> attempts = new List<string>();

        // Define the FSM states as a private enum.
        private enum State
        {
            WordEntry,           // User enters a guess.
            Confirm,             // Confirm if the guess should be submitted.
            Score,               // Update the attempt count and record the guess.
            IsWinner,            // Check if the guess matches the secret word.
            Review,              // Provide feedback on which letters are correct.
            ConfirmAfterReview,  // Pause after review, then go back to word entry.
            Display              // Show all attempts and final outcome.
        }

        /// <summary>
        /// PlayRound implements the finite state machine for a single round of Wordle.
        /// The user gets six attempts to guess the secret word.
        /// </summary>
        public void PlayRound()
        {
            State currentState = State.WordEntry;
            string currentGuess = "";

            // Continue the game until six attempts are used or the player wins.
            while (attemptCount < 6 && !winner)
            {
                switch (currentState)
                {
                    case State.WordEntry:
                        Console.Write("Enter a 5-letter word: ");
                        currentGuess = Console.ReadLine().Trim().ToUpper();
                        if (currentGuess.Length != 5)
                        {
                            Console.WriteLine("Invalid entry. Please enter exactly 5 letters.");
                            // Stay in the WordEntry state.
                        }
                        else
                        {
                            currentState = State.Confirm;
                        }
                        break;

                    case State.Confirm:
                        Console.Write($"You entered \"{currentGuess}\". Submit this guess? (Y/N): ");
                        string response = Console.ReadLine().Trim().ToUpper();
                        if (response == "Y")
                        {
                            currentState = State.Score;
                        }
                        else
                        {
                            // Let the user re-enter the word.
                            currentState = State.WordEntry;
                        }
                        break;

                    case State.Score:
                        Score(currentGuess);
                        currentState = State.IsWinner;
                        break;

                    case State.IsWinner:
                        if (IsWinner(currentGuess))
                        {
                            winner = true;
                            currentState = State.Display;
                        }
                        else
                        {
                            currentState = State.Review;
                        }
                        break;

                    case State.Review:
                        ReviewGuess(currentGuess);
                        currentState = State.ConfirmAfterReview;
                        break;

                    case State.ConfirmAfterReview:
                        Console.WriteLine("Press Enter to continue to your next guess...");
                        Console.ReadLine();
                        currentState = State.WordEntry;
                        break;

                    default:
                        break;
                }
            }

            // If six attempts have been used without a win, or the player just won, display the results.
            currentState = State.Display;
            Display();
        }

        /// <summary>
        /// Score increments the attempt counter and stores the guess.
        /// </summary>
        /// <param name="guess">The user's guess.</param>
        private void Score(string guess)
        {
            attemptCount++;
            attempts.Add(guess);
        }

        /// <summary>
        /// IsWinner returns true if the guess exactly matches the secret word.
        /// </summary>
        /// <param name="guess">The user's guess.</param>
        /// <returns>True if the guess is correct; otherwise, false.</returns>
        private bool IsWinner(string guess)
        {
            return guess.Equals(secretWord, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// ReviewGuess compares each letter of the guess with the secret word
        /// and provides feedback on each letter.
        /// </summary>
        /// <param name="guess">The user's guess.</param>
        private void ReviewGuess(string guess)
        {
            Console.WriteLine("Review of your guess:");

            // For each letter in the guess, compare with the secret word.
            for (int i = 0; i < guess.Length; i++)
            {
                if (guess[i] == secretWord[i])
                {
                    // Letter is in the correct position.
                    Console.WriteLine($"Letter '{guess[i]}' is in the correct position.");
                }
                else if (secretWord.Contains(guess[i]))
                {
                    // Letter exists in the secret word but in a different position.
                    Console.WriteLine($"Letter '{guess[i]}' is in the word but in the wrong position.");
                }
                else
                {
                    // Letter is not in the secret word.
                    Console.WriteLine($"Letter '{guess[i]}' is not in the word.");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display shows all the guesses, the number of attempts used, and whether the player won or lost.
        /// The display remains until the user presses Enter.
        /// </summary>
        public void Display()
        {
            Console.Clear();
            Console.WriteLine("==================================");
            Console.WriteLine("           Game Summary           ");
            Console.WriteLine("==================================");
            Console.WriteLine($"Total Attempts: {attemptCount}");
            Console.WriteLine("Your guesses:");

            foreach (var guess in attempts)
            {
                Console.WriteLine(guess);
            }

            if (winner)
            {
                Console.WriteLine("\nCongratulations! You Won!");
            }
            else
            {
                Console.WriteLine("\nYou've used all your attempts. You Lost!");
                Console.WriteLine($"The secret word was: {secretWord}");
            }

            Console.WriteLine("\nPress Enter to return to the Main Menu...");
            Console.ReadLine();
        }
    }
}

