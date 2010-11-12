using System.Collections.Generic;
using Mogade.Configuration;

namespace Mogade.WindowsPhone
{
   public interface IStorage
   {
      int? GetGameVersion();
      GameConfiguration LoadConfiguration();
      string GetUniqueIdentifier();
      void Save(GameConfiguration configuration);

      ICollection<string> GetUserNames();
      void SaveUserName(string userName);
      void RemoveUserName(string userName);
   }
}