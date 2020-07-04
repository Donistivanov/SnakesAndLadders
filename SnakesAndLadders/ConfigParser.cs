using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakesAndLadders
{
    public class ConfigParser
    {
        public static (
            Dictionary<int, int> snakesAndLaddersTransitions,
            IEnumerable<(int playerId, int diceRoll)> playerMoves
            )
            Parse(string gameInfo)
        {
            var gameInfoChunks = gameInfo.Split($"{Environment.NewLine}{Environment.NewLine}");
            var (snakesAndLaddersConfig, playersMoves) = (gameInfoChunks[0], gameInfoChunks[1]);

            var snakesAndLaddersTransitions = ParseTransitions(snakesAndLaddersConfig);
            var playerMoves = ParsePlayersMoves(playersMoves);

            return (snakesAndLaddersTransitions, playerMoves);
        }

        public static Dictionary<int, int> ParseTransitions(string pairsAsConfigString)
        {
            var pairsAsStrings = pairsAsConfigString.Split(Environment.NewLine);

            var startEndPairs = pairsAsStrings.Select(pairAsString =>
            {
                var pairAsArr = pairAsString.Split(" ");
                var (start, end) = (int.Parse(pairAsArr[0]), int.Parse(pairAsArr[1]));
                return new KeyValuePair<int, int>(start, end);
            });

            return new Dictionary<int, int>(startEndPairs);
        }

        public static IEnumerable<(int playerId, int diceRoll)> ParsePlayersMoves(string playersMovesConfig)
        {
            var playersRollsAsString = playersMovesConfig.Split(Environment.NewLine);
            var playersRollsAsChars = playersRollsAsString.Select(s => s.ToCharArray());
            var playersRollsAsInts = playersRollsAsChars
                .Select(
                    charsList => charsList.Select(c => int.Parse(c.ToString())).ToList()
                ).ToList();

            var numberOfPlayers = playersRollsAsInts.Count;
            // https://stackoverflow.com/a/3363953
            var playerRollState = Enumerable.Repeat(0, numberOfPlayers).ToList();

            // Take it in turns to roll the dice.
            var areThereMoreRolls = true;
            while (areThereMoreRolls)
            {
                areThereMoreRolls = false;
                for (int playerId = 0; playerId < playerRollState.Count; playerId++)
                {
                    var playerRollsUntilNow = playerRollState[playerId];
                    var diceRollsForPlayer = playersRollsAsInts[playerId];
                    var totalRollsForPlayer = diceRollsForPlayer.Count;

                    if (playerRollsUntilNow < totalRollsForPlayer)
                    {
                        yield return (playerId, diceRollsForPlayer[playerRollsUntilNow]);
                        playerRollState[playerId] += 1;

                        // if with the latest roll we are still less than the total rolls for a player
                        if (playerRollState[playerId] < totalRollsForPlayer)
                        {
                            areThereMoreRolls = true;
                        }
                        // cannot set false in else statement because we might overwrite another player's declaration they have more rolls
                    }
                }
            }
        }
    }
}
