﻿using WorldCupLibrary.Entities;

namespace WorldCupLibrary.MatchBoard;

public interface IMatchBoardService
{
    Guid StartMatch(int homeTeamId, int awayTeamId);

    void UpdateScore(Guid matchId, int homeTeamGoals, int awayTeamGoals);

    void FinishMatch(Guid matchId);

    List<Match> GetMatchesInProgress();
}