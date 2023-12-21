namespace WorldCupLibrary.Entities;

public static class LocalStorage
{
    public static List<Team> Teams { get; set; } 

    public static List<Match> Matches { get; set; } 

    public static void Initilize()
    {
        Teams = new()
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
        
        Matches = new List<Match>();
    }
}