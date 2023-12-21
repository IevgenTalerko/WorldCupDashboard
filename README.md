# World Cup Match Scoreboard 

World Cup Match Scoreboard  is a class library for live world cup matches results tracking.

## Usage

To add this library to your project you can convert it to nuget package, publish it and inject public interface **IMatchBoardService** with your DI container.

## Public methods

- StartMatch - add match to dashboard
- UpdateScore - change the match score
- FinishMatch - mark match as finished and remove it from livescore dashboard
- GetMatchesInProgress - get all pending matches with their scores

## Important Note

Hope for your positive feedback, Mr. Reviewer :)
