using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Text;
using Mogade.Configuration;
using Newtonsoft.Json;
using Microsoft.Phone.Info;  

namespace Mogade.WindowsPhone
{
   public class Storage : IStorage
   {
      private const string _mogadeDataFile = "mogade.dat";
      private const string _userNamesDataFile = "usernames.dat";

      private readonly Configuration _configuration;
      private IList<string> _userNames;

      public Storage()
      {
         _configuration = Read<Configuration>(_mogadeDataFile);

         _userNames = Read<List<string>>(_userNamesDataFile) ?? new List<string>(1);
         if (_configuration == null)
         {
            _configuration = new Configuration {UniqueIdentifier = Guid.NewGuid().ToString()};
            WriteToFile(_configuration, _mogadeDataFile);
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
         WriteToFile(_configuration, _mogadeDataFile);
      }

      public ICollection<string> GetUserNames()
      {
         return _userNames;
      }

      public void SaveUserName(string userName)
      {
         if (string.IsNullOrEmpty(userName)) { return; }         
         if ( _userNames.Contains(userName)) { return; }
         _userNames.Add(userName);
         WriteToFile(_userNames, _userNamesDataFile);
      }

      public void RemoveUserName(string userName)
      {         
         if (string.IsNullOrEmpty(userName) || !_userNames.Remove(userName)) { return; }
         WriteToFile(_userNames, _userNamesDataFile);         
      }
      private static T Read<T>(string dataFile)
      {
         using (var store = IsolatedStorageFile.GetUserStoreForApplication())
         using (var stream = new IsolatedStorageFileStream(dataFile, System.IO.FileMode.OpenOrCreate, store))
         {
            if (stream.Length <= 0) { return default(T); }
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
         }        
      }
      private static void WriteToFile(object objectToWrite, string dataFile)
      {
         using (var store = IsolatedStorageFile.GetUserStoreForApplication())         
         using (var stream = new IsolatedStorageFileStream(dataFile, System.IO.FileMode.Create, store))
         {
            var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objectToWrite));
            stream.Write(buffer, 0, buffer.Length);            
         }         
      }
   }
}