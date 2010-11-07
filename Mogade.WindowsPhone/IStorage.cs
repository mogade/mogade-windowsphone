using Mogade.Configuration;

namespace Mogade.WindowsPhone
{
   public interface IStorage
   {
      int? GetGameVersion();
      GameConfiguration LoadConfiguration();
      string GetUniqueIdentifier();

      void Save(GameConfiguration configuration);
   }
}