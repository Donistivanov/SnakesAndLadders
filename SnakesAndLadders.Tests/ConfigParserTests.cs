using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SnakesAndLadders.Tests
{
    public class ConfigParserTests
    {
        [Fact]
        public void ParsesGameInfo()
        {
            var (inMemoryPairs, stringifiedPairs) = GetTransitions();
            var (inMemoryRolls, stringifiedRolls) = GetRolls();
            var totalGameInfo = string.Join($"{Environment.NewLine}{Environment.NewLine}", new List<string>() { stringifiedPairs, stringifiedRolls });

            var (transitionDict, parsedPlayerRolls) = ConfigParser.Parse(totalGameInfo);

            CompareInMemoryTransitionsWithParsing(inMemoryPairs, transitionDict);
            CompareInMemoryRollsWithParsing(inMemoryRolls, parsedPlayerRolls.ToList());
        }

        [Fact]
        public void ParsesTransitionPairs()
        {
            var (inMemoryPairs, stringifiedPairs) = GetTransitions();

            var transitionDict = ConfigParser.ParseTransitions(stringifiedPairs);

            CompareInMemoryTransitionsWithParsing(inMemoryPairs, transitionDict);
        }

        [Fact]
        public void ParsesPlayerMoves()
        {
            var (inMemoryRolls, stringifiedRolls) = GetRolls();

            var parsedPlayerRolls = ConfigParser.ParsePlayersMoves(stringifiedRolls).ToList();

            CompareInMemoryRollsWithParsing(inMemoryRolls, parsedPlayerRolls);
        }

        private void CompareInMemoryTransitionsWithParsing(List<List<int>> inMemoryPairs, Dictionary<int, int> parsedTransitions)
        {
            var expectedCount = inMemoryPairs.Count;
            var observedCount = parsedTransitions.Count;
            Assert.True(expectedCount == observedCount, $"The transition dictionary should have {expectedCount} members, but had {observedCount}");

            foreach (var pairAsInts in inMemoryPairs)
            {
                var expectedKey = pairAsInts[0];
                var expectedValue = pairAsInts[1];
                Assert.Contains(parsedTransitions, kv => kv.Key == expectedKey && kv.Value == expectedValue);
            }
        }

        private void CompareInMemoryRollsWithParsing(List<List<int>> inMemoryRolls, List<(int playerId, int diceRoll)> parsedPlayerRolls)
        {
            var expectedTotalRollCount = inMemoryRolls.Sum(playerRolls => playerRolls.Count);
            var parsedTotalRollCount = parsedPlayerRolls.Count;
            Assert.True(expectedTotalRollCount == parsedTotalRollCount, $"The number of rolls should be {expectedTotalRollCount}, but was {parsedTotalRollCount}");

            var possibleRolls = new List<int>() { 1, 2, 3, 4, 5, 6 };
            for (int playerId = 0; playerId < inMemoryRolls.Count; playerId++)
            {
                foreach (var diceRoll in possibleRolls)
                {
                    var expectedRoll = inMemoryRolls.ElementAt(playerId).Count(roll => roll == diceRoll);
                    var parsedRoll = parsedPlayerRolls.Count(roll => roll.playerId == playerId && roll.diceRoll == diceRoll);
                    Assert.True(expectedRoll == parsedRoll, $"Player {playerId + 1} should have rolled a {diceRoll}, {expectedRoll} time(s), but rolled it {parsedRoll} time(s)");
                }
            }
        }

        private (List<List<int>> inMemoryPairs, string stringifiedPairs) GetTransitions()
        {
            var pairsAsInts = new List<List<int>>() {
                new List<int> () {12, 34},
                new List<int> () {4, 92},
                new List<int> () {36, 1},
            };
            var pairsAsStrings = pairsAsInts.Select(pair => string.Join(' ', pair));
            var configPairs = string.Join(Environment.NewLine, pairsAsStrings);

            return (pairsAsInts, configPairs);
        }

        private (List<List<int>> inMemoryRolls, string stringifiedRolls) GetRolls()
        {
            var playersRolls = new List<List<int>>() {
                new List<int> () {1, 3, 4, 6, 2, 5, 6, 3},
                new List<int> () {4, 6, 6, 6, 1, 4, 2, 2},
                new List<int> () {6, 5, 3, 5, 4, 4, 1, 1},
            };
            var playersRollsAsTrings = playersRolls.Select(playerRolls => string.Join("", playerRolls));
            var configRolls = string.Join(Environment.NewLine, playersRollsAsTrings);

            return (playersRolls, configRolls);
        }
    }
}
