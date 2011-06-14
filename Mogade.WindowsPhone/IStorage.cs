using System.Collections.Generic;

namespace Mogade.WindowsPhone
{
   public interface IStorage
   {
      string GetUniqueIdentifier();
      ICollection<string> GetUserNames();
      void SaveUserName(string userName);
      void RemoveUserName(string userName);
   }
}