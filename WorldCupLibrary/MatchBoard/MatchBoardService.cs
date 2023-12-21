﻿using WorldCupLibrary.Entities;
using Match = WorldCupLibrary.Entities.Match;

namespace WorldCupLibrary.MatchBoard;

public class MatchBoardService : IMatchBoardService
{
    public Guid StartMatch(int homeTeamId, int awayTeamId)
    {
        ValidateStartMatch(homeTeamId, awayTeamId);
        
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
        matchToUpdate.HomeTeamGoals = homeTeamGoals;
        matchToUpdate.AwayTeamGoals = awayTeamGoals;
    }

    public void FinishMatch(Guid matchId)
    {
        var matchToFinish = LocalStorage.Matches.SingleOrDefault(x => x.Id == matchId);
        matchToFinish.Status = MatchStatus.Finished;
    }

    public List<Match> GetMatchesInProgress()
    {
        return LocalStorage.Matches
            .Where(x => x.Status == MatchStatus.Started)
            .OrderByDescending(x => x.HomeTeamGoals + x.AwayTeamGoals)
            .ThenByDescending(x => x.StartedOn)
            .ToList();
    }

    private void ValidateStartMatch(int homeTeamId, int awayTeamId)
    {
        if (IsMatchInProgressForTeam(homeTeamId) || IsMatchInProgressForTeam(awayTeamId))
            throw new InvalidOperationException("Only one match for the team can be in progress");
    }

    private bool IsMatchInProgressForTeam(int teamId)
    {
        return LocalStorage.Matches.Any(x =>
            (x.HomeTeamId == teamId || x.AwayTeamId == teamId) && x.Status == MatchStatus.Started);
    }
}