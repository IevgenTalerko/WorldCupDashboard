using FluentAssertions;
using WorldCupLibrary.Entities;
using WorldCupLibrary.MatchBoard;
using Xunit;

namespace WorldCupLibrary.Tests;

public class MatchBoardTests
{
    private readonly IMatchBoardService _matchBoardService = new MatchBoardService();

    public MatchBoardTests()
    {
        LocalStorage.Initilize();
    }

    [Fact]
    public void ItStartsMatch()
    {
        // Arrange
        var homeTeam = LocalStorage.Teams.First();
        var awayTeam = LocalStorage.Teams.Last();
        
        // Act
        var matchId = _matchBoardService.StartMatch(homeTeam.Id, awayTeam.Id);

        // Assert
        var match = LocalStorage.Matches.Single(x => x.Id == matchId);
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
        var match = MatchBoardTestHelper.CreateMatch();
        
        // Act
        _matchBoardService.Invoking(x => x.StartMatch(match.HomeTeamId, match.AwayTeamId))
            .Should().Throw<InvalidOperationException>()
            .WithMessage(Constants.ValidationMessages.OneMatchInProgress);
    }

    [Fact]
    public void ItUpdatesMatchScore()
    {
        // Arrange
        var match = MatchBoardTestHelper.CreateMatch();

        var homeTeamGoals = 1;
        var awayTeamGoals = 0;
        
        // Act
        _matchBoardService.UpdateScore(match.Id, homeTeamGoals, awayTeamGoals);
        
        // Assert
        var matchUpdated = LocalStorage.Matches.Single(x => x.Id == match.Id);
        matchUpdated.HomeTeamGoals.Should().Be(homeTeamGoals);
        matchUpdated.AwayTeamGoals.Should().Be(awayTeamGoals);
    }

    [Fact]
    public void ItFailsToUpdateNotStartedMatch()
    {
        // Arrange
        var match = MatchBoardTestHelper.CreateMatch(x => x.Status = MatchStatus.Finished);
        
        var homeTeamGoals = 1;
        var awayTeamGoals = 0;
        
        // Act
        _matchBoardService.Invoking(x =>x.UpdateScore(match.Id, homeTeamGoals, awayTeamGoals))
            .Should().Throw<InvalidOperationException>()
            .WithMessage(Constants.ValidationMessages.StartedStatus);
    }

    [Fact]
    public void ItFailsToUpdateBothTeamsInOneOperation()
    {
        // Arrange
        var match = MatchBoardTestHelper.CreateMatch();
        
        var homeTeamGoals = 1;
        var awayTeamGoals = 1;
        
        // Act
        _matchBoardService.Invoking(x =>x.UpdateScore(match.Id, homeTeamGoals, awayTeamGoals))
            .Should().Throw<InvalidOperationException>()
            .WithMessage(Constants.ValidationMessages.CantUpdateBothTeams);
    }

    [Fact]
    public void ItFailsToAddMoreThanOneGoal()
    {
        // Arrange
        var match = MatchBoardTestHelper.CreateMatch();
        
        var homeTeamGoals = 2;
        var awayTeamGoals = 0;
        
        // Act
        _matchBoardService.Invoking(x =>x.UpdateScore(match.Id, homeTeamGoals, awayTeamGoals))
            .Should().Throw<InvalidOperationException>()
            .WithMessage(Constants.ValidationMessages.CantAddMoreThanOneGoal);
    }

    [Fact]
    public void ItFinishesMatch()
    {
        // Arrange
        var match = MatchBoardTestHelper.CreateMatch();
        
        // Act
        _matchBoardService.FinishMatch(match.Id);
        
        // Assert
        var finishedMatch = LocalStorage.Matches.Single(x => x.Id == match.Id);
        finishedMatch.Status.Should().Be(MatchStatus.Finished);
    }

    [Fact]
    public void ItReturnsMatchesInGoalsScoredOrder()
    {
        // Arrange
        var match1 = MatchBoardTestHelper.CreateMatch(x =>
        {
            x.HomeTeamId = 1;
            x.AwayTeamId = 2;
            x.HomeTeamGoals = 0;
            x.AwayTeamGoals = 0;
        });
        var match2 = MatchBoardTestHelper.CreateMatch(x =>
        {
            x.HomeTeamId = 3;
            x.AwayTeamId = 4;
            x.HomeTeamGoals = 1;
            x.AwayTeamGoals = 2;
        });
        var match3 = MatchBoardTestHelper.CreateMatch(x =>
        {
            x.HomeTeamId = 5;
            x.AwayTeamId = 6;
            x.HomeTeamGoals = 2;
            x.AwayTeamGoals = 3;
        });
        
        // Act
        var matches = _matchBoardService.GetMatchesInProgress();
        
        // Assert
        matches.Select(x => x.Id).ToArray()
            .Should().BeEquivalentTo(new [] { match3.Id, match2.Id, match1.Id });
    }

    [Fact]
    public void ItReturnsMatchesInStartDateOrder()
    {
        // Arrange
        var match1 = MatchBoardTestHelper.CreateMatch(x =>
        {
            x.HomeTeamId = 1;
            x.AwayTeamId = 2;
            x.HomeTeamGoals = 2;
            x.AwayTeamGoals = 0;
            x.StartedOn = DateTime.Now.AddMinutes(-30);
        });
        var match2 = MatchBoardTestHelper.CreateMatch(x =>
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
        matches.Select(x => x.Id).ToArray()
            .Should().BeEquivalentTo(new [] { match2.Id, match1.Id });
    }

    [Fact]
    public void ItDoesNotReturnFinishedMatches()
    {
        // Arrange
        var match1 = MatchBoardTestHelper.CreateMatch();
        var match2 = MatchBoardTestHelper.CreateMatch(x =>
        {
            x.HomeTeamId = 3;
            x.AwayTeamId = 4;
            x.Status = MatchStatus.Finished;
        });
        
        // Act
        var matches = _matchBoardService.GetMatchesInProgress();
        
        // Assert
        var match = matches.Single();
        match.Id.Should().Be(match1.Id);
    }

    [Fact]
    public void ItFailsWhenTryToUpdateNotExistingMatch()
    {
        // Arrange
        var homeTeamGoals = 1;
        var awayTeamGoals = 0;
        
        // Act
        _matchBoardService.Invoking(x => x.UpdateScore(Guid.NewGuid(), homeTeamGoals, awayTeamGoals))
            .Should().Throw<ArgumentException>()
            .WithMessage(Constants.ValidationMessages.MatchNotFound);
    }

    [Fact]
    public void ItFailsWhenTryToFinishNotExistingMatch()
    {
        // Act
        _matchBoardService.Invoking(x => x.FinishMatch(Guid.NewGuid()))
            .Should().Throw<ArgumentException>()
            .WithMessage(Constants.ValidationMessages.MatchNotFound);

    }

    [Fact]
    public void ItFailsToStartMatchIfTeamDoesNotExist()
    {
        _matchBoardService.Invoking(x => x.StartMatch(55, 33))
            .Should().Throw<ArgumentException>()
            .WithMessage(Constants.ValidationMessages.TeamNotFound);
    }
}