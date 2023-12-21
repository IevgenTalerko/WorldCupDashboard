namespace WorldCupLibrary.Entities;

public class LocalStorage
{
    public List<Team> Teams { get; set; } = new()
    {
        new Team { Id = 1, Name = "England" },
        new Team { Id = 2, Name = "Germany" },
        new Team { Id = 3, Name = "France" },
        new Team { Id = 4, Name = "Spain" },
        new Team { Id = 5, Name = "Brazil" },
        new Team { Id = 6, Name = "Argentina" },
        new Team { Id = 7, Name = "Italy" },
        new Team { Id = 8, Name = "Netherlands" },
    };

    public List<Match> Matches { get; set; } = new()
    {
        new Match { Id = Guid.NewGuid(), HomeTeamId = 1, AwayTeamId = 5, HomeTeamGoals = 3, AwayTeamGoals = 0, Status = MatchStatus.Finished, StartedOn = DateTime.Now.AddHours(-1)},
        new Match { Id = Guid.NewGuid(), HomeTeamId = 2, AwayTeamId = 6, HomeTeamGoals = 1, AwayTeamGoals = 4, Status = MatchStatus.Finished, StartedOn = DateTime.Now.AddHours(-1) },
        new Match { Id = Guid.NewGuid(), HomeTeamId = 3, AwayTeamId = 7, HomeTeamGoals = 2, AwayTeamGoals = 3, Status = MatchStatus.Finished, StartedOn = DateTime.Now.AddHours(-1) },
        new Match { Id = Guid.NewGuid(), HomeTeamId = 4, AwayTeamId = 8, HomeTeamGoals = 0, AwayTeamGoals = 1, Status = MatchStatus.Finished, StartedOn = DateTime.Now.AddHours(-1) },
    };
}