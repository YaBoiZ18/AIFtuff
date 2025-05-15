namespace WordleGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool playing = true;

            while (playing)
            {
                Console.Clear();
                Console.WriteLine("==================================");
                Console.WriteLine("         Welcome to Wordle        ");
                Console.WriteLine("==================================");
                Console.WriteLine("1. Play a round of Wordle");
                Console.WriteLine("2. Exit");
                Console.Write("Please choose an option (1 or 2): ");
                string choice = Console.ReadLine().Trim();

                switch (choice)
                {
                    case "1":
                        // Start a new round.
                        Wordle game = new Wordle();
                        game.PlayRound();
                        break;

                    case "2":
                        Console.WriteLine("Thanks for Playing and come back another time!");
                        playing = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                        Console.WriteLine("Press Enter to try again...");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
