namespace Paerux.Localization
{
   [System.Serializable]
   public struct Translation
   {
      public string language;
      public string value;
   }

   [System.Serializable]
   public struct KeyTranslation
   {
      public string key;
      public Translation[] translations;
   }

   public class LocalizationData
   {
      public string[] available_languages;
      public KeyTranslation[] table;
   }
}