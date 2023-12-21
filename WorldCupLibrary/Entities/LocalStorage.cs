namespace WorldCupLibrary.Entities;

public static class LocalStorage
{
    public static List<Team> Teams { get; set; } 

    public static List<Match> Matches { get; set; } 

    public static void Initilize()
    {
        Teams = new List<Team>();
        foreach (var name in Enum.GetNames(typeof(KnownTeams)))
        {
            Teams.Add(new Team
            {
                Id = (int)Enum.Parse(typeof(KnownTeams), name),
                Name = name
            });
        }
        
        Matches = new List<Match>();
    }
}

enum KnownTeams
{
    England = 1,
    Germany = 2, 
    France = 3,
    Spain = 4,
    Brazil = 5,
    Argentina = 6,
    Italy = 7,
    Netherlands = 8
}