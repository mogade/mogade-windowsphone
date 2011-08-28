using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Mogade.WindowsPhone
{
   public class MogadeClient : IMogadeClient
   {
      static MogadeClient()
      {
         DriverConfiguration.Configuration(c => c.NetworkAvailableCheck(NetworkInterface.GetIsNetworkAvailable));
      }

      private readonly IDriver _driver;
      private readonly IStorage _storage;
      
      public static IMogadeClient Initialize(string gameKey, string secret)
      {
         return new MogadeClient(gameKey, secret);
      }

      private MogadeClient(string gameKey, string secret)
      {
         _driver = new Driver(gameKey, secret);
         _storage = new Storage();
      }

      public IDriver Driver
      {
         get { return _driver; }
      }

      public string ApiVersion
      {
         get { return _driver.ApiVersion; }
      }

      public void SaveScore(string leaderboardId, Score score, Action<Response<SavedScore>> callback)
      {
         _driver.SaveScore(leaderboardId, score, GetUniqueIdentifier(), callback);
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, Action<Response<LeaderboardScores>> callback)
      {
         _driver.GetLeaderboard(leaderboardId, scope, page, 10, callback);
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, int records, Action<Response<LeaderboardScores>> callback)
      {
         _driver.GetLeaderboard(leaderboardId, scope, page, records, callback);
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, string userName, Action<Response<Score>> callback)
      {
         _driver.GetLeaderboard(leaderboardId, scope, userName, GetUniqueIdentifier(), callback);
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, string userName, int records, Action<Response<LeaderboardScores>> callback)
      {
         _driver.GetLeaderboard(leaderboardId, scope, userName, GetUniqueIdentifier(), records, callback);
      }

      public void GetLeaderboardCount(string leaderboardId, LeaderboardScope scope, Action<Response<int>> callback)
      {
         _driver.GetLeaderboardCount(leaderboardId, scope, callback);
      }

      public void GetRanks(string leaderboardId, string userName, Action<Response<Ranks>> callback)
      {
         _driver.GetRanks(leaderboardId, userName, GetUniqueIdentifier(), callback);
      }

      public void GetRank(string leaderboardId, string userName, LeaderboardScope scope, Action<Response<int>> callback)
      {
         _driver.GetRank(leaderboardId, userName, GetUniqueIdentifier(), scope, callback);
      }

      public void GetRanks(string leaderboardId, string userName, LeaderboardScope[] scopes, Action<Response<Ranks>> callback)
      {
         _driver.GetRanks(leaderboardId, userName, GetUniqueIdentifier(), scopes, callback);
      }

      public void GetRanks(string leaderboardId, int score, Action<Response<Ranks>> callback)
      {
         _driver.GetRanks(leaderboardId, score, callback);
      }

      public void GetRank(string leaderboardId, int score, LeaderboardScope scope, Action<Response<int>> callback)
      {
         _driver.GetRank(leaderboardId, score, scope, callback);
      }

      public void GetRanks(string leaderboardId, int score, LeaderboardScope[] scopes, Action<Response<Ranks>> callback)
      {
         _driver.GetRanks(leaderboardId, score, scopes, callback);
      }

      public void GetEarnedAchievements(string userName, Action<Response<ICollection<string>>> callback)
      {
         _driver.GetEarnedAchievements(userName, GetUniqueIdentifier(), callback);
      }

      public void AchievementEarned(string achievementId, string userName, Action<Response<Achievement>> callback)
      {
         _driver.AchievementEarned(achievementId, userName, GetUniqueIdentifier(), callback);
      }

      public void LogApplicationStart()
      {
         _driver.LogApplicationStart(GetUniqueIdentifier(), null);
      }

      public void LogError(string subject, string details)
      {
         _driver.LogError(subject, details, null);
      }

      public void GetAssets(Action<Response<IList<Asset>>> callback)
      {
         _driver.GetAssets(callback);
      }

      public string GetUniqueIdentifier()
      {
         return _storage.GetUniqueIdentifier();
      }

      public ICollection<string> GetUserNames()
      {
         return _storage.GetUserNames();
      }

      public void SaveUserName(string userName)
      {
         _storage.SaveUserName(userName);
      }

      public void RemoveUserName(string userName)
      {
         _storage.RemoveUserName(userName);
      }
   }
}