using System.Text.RegularExpressions;

namespace WorldCupLibrary.MatchBoard;

public class MatchBoardService : IMatchBoardService
{
    public void StartMatch(int homeTeamId, int awayTeamId)
    {
        throw new NotImplementedException();
    }

    public void UpdateScore(Guid matchId, int homeTeamGoals, int awayTeamGoals)
    {
        throw new NotImplementedException();
    }

    public void FinishMatch(Guid matchId)
    {
        throw new NotImplementedException();
    }

    public List<Match> GetMatchesInProgress()
    {
        throw new NotImplementedException();
    }
}