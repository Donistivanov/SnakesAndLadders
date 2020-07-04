using System.Collections.Generic;

namespace SnakesAndLadders
{
    public class Game
    {
        public static Dictionary<int, int> Play(IEnumerable<(int playerId, int diceRoll)> playerMoves, Dictionary<int, int> snakesAndLadders)
        {
            var playerState = new Dictionary<int, int>();

            foreach ((int playerId, int diceRoll) in playerMoves)
            {
                // Each player puts their counter assuming its starting point zero.
                var playerCurrPosition = playerState.ContainsKey(playerId) ? playerState[playerId] : 0;
                // Move your counter forward the number of spaces shown on the dice.
                var playerNextPosition = playerCurrPosition + diceRoll;
                // If your counter lands at the bottom of a ladder, you can move up to the top of the ladder.
                // If your counter lands on the head of a snake, you must slide down to the bottom of the snake.
                if (snakesAndLadders.ContainsKey(playerNextPosition))
                {
                    playerNextPosition = snakesAndLadders[playerNextPosition];
                }
                playerState[playerId] = playerNextPosition >= 100 ? 100 : playerNextPosition;

                // The first player to get to the space 100 is the winner.
                if (playerState[playerId] == 100)
                {
                    break;
                }
            }

            return playerState;
        }
    }
}
