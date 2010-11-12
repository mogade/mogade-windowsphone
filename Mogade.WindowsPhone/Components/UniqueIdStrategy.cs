namespace Mogade.WindowsPhone
{
   /// <remarks>
   /// Unlike the DeviceId, UserId changes when the device changes owner. Microsoft recommends using 
   /// this value, but for all intents and purposes, mogade should work fine with either UserId or 
   /// DeviceId - use whichever you feel most comfortable requesting (or Generated if you don't wish to request any).
   /// </remarks>   
   public enum UniqueIdStrategy
   {
      /// <summary>
      /// The mogade library will generate a unique id and store it in isolated storage
      /// </summary>
      /// <remarks>
      /// This is the default choice as it doesn't require any additional steps, however
      /// some features might not be available (or fully functional) using this approach
      /// </remarks>
      Generated,

      /// <summary>
      /// Uses the device's Id
      /// </summary>
      /// <remarks>
      /// This approach requires that developers request the ID_CAP_IDENTITY_DEVICE capability
      /// in their Manifest.
      /// </remarks>
      DeviceId,

      /// <summary>
      /// Uses the user's windows live *anonymous* id
      /// </summary>
      /// <remarks>
      /// This approach requires that developers request the ID_CAP_IDENTITY_USER capability
      /// in their Manifest.      
      /// </remarks>
      UserId,
   }
}