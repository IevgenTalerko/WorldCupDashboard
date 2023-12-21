using FluentAssertions;
using WorldCupLibrary.Entities;
using WorldCupLibrary.MatchBoard;
using Xunit;

namespace WorldCupLibrary.Tests;

public class MatchBoardTests
{
    private readonly LocalStorage _localStorage = new();
    private readonly IMatchBoardService _matchBoardService = new MatchBoardService();

    [Fact]
    public void ItStartsMatch()
    {
        // Arrange
        var homeTeam = _localStorage.Teams.First();
        var awayTeam = _localStorage.Teams.Last();
        
        // Act
        var matchId = _matchBoardService.StartMatch(homeTeam.Id, awayTeam.Id);

        // Assert
        var match = _localStorage.Matches.Single(x => x.Id == matchId);
        match.HomeTeamId.Should().Be(homeTeam.Id);
        match.AwayTeamId.Should().Be(awayTeam.Id);
        match.HomeTeamGoals.Should().Be(0);
        match.AwayTeamGoals.Should().Be(0);
        match.Status.Should().Be(MatchStatus.Started);
        match.StartedOn.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 5));
    }

    [Fact]
    public void ItFailsToStartTwoMatchesForSameTeam()
    {
        // Arrange
        var match = _localStorage.CreateMatch();
        
        // Act
        var matchId = _matchBoardService.Invoking(x => x.StartMatch(match.HomeTeamId, match.AwayTeamId))
            .Should().Throw<InvalidOperationException>("Only one match for the team can be in progress");
    }

    [Fact]
    public void ItUpdatesMatchScore()
    {
        // Arrange
        var match = _localStorage.CreateMatch();

        var homeTeamGoals = 1;
        var awayTeamGoals = 0;
        
        // Act
        _matchBoardService.UpdateScore(match.Id, homeTeamGoals, awayTeamGoals);
        
        // Assert
        var matchUpdated = _localStorage.Matches.Single(x => x.Id == match.Id);
        matchUpdated.HomeTeamGoals.Should().Be(homeTeamGoals);
        matchUpdated.AwayTeamGoals.Should().Be(homeTeamGoals);
    }

    [Fact]
    public void ItFailsToUpdateNotStartedMatch()
    {
        // Arrange
        var match = _localStorage.CreateMatch(x => x.Status = MatchStatus.Finished);
        
        var homeTeamGoals = 1;
        var awayTeamGoals = 0;
        
        // Act
        _matchBoardService.Invoking(x =>x.UpdateScore(match.Id, homeTeamGoals, awayTeamGoals))
            .Should().Throw<InvalidOperationException>("Only matches with Started status can be updated");
    }

    [Fact]
    public void ItFailsToUpdateBothTeamsInOneOperation()
    {
        // Arrange
        var match = _localStorage.CreateMatch();
        
        var homeTeamGoals = 1;
        var awayTeamGoals = 1;
        
        // Act
        _matchBoardService.Invoking(x =>x.UpdateScore(match.Id, homeTeamGoals, awayTeamGoals))
            .Should().Throw<InvalidOperationException>("Can't update both teams score in one operation");
    }

    [Fact]
    public void ItFailsToAddMoreThanOneGoal()
    {
        // Arrange
        var match = _localStorage.CreateMatch();
        
        var homeTeamGoals = 2;
        var awayTeamGoals = 0;
        
        // Act
        _matchBoardService.Invoking(x =>x.UpdateScore(match.Id, homeTeamGoals, awayTeamGoals))
            .Should().Throw<InvalidOperationException>("Can't add more than one goal in one operation");
    }

    [Fact]
    public void ItFinishesMatch()
    {
        // Arrange
        var match = _localStorage.CreateMatch();
        
        // Act
        _matchBoardService.FinishMatch(match.Id);
        
        // Assert
        var finishedMatch = _localStorage.Matches.Single(x => x.Id == match.Id);
        finishedMatch.Status.Should().Be(MatchStatus.Finished);
    }

    [Fact]
    public void ItReturnsMatchesInGoalsScoredOrder()
    {
        // Arrange
        var match1 = _localStorage.CreateMatch(x =>
        {
            x.HomeTeamId = 1;
            x.AwayTeamId = 2;
            x.HomeTeamGoals = 0;
            x.AwayTeamGoals = 0;
        });
        var match2 = _localStorage.CreateMatch(x =>
        {
            x.HomeTeamId = 3;
            x.AwayTeamId = 4;
            x.HomeTeamGoals = 1;
            x.AwayTeamGoals = 2;
        });
        var match3 = _localStorage.CreateMatch(x =>
        {
            x.HomeTeamId = 5;
            x.AwayTeamId = 6;
            x.HomeTeamGoals = 2;
            x.AwayTeamGoals = 3;
        });
        
        // Act
        var matches = _matchBoardService.GetMatchesInProgress();
        
        // Assert
        matches.Should().BeEquivalentTo(new List<Match> { match3, match2, match1 });
    }

    [Fact]
    public void ItReturnsMatchesInStartDateOrder()
    {
        // Arrange
        var match1 = _localStorage.CreateMatch(x =>
        {
            x.HomeTeamId = 1;
            x.AwayTeamId = 2;
            x.HomeTeamGoals = 2;
            x.AwayTeamGoals = 0;
            x.StartedOn = DateTime.Now.AddMinutes(-30);
        });
        var match2 = _localStorage.CreateMatch(x =>
        {
            x.HomeTeamId = 3;
            x.AwayTeamId = 4;
            x.HomeTeamGoals = 1;
            x.AwayTeamGoals = 1;
            x.StartedOn = DateTime.Now.AddMinutes(-60);
        });
        
        // Act
        var matches = _matchBoardService.GetMatchesInProgress();
        
        // Assert
        matches.Should().BeEquivalentTo(new List<Match> { match2, match1 });
    }

    [Fact]
    public void ItDoesNotReturnFinishedMatches()
    {
        // Arrange
        var match1 = _localStorage.CreateMatch();
        var match2 = _localStorage.CreateMatch(x =>
        {
            x.HomeTeamId = 3;
            x.AwayTeamId = 4;
            x.Status = MatchStatus.Finished;
        });
        
        // Act
        var matches = _matchBoardService.GetMatchesInProgress();
        
        // Assert
        matches.Should().BeEquivalentTo(new List<Match> { match1 });

    }
}