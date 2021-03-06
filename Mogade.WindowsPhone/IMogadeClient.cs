using System;
using System.Collections.Generic;

namespace Mogade.WindowsPhone
{
   public interface IMogadeClient
   {

      /// <summary>
      /// Makes the underlying Mogade Driver directly availble
      /// </summary>
      IDriver Driver { get; }

      /// <summary>
      /// Returns the version of the API this library understands
      /// </summary
      string ApiVersion { get; }

      /// <summary>
      /// Saves a score
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to save the score to</param>
      /// <param name="score">The required score to save</param>
      /// <returns>A rank object containing the daily, weekly, and overall rank of the user (or 0 if the user doesn't have a rank for the specific scope)</returns>
      /// <remarks>
      /// The username and points properties of the Score are required.
      /// Usernames should be 20 characters max
      /// 
      /// The data field is optional and limited to 50 characters. You can stuff meta information inside, such as "4|12:30", which
      /// might mean the user got to level 4 and played for 12 minutes and 30 seconds. You are responsible for encoding/decoding
      /// this information...we just take it in, store it, and pass it back out
      /// </remarks>
      void SaveScore(string leaderboardId, Score score, Action<Response<SavedScore>> callback);


      /// <summary>
      /// Gets a leaderboard page with a 10 records
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="page">The page to get (starting with 1)</param>
      /// <returns>A leaderboard object containing an array of scores</returns>
      void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page,  Action<Response<LeaderboardScores>> callback);

      /// <summary>
      /// Gets a leaderboard page with a specific number of records
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="page">The page to get (starting with 1)</param>
      /// <param name="records">The number of records (up to 50)</param>
      /// <returns>A leaderboard object containing an array of scores</returns>
      void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, int records, Action<Response<LeaderboardScores>> callback);

      /// <summary>
      /// Gets a leaderboard page with a specific number of records and the player's stats
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="userName">the name of the user</param>
      /// <param name="page">The page to get (starting with 1)</param>
      /// <param name="records">The number of records (up to 50)</param>
      /// <returns>A leaderboard object containing an array of scores</returns>
      void GetLeaderboardWithPlayerStats(string leaderboardId, LeaderboardScope scope, string userName,  int page, int records, Action<Response<LeaderboardScoresWithPlayerStats>> callback);

      /// <summary>
      /// Gets a player's score
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="userName">the name of the user</param>
      /// <returns>A leaderboard object containing an array of scores</returns>
      void GetPlayerScore(string leaderboardId, LeaderboardScope scope, string userName, Action<Response<Score>> callback);

      /// <summary>
      /// Gets a leaderboard located around the user's page
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="userName">the name of the user</param>
      /// <param name="records">The number of records (up to 50)</param>
      /// <returns>A leaderboard object containing an array of scores</returns>
      /// <remarks>
      /// Will return up to 10 records. The user's score object will only be returned when page = 1 and, of course, if the user has a score
      /// </remarks>
      void GetLeaderboard(string leaderboardId, LeaderboardScope scope, string userName,int records, Action<Response<LeaderboardScores>> callback);

      /// <summary>
      /// Gets the number of scores in a leaderboard
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <returns>the number of scores</returns>
      /// <remarks>
      /// the number of scores is limited to 25 000
      /// </remarks>
      void GetLeaderboardCount(string leaderboardId, LeaderboardScope scope, Action<Response<int>> callback);

      /// <summary>
      /// Gets the scores of the players  which are immediatly ahead of the specified player in the leaderboard
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="userName">the name of the user</param>
      /// <returns>The scores of the closest 3 players (could be length 2, 1 or 0)</returns>
      void GetRivals(string leaderboardId, LeaderboardScope scope, string userName, Action<Response<IList<Score>>> callback);

      /// <summary>
      /// Gets a a user's rank across all scopes
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="userName">the name of the user</param>
      /// <returns>Returns a rank object (0 means the user doesn't have a rank for the specified scope)</returns>
      void GetRanks(string leaderboardId, string userName,Action<Response<Ranks>> callback);

      /// <summary>
      /// Gets a a user's rank across an individual scope
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="userName">the name of the user</param>
      /// <param name="scope">The scope to get the rank for</param>
      /// <returns>Returns the user's rank (0 means the user doesn't have a rank for the specified scope)</returns>
      void GetRank(string leaderboardId, string userName, LeaderboardScope scope, Action<Response<int>> callback);

      /// <summary>
      /// Gets a a user's rank across an individual scope
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="userName">the name of the user</param>
      /// <param name="scopes">The scopes to get the rank for</param>
      /// <returns>Returns the user's rank (0 means the user doesn't have a rank for the specified scope, or that the scope wasn't requested)</returns>
      void GetRanks(string leaderboardId, string userName, LeaderboardScope[] scopes, Action<Response<Ranks>> callback);

      /// <summary>
      /// Gets the rank for a score across all scopes
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="score">The score to get the rank of</param>
      /// <returns>Returns a rank object (0 means the user doesn't have a rank for the specified scope)</returns>
      void GetRanks(string leaderboardId, int score, Action<Response<Ranks>> callback);

      /// <summary>
      /// Gets the rank for a score across an individual scope
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="score">The score to get the rank of</param>
      /// <param name="scope">The scope to get the rank for</param>
      /// <returns>Returns the user's rank (0 means the user doesn't have a rank for the specified scope)</returns>
      void GetRank(string leaderboardId, int score, LeaderboardScope scope, Action<Response<int>> callback);

      /// <summary>
      /// Gets the rank for a score acrossspecified scopes
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="score">The score to get the rank of</param>
      /// <param name="scopes">The scopes to get the rank for</param>
      /// <returns>Returns the user's rank (0 means the user doesn't have a rank for the specified scope, or that the scope wasn't requested)</returns>
      void GetRanks(string leaderboardId, int score, LeaderboardScope[] scopes, Action<Response<Ranks>> callback);

      /// <summary>
      /// Gets the game's achievements
      /// </summary>
      void GetAchievements(Action<Response<ICollection<Achievement>>> callback);

      /// <summary>
      /// Gets the achievement ids that the player has earned
      /// </summary>
      /// <param name="userName">the name of the user</param>
      /// <returns>An array containing the achievements earned by the user (or an empty array if the user hasn't earned anything)</returns>
      void GetEarnedAchievements(string userName, Action<Response<ICollection<string>>> callback);

      /// <summary>
      /// Grants the user the specified achievement
      /// </summary>
      /// <param name="achievementId">The id of the achievementId earned</param>
      /// <param name="userName">the name of the user</param>
      /// <returns>An array containing the achievements earned by the user (or an achievement with a null id if the user has already earned it)</returns>
      void AchievementEarned(string achievementId, string userName, Action<Response<Achievement>> callback);

      /// <summary>
      /// Logs an application start (for analytic purposes)
      /// </summary>
      void LogApplicationStart();

      /// <summary>
      /// Logs a hit for today to a custom stat counter
      /// </summary>
      /// <param name="index">The statistic to count (1-5)</param>
      void LogCustomStat(int index);

      /// <summary>
      /// Logs an error
      /// </summary>
      /// <param name="subject">the subject of the error (a brief description)</param>
      /// <param name="details">the error's details</param>
      void LogError(string subject, string details);

      /// <summary>
      /// Geta s game's assets
      /// </summary>
      void GetAssets(Action<Response<IList<Asset>>> callback);

      /// <summary>
      /// renames a user
      /// </summary>
      void Rename(string currentUserName, string newUserName, Action<Response<bool>> callback);

      /// <summary>
      /// Gets the unique identifier mogade is using for this device/user
      /// </summary>
      string GetUniqueIdentifier();


      /// <summary>
      /// Returns a list of previously saved usernames
      /// </summary>
      /// <remarks>
      /// This method works using isolated storage only. It can be useful if you don't already do user management
      /// but would like to present users with a convinient way to pick a name (from a list) when submitting a score
      /// or achievement (rather than having to type it in each time)
      /// </remarks>
      ICollection<string> GetUserNames();

      /// <summary>
      /// Adds a user to the list of locally stored username names
      /// </summary>
      /// <remarks>
      /// This will simply ignore duplicates
      /// </remarks>
      void SaveUserName(string userName);

      /// <summary>
      /// Adds a user to the list of locally stored username names
      /// </summary>
      /// <remarks>
      /// This will simply ignore duplicates
      /// </remarks>
      void RemoveUserName(string userName);
   }
}