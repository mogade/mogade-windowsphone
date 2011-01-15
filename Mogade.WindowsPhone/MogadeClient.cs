using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Mogade.Achievements;
using Mogade.Configuration;
using Mogade.Leaderboards;

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

      public void Update(Action<Response<bool>> callback)
      {
         var currentVersion = _storage.GetGameVersion();
         if (currentVersion == null)
         {
            UpdateFromServer(callback);
            return;
         }

         _driver.GetGameVersion(v =>
         {
            if (v.Data != currentVersion)
            {
               UpdateFromServer(callback);
            }
         });
      }

      public void GetUserSettings(string userName, Action<Response<UserSettings>> callback)
      {
         _driver.GetUserSettings(userName, GetUniqueIdentifier(), callback);
      }

      public void GetGameConfiguration(Action<Response<GameConfiguration>> callback)
      {
         _driver.GetGameConfiguration(callback);
      }

      public void SaveScore(string leaderboardId, Score score, Action<Response<Ranks>> callback)
      {         
         _driver.SaveScore(leaderboardId, score,  GetUniqueIdentifier(), callback);
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, Action<Response<LeaderboardScores>> callback)
      {
         _driver.GetLeaderboard(leaderboardId, scope, page, callback);
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, string userName, Action<Response<LeaderboardScoresWithUser>> callback)
      {
         _driver.GetLeaderboard(leaderboardId, scope, page, userName, GetUniqueIdentifier(), callback);
      }

      public void GrantAchievement(string achievementId, string userName, Action<Response<int>> callback)
      {
         _driver.GrantAchievement(achievementId, userName, GetUniqueIdentifier(), callback);
      }

      public void GrantAchievement(Achievement achievement, string userName, Action<Response<int>> callback)
      {
         _driver.GrantAchievement(achievement, userName, GetUniqueIdentifier(), callback);
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

      public void LogError(string subject, string details)
      {
         _driver.LogError(subject, details);
      }

      public void LogApplicationStart()
      {
         _driver.LogApplicationStart(GetUniqueIdentifier());
      }


      private void UpdateFromServer(Action<Response<bool>> callback)
      {
         _driver.GetGameConfiguration(g =>
         {
            if (g.Success) { _storage.Save(g.Data); }
            if (callback != null)
            {
               var response = Response<bool>.CreateSuccess(g.Success.ToString());
               response.Data = g.Success;
               callback(response);
            }               
         });
      }
   }
}