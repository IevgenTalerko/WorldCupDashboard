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

    public List<Match> Matches { get; set; } = new();
}