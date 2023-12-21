namespace WorldCupLibrary;

public class Constants
{
    public class ValidationMessages
    {
        public const string OneMatchInProgress = "Only one match for the team can be in progress";
        public const string StartedStatus = "Only matches with Started status can be updated";
        public const string CantUpdateBothTeams = "Can't update both teams score in one operation";
        public const string CantAddMoreThanOneGoal = "Can't add more than one goal in one operation";
        public const string MatchNotFound = "Match not found";
        public const string TeamNotFound = "Team not found";
    }
}