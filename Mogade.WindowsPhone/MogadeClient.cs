using System;
using System.Collections.Generic;
using Mogade.Achievements;
using Mogade.Configuration;
using Mogade.Leaderboards;

namespace Mogade.WindowsPhone
{
   public class MogadeClient : IMogadeClient
   {
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

      public void Update(Action<bool> callback)
      {
         var currentVersion = _storage.GetGameVersion();
         if (currentVersion == null)
         {
            UpdateFromServer(callback);
            return;
         }

         _driver.GetGameVersion(v =>
         {
            if (v != currentVersion)
            {
               UpdateFromServer(callback);
            }
         });
      }

      public void GetUserSettings(string userName, Action<UserSettings> callback)
      {
         _driver.GetUserSettings(userName, GetUniqueIdentifier(), callback);
      }

      public void GetGameConfiguration(Action<GameConfiguration> callback)
      {
         _driver.GetGameConfiguration(callback);
      }

      public void SaveScore(string leaderboardId, Score score, Action<Ranks> callback)
      {         
         _driver.SaveScore(leaderboardId, score,  GetUniqueIdentifier(), callback);
      }

      public void GetLeaderboard(string leaderboardId, LeaderboardScope scope, int page, Action<LeaderboardScores> callback)
      {
         _driver.GetLeaderboard(leaderboardId, scope, page, callback);
      }

      public void GrantAchievement(string achievementId, string userName, Action<int> callback)
      {
         _driver.GrantAchievement(achievementId, userName, GetUniqueIdentifier(), callback);
      }

      public void GrantAchievement(Achievement achievement, string userName, Action<int> callback)
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

      private void UpdateFromServer(Action<bool> callback)
      {
         _driver.GetGameConfiguration(g =>
         {
            _storage.Save(g);
            if (callback != null) { callback(true); }
         });
      }
   }
}