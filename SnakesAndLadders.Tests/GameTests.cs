using System.Collections.Generic;
using Xunit;

namespace SnakesAndLadders.Tests
{
    public class GameTests
    {
        [Fact]
        public void PlayerCanOvershootEnd()
        {
            var endPlayerPlaces = Game.Play(GetOverShootMoves(), new Dictionary<int, int>());

            var expectedEnd = 100;
            var observedEndOfOvershootingPlayer = endPlayerPlaces[0];
            Assert.True(expectedEnd == observedEndOfOvershootingPlayer, $"If player rolls higher than spaces left for end, consider them as landing at {expectedEnd}. Observed end: {observedEndOfOvershootingPlayer}");
        }

        [Fact]
        public void DoNotMoveOtherPlayersIfOneWins()
        {
            var endPlayerPlaces = Game.Play(GetMovesWithRollsAfterEnding(), new Dictionary<int, int>());

            var expectedEndOfLosingPlayer = 4;
            var observedEndOfLostingPlayer = endPlayerPlaces[0];
            Assert.True(expectedEndOfLosingPlayer == observedEndOfLostingPlayer, $"Do not move losing players if there is a winner. Observed losing player end space: {observedEndOfLostingPlayer}, expected end: {expectedEndOfLosingPlayer}");
        }

        [Fact]
        public void PlayerTakesTransition()
        {
            var snakesAndLadders = new Dictionary<int, int>();
            var transitionStart = 4;
            var transitionEnd = 96;
            snakesAndLadders[transitionStart] = transitionEnd;

            var endPlayerPlaces = Game.Play(GetMovesWithPlayerLandingOnExpectedSpace(transitionStart), snakesAndLadders);

            var expectedEnd = transitionEnd;
            var observedEnd = endPlayerPlaces[0];
            Assert.True(expectedEnd == observedEnd, $"Players should take transitions. Expected end: {expectedEnd}, observed end: {observedEnd}");
        }

        private IEnumerable<(int playerId, int diceRoll)> GetOverShootMoves()
        {
            yield return (0, 50);
            yield return (1, 1);
            yield return (0, 51);
            yield return (1, 50);
        }

        private IEnumerable<(int playerId, int diceRoll)> GetMovesWithRollsAfterEnding()
        {
            yield return (0, 2);
            yield return (1, 50);
            yield return (0, 2);
            yield return (1, 50); // End is here
            yield return (0, 2);
        }

        private IEnumerable<(int playerId, int diceRoll)> GetMovesWithPlayerLandingOnExpectedSpace(int expectedSpace)
        {
            yield return (0, expectedSpace);
        }
    }
}
