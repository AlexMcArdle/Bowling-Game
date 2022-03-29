namespace McArdle.Bowling
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("\n\nLet's Go Bowling!");

            // Niko, let's go bowling!
            var game = new BowlingGame();
            game.GoBowling();

            // Check if the player would like to play again or quit.
            var running = true;
            while (running)
            {
                Console.WriteLine("\n\nPlay (A)gain or (Q)uit?");
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.A)
                {
                    game.GoBowling(true);
                }
                else if (key.Key == ConsoleKey.Q)
                {
                    running = false;
                }
            }
        }
    }
}