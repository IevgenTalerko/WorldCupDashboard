using WorldCupLibrary.Entities;

namespace WorldCupLibrary.Tests;

public static class MatchBoardTestHelper
{
    public static Match CreateMatch(this LocalStorage storage, Action<Match> overrides = null)
    {
        Match match = new()
        {
            AwayTeamId = 1,
            HomeTeamId = 2,
            AwayTeamGoals = 0,
            HomeTeamGoals = 0,
            Status = MatchStatus.Started,
            StartedOn = DateTime.Now
        };

        overrides?.Invoke(match);
        storage.Matches.Add(match);

        return match;
    }
}