using WorldCupLibrary.Entities;

namespace WorldCupLibrary.Models;

public class MatchModel
{
    public Guid Id { get; set; }
    
    public string HomeTeamName { get; set; }

    public string AwayTeamName { get; set; }

    public int HomeTeamGoals { get; set; }
    
    public int AwayTeamGoals { get; set; }

    public DateTime StartedOn { get; set; }
}