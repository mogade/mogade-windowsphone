namespace Mogade.WindowsPhone
{
   public interface IConfigurationData
   {
      UniqueIdStrategy UniqueIdStrategy { get; }
   }

   public class ConfigurationData : IConfigurationData
   {
      private UniqueIdStrategy _uniqueIdStrategy = UniqueIdStrategy.UserId;

      public UniqueIdStrategy UniqueIdStrategy
      {
         get { return _uniqueIdStrategy; }
         set { _uniqueIdStrategy = value; }
      }
   }
}