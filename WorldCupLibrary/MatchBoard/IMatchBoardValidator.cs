using WorldCupLibrary.Entities;

namespace WorldCupLibrary.MatchBoard;

internal interface IMatchBoardValidator
{
    void ValidateStartMatch(int homeTeamId, int awayTeamId);

    void ValidateUpdateScore(Match? match, int homeTeamGoals, int awayTeamGoals);

    void ValidateMatchExists(Match? match);
}