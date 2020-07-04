using System;
using System.IO;
using System.Linq;

namespace SnakesAndLadders
{
    class Program
    {
        static void Main()
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file#example-synchronous-read-in-a-console-app
            try
            {
                using StreamReader sr = new StreamReader("config.txt");
                var gameInfo = sr.ReadToEnd();

                var (snakesAndLadders, playerMoves) = ConfigParser.Parse(gameInfo);

                var playersEndStateOrdered = Game.Play(playerMoves, snakesAndLadders).OrderBy(kv => kv.Key);

                foreach (var playerEndState in playersEndStateOrdered)
                {
                    Console.WriteLine($"Player {playerEndState.Key + 1} ended up at: {playerEndState.Value}");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Press Enter to continue ...");
            Console.ReadLine();
        }
    }
}
