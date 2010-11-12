using System;
using System.IO.IsolatedStorage;
using System.Text;
using Mogade.Configuration;
using Newtonsoft.Json;
using Microsoft.Phone.Info;  

namespace Mogade.WindowsPhone
{
   public class Storage : IStorage
   {      
      private readonly Configuration _configuration;

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
         if (MogadeConfiguration.Data.UniqueIdStrategy == UniqueIdStrategy.DeviceId)
         {
            object raw;  
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out raw) && raw != null)
            {
               var bytes = (byte[]) raw;
               var sb = new StringBuilder(bytes.Length * 2);
               for(var i = 0; i < bytes.Length; ++i)
               {
                  sb.Append(bytes[i].ToString("X2"));
               }
               return sb.ToString();
            }
         }
         else if (MogadeConfiguration.Data.UniqueIdStrategy == UniqueIdStrategy.UserId)
         {
            object anid;
            if (UserExtendedProperties.TryGetValue("ANID", out anid) && anid != null)
            {
               return anid.ToString();
            } 
         }
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