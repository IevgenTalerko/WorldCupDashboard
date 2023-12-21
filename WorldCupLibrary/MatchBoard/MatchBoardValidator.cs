using WorldCupLibrary.Entities;

namespace WorldCupLibrary.MatchBoard;

public class MatchBoardValidator : IMatchBoardValidator
{
    public void ValidateStartMatch(int homeTeamId, int awayTeamId)
    {
        if (IsMatchInProgressForTeam(homeTeamId) || IsMatchInProgressForTeam(awayTeamId))
            throw new InvalidOperationException(Constants.ValidationMessages.OneMatchInProgress);
    }

    public void ValidateUpdateScore(Match? match, int homeTeamGoals, int awayTeamGoals)
    {
        ValidateMatchExists(match);

        if (match.Status != MatchStatus.Started)
            throw new InvalidOperationException(Constants.ValidationMessages.StartedStatus);

        if (Math.Abs(match.HomeTeamGoals - homeTeamGoals) > 1 ||
            Math.Abs(match.AwayTeamGoals - awayTeamGoals) > 1)
            throw new InvalidOperationException(Constants.ValidationMessages.CantAddMoreThanOneGoal);
        
        if (match.HomeTeamGoals != homeTeamGoals &&
            match.AwayTeamGoals != awayTeamGoals)
            throw new InvalidOperationException(Constants.ValidationMessages.CantUpdateBothTeams);
    }

    public void ValidateMatchExists(Match? match)
    {
        if (match is null)
            throw new ArgumentException(Constants.ValidationMessages.MatchNotFound);
    }
    
    private bool IsMatchInProgressForTeam(int teamId)
    {
        return LocalStorage.Matches.Any(x =>
            (x.HomeTeamId == teamId || x.AwayTeamId == teamId) && x.Status == MatchStatus.Started);
    }
}