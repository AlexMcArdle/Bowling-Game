namespace McArdle.Bowling
{
	public class BowlingGame
	{
        private static readonly int MaxFrames = 10;
        private static readonly Random Random = new();

        // Starter values.
        int?[][] Scoreboard = GenerateScoreboard(MaxFrames);
        private int CurrentFrameNumber = 1;
        private int CurrentRollNumber = 1;
        private int CurrentPinsLeft = 10;
        private int CurrentRoll = 0;

        public BowlingGame() { }

        /// <summary>
        /// Play the game.
        /// </summary>
        public void GoBowling(bool reset = false)
        {
            // Reset values. This is not necessary during the first game.
            if (reset)
            {
                ResetGame();
            }

            // Play the game
            while (CurrentFrameNumber <= MaxFrames)
            {
                Console.WriteLine($"\n\nYou are on frame {CurrentFrameNumber} roll {CurrentRollNumber}");
                Console.WriteLine("Press any key to roll..\n\n");
                Console.ReadKey(true);

                // Roll the ball.
                CurrentRoll = Random.Next(CurrentPinsLeft + 1);

                // Subtract the pins from the total.
                CurrentPinsLeft -= CurrentRoll;
                // Add the score to the scoreboard.
                Scoreboard[CurrentFrameNumber - 1][CurrentRollNumber - 1] = CurrentRoll;

                // Print details about the roll.
                Console.WriteLine($"Frame: {CurrentFrameNumber} Roll: {CurrentRollNumber} Rolled: {CurrentRoll}");
                if (WasStrike())
                {
                    Console.WriteLine("STRIKE!");
                }
                else if (WasSpare())
                {
                    Console.WriteLine("Spare!");
                }

                // Reset pins during the final round if it was a strike or a spare.
                if (CurrentFrameNumber == 10 && (WasStrike() && WasSpare() && CanRollAgain()))
                {
                    CurrentPinsLeft = 10;
                }

                // Check if we can roll again.
                if (CanRollAgain())
                {
                    CurrentRollNumber++;

                }
                // Reset the pins if we can't.
                else
                {
                    CurrentFrameNumber++;
                    CurrentPinsLeft = 10;
                    CurrentRollNumber = 1;
                }

                // Always print the scoreboard after a roll.
                PrintScoreboard();

            }

            Console.WriteLine("\nGame Over!");
        }

        /// <summary>
        /// Set new game values.
        /// </summary>
        private void ResetGame()
        {
            CurrentFrameNumber = 1;
            CurrentRollNumber = 1;
            CurrentPinsLeft = 10;
            CurrentRoll = 0;
            Scoreboard = GenerateScoreboard(MaxFrames);
        }

        /// <summary>
        /// Prints the scoreboard.
        /// </summary>
        private void PrintScoreboard()
        {
            // Print Header
            var header = new[] { "Frame", "1", "2", "3" };
            var separator = string.Join("___", new[] { "_____", "_____", "_____", "_____" });
            Console.WriteLine(separator);
            Console.WriteLine(string.Join(" | ", header.Select(h => h.PadLeft(5))));
            Console.WriteLine(separator);


            // Print each frame.
            for (var i = 0; i < Scoreboard.Length; i++)
            {
                var frame = Scoreboard[i];

                Console.WriteLine($"{i + 1,5} | {string.Join(" | ", frame.Select(r => (r?.ToString() ?? string.Empty).PadLeft(5)))}");
            }

            // Print the score.
            Console.WriteLine(separator);
            Console.WriteLine($"{(IsGameOver() ? "Final" : "Current")} score: {CalculateScore()}");
            Console.WriteLine(separator);
        }

        /// <summary>
        /// Game is over when the maximum frames have been reached.
        /// </summary>
        private bool IsGameOver()
        {
            return CurrentFrameNumber > MaxFrames;
        }

        /// <summary>
        /// Get current game score.
        /// </summary>
        private int CalculateScore()
        {
            return Scoreboard.Sum(f => f.Sum()) ?? 0;
        }

        /// <summary>
        /// Can the player roll again during the current frame?
        /// </summary>
        private bool CanRollAgain()
        {
            // Final round can have up to 3 rolls.
            if (CurrentFrameNumber == MaxFrames && CurrentRollNumber < 3)
            {
                var spare = WasSpare();
                var strike = WasStrike();
                var spareOrStrike = HasSpareOrStrike();
                return spare || strike || CurrentRollNumber < 2 || (CurrentRollNumber <= 3 && spareOrStrike);
            }
            // You can only roll again if there are pins remaining and it is not the final frame.
            else if (CurrentFrameNumber < MaxFrames)
            {
                return CurrentPinsLeft > 0 && CurrentRollNumber < 2;
            }

            return false;
        }

        /// <summary>
        /// Does the current frame have a spare or strike?
        /// </summary>
        private bool HasSpareOrStrike()
        {
            return Scoreboard[CurrentFrameNumber - 1].Where(r => r.HasValue).Sum() >= 10;
        }

        /// <summary>
        /// Was the current roll a strike?
        /// </summary>
        private bool WasStrike()
        {
            return CurrentRoll == 10;
        }

        /// <summary>
        /// Was the current roll a spare?
        /// Strikes are also considered spares.
        /// </summary>
        private bool WasSpare()
        {
            var frame = Scoreboard[CurrentFrameNumber - 1];
            var frameSum = frame.Sum();

            if (CurrentFrameNumber == MaxFrames && ((CurrentRollNumber == 2 && frame[0] == 10) || CurrentRollNumber == 3))
            {
                return false;
            }
            return frameSum >= 10;
        }

        /// <summary>
        /// Generate a new game scoreboard.
        /// </summary>
        private static int?[][] GenerateScoreboard(int maxFrames = 10, int maxRollsPerFrame = 3)
        {
            var newScoreboard = new List<int?[]>();
            for (var i = 0; i < maxFrames; i++)
            {
                newScoreboard.Add(new int?[maxRollsPerFrame]);
            }
            return newScoreboard.ToArray();
        }   
    }
}

