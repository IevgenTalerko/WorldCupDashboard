namespace WorldCupLibrary.Entities;

public class Match
{
    public Guid Id { get; set; }
    
    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public int HomeTeamGoals { get; set; }
    
    public int AwayTeamGoals { get; set; }

    public MatchStatus Status { get; set; }
    
    public DateTime StartedOn { get; set; }
}

public enum MatchStatus
{
    Started = 0,
    Finished = 1
}