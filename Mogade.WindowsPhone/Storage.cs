using System;
using System.IO.IsolatedStorage;
using System.Text;
using Mogade.Configuration;
using Newtonsoft.Json;

namespace Mogade.WindowsPhone
{
   public class Storage : IStorage
   {
      private Configuration _configuration;

      public Storage()
      {
         _configuration = ReadConfiguration();
         if (_configuration == null)
         {
            _configuration = new Configuration {UniqueIdentifier = Guid.NewGuid().ToString()};
            WriteConfiguration(_configuration);
         }         
      }

      public int? GetGameVersion()
      {
         return _configuration.GameConfiguration == null ? null : (int?)_configuration.GameConfiguration.Version;
      }

      public GameConfiguration LoadConfiguration()
      {
         return _configuration.GameConfiguration;
      }

      public string GetUniqueIdentifier()
      {
         return _configuration.UniqueIdentifier;
      }

      public void Save(GameConfiguration configuration)
      {
         _configuration.GameConfiguration = configuration;
         WriteConfiguration(_configuration);
      }


      private static Configuration ReadConfiguration()
      {
         using (var store = IsolatedStorageFile.GetUserStoreForApplication())
         using (var stream = new IsolatedStorageFileStream("mogade.dat", System.IO.FileMode.OpenOrCreate, store))
         {
            if (stream.Length <= 0) { return null; }
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return JsonConvert.DeserializeObject<Configuration>(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
         }        
      }
      private static void WriteConfiguration(Configuration configuration)
      {
         using (var store = IsolatedStorageFile.GetUserStoreForApplication())         
         using (var stream = new IsolatedStorageFileStream("mogade.dat", System.IO.FileMode.Create, store))
         {
            var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(configuration));
            stream.Write(buffer, 0, buffer.Length);            
         }         
      }
   }
}