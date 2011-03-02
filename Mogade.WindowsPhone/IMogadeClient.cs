using System;
using System.Collections.Generic;
using Mogade.Achievements;
using Mogade.Configuration;
using Mogade.Leaderboards;

namespace Mogade.WindowsPhone
{
   public interface IMogadeClient
   {

      /// <summary>
      /// Makes the underlying Mogade Driver directly availble
      /// </summary>
      IDriver Driver { get; }

      /// <summary>
      /// Updates the configuration if its changed
      /// </summary>
      /// <param name="callback">Will callback with true on success</param>
      void Update(Action<Response<bool>> callback);

      /// <summary>
      /// Returns the user's stored settings for this game
      /// </summary>            
      /// <returns>The user's settings</returns>
      /// <remarks>
      /// The achievements collection is a list of achievement ids the user has earned
      /// The leaderboards collection is a list of id=>points representing the user's top score for each leaderboard
      /// 
      /// In the case where a user hasn't completed an achievement, or earned a top score in a leaderboard
      /// the entries will simply be missing.
      /// 
      /// This is meant to be mapped agains the GameConfiguration object (returned from GetGameConfiguration) which
      /// returns user-agnostic game settings.
      /// </remarks>
      void GetUserSettings(string userName, Action<Response<UserSettings>> callback);

      /// <summary>
      /// Returns the game's configuration
      /// </summary>                        
      void GetGameConfiguration(Action<Response<GameConfiguration>> callback);

      /// <summary>
      /// Saves a score
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to save the score to</param>
      /// <param name="score">The required score to save</param>
      /// <returns>A rank object containing the daily, weekly, and overall rank of the supplied score for the given leaderboard</returns>
      /// <remarks>
      /// The username and points properties of the Score are required.
      /// Usernames should be 20 characters max
      /// 
      /// The data field is optional and limited to 25 characters. You can stuff meta information inside, such as "4|12:30", which
      /// might mean the user got to level 4 and played for 12 minutes and 30 seconds. You are responsible for encoding/decoding
      /// this information...we just take it in, store it, and pass it back out      
      /// </remarks>
      void SaveScore(string leaderboardId, Score score, Action<Response<Ranks>> callback);

      /// <summary>
      /// Get's the user's top rank for a given leaderboard for yesterday
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>      
      /// <returns>Returns 0 if the user doesn't have a rank</returns>
      void GetYesterdaysTopRank(string leaderboardId, string userName, Action<Response<int>> callback);

      /// <summary>
      /// Gets a leaderboard page
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="page">The page to get (starting with 1)</param>
      /// <returns>A leaderboard object containing an array of scores</returns>
      /// <remarks>
      /// Defaults to up to 10 scores per page
      /// </remarks>
      void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, Action<Response<LeaderboardScores>> callback);

      /// <summary>
      /// Gets a leaderboard page AND the user's score object for the specified leaderboard
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="page">The page to get (starting with 1)</param>
      /// <returns>A leaderboard object containing an array of scores</returns>
      /// <remarks>
      /// Defaults to up to 10 scores per page. The user's score object will only be returned when page = 1 and, of course, if the user has a score
      /// </remarks>
      void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, string userName, Action<Response<LeaderboardScoresWithUser>> callback);

      /// <summary>
      /// Gets a leaderboard page
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="page">The page to get (starting with 1)</param>
      /// <param name="records">The number of records (up to 50)</param>
      /// <returns>A leaderboard object containing an array of scores</returns>      
      void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, int records, Action<Response<LeaderboardScores>> callback);

      /// <summary>
      /// Gets a leaderboard page AND the user's score object for the specified leaderboard
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <param name="scope">The scope to get the scores from (daily, weekly or overall)</param>
      /// <param name="page">The page to get (starting with 1)</param>
      /// <param name="records">The number of records (up to 50)</param>
      /// <returns>A leaderboard object containing an array of scores</returns>
      /// <remarks>
      /// The user's score object will only be returned when page = 1 and, of course, if the user has a score
      /// </remarks>
      void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, int records, string userName, Action<Response<LeaderboardScoresWithUser>> callback);

      /// <summary>
      /// Gets the top 3 scores from yesterday
      /// </summary>
      /// <param name="leaderboardId">The id of the leaderboard to get the scores from</param>
      /// <returns>A leaderboard object containing an array of scores</returns
      void GetYesterdaysLeaders(string leaderboardId, Action<Response<LeaderboardScores>> callback);

      /// <summary>
      /// Grants the user the specified achievement
      /// </summary>
      /// <param name="achievementId">The id of the achievement being granted</param>
      /// <param name="userName">The user's username</param>      
      /// <returns>A partial achievement object, containing only the id and points of the earned achievement</returns>
      void GrantAchievement(string achievementId, string userName, Action<Response<Achievement>> callback);

      /// <summary>
      /// Grants the user the specified achievement
      /// </summary>
      /// <param name="achievementId">The achievement being granted</param>
      /// <param name="userName">The user's username</param>
      /// <param name="uniqueIdentifier">A unique identifier for the user. Mobile devices should use the deviceId.</param>
      /// <returns>A partial achievement object, containing only the id and points of the earned achievement</returns>
      void GrantAchievement(Achievement achievement, string userName, Action<Response<Achievement>> callback);

      /// <summary>
      /// Returns mogade's unique identifier for this device
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

      /// <summary>
      /// Logs an error
      /// </summary>
      /// <param name="subject">the subject of the error (a brief description)</param>
      /// <param name="details">the error's details</param>
      void LogError(string subject, string details);

      /// <summary>
      /// Logs that the appliction has started
      /// </summary>      
      void LogApplicationStart();
   }
}