using WorldCupLibrary.Entities;
using WorldCupLibrary.Models;
using Match = WorldCupLibrary.Entities.Match;

namespace WorldCupLibrary.MatchBoard;

public class MatchBoardService : IMatchBoardService
{
    private readonly IMatchBoardValidator _matchBoardValidator = new MatchBoardValidator();

    public Guid StartMatch(int homeTeamId, int awayTeamId)
    {
        _matchBoardValidator.ValidateStartMatch(homeTeamId, awayTeamId);
        
        var newMatch = new Match
        {
            Id = Guid.NewGuid(),
            HomeTeamId = homeTeamId,
            AwayTeamId = awayTeamId,
            HomeTeamGoals = 0,
            AwayTeamGoals = 0,
            Status = MatchStatus.Started,
            StartedOn = DateTime.Now
        };
        
        LocalStorage.Matches.Add(newMatch);

        return newMatch.Id;
    }

    public void UpdateScore(Guid matchId, int homeTeamGoals, int awayTeamGoals)
    {
        var matchToUpdate = LocalStorage.Matches.SingleOrDefault(x => x.Id == matchId);
        _matchBoardValidator.ValidateUpdateScore(matchToUpdate, homeTeamGoals, awayTeamGoals);
        
        matchToUpdate.HomeTeamGoals = homeTeamGoals;
        matchToUpdate.AwayTeamGoals = awayTeamGoals;
    }

    public void FinishMatch(Guid matchId)
    {
        var matchToFinish = LocalStorage.Matches.SingleOrDefault(x => x.Id == matchId);
        _matchBoardValidator.ValidateMatchExists(matchToFinish);
        
        matchToFinish.Status = MatchStatus.Finished;
    }

    public List<MatchModel> GetMatchesInProgress()
    {
        return LocalStorage.Matches
            .Where(x => x.Status == MatchStatus.Started)
            .Select(x => new MatchModel
            {
                Id = x.Id,
                StartedOn = x.StartedOn,
                AwayTeamGoals = x.AwayTeamGoals,
                HomeTeamGoals = x.HomeTeamGoals,
                AwayTeamName = GetTeamNameById(x.AwayTeamId),
                HomeTeamName = GetTeamNameById(x.HomeTeamId)
            })
            .OrderByDescending(x => x.HomeTeamGoals + x.AwayTeamGoals)
            .ThenByDescending(x => x.StartedOn)
            .ToList();
    }

    private string GetTeamNameById(int id) => LocalStorage.Teams.Single(x => x.Id == id).Name;
}