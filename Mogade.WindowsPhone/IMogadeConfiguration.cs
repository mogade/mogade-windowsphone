using System;

namespace Mogade.WindowsPhone
{
   public interface IMogadeConfiguration
   {
      IMogadeConfiguration ConnectToTest();
      IMogadeConfiguration ConnectTo(string url);
      IMogadeConfiguration UsingUniqueIdStrategy(UniqueIdStrategy strategyToUse);
   }

   public class MogadeConfiguration : IMogadeConfiguration
   {
      private static readonly ConfigurationData _data = new ConfigurationData();
      private static readonly MogadeConfiguration _configuration = new MogadeConfiguration();
      private MogadeConfiguration() { }

      public static IConfigurationData Data
      {
         get { return _data; }
      }

      public static void Configuration(Action<IMogadeConfiguration> action)
      {
         action(_configuration);
      }

      public IMogadeConfiguration ConnectToTest()
      {
         DriverConfiguration.Configuration(c => c.ConnectToTest());
         return this;
      }

      public IMogadeConfiguration ConnectTo(string url)
      {
         DriverConfiguration.Configuration(c => c.ConnectTo(url));
         return this;
      }

      public IMogadeConfiguration UsingUniqueIdStrategy(UniqueIdStrategy strategyToUse)
      {
         _data.UniqueIdStrategy = strategyToUse;
         return this;
      }
   }
}