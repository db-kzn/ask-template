namespace ASK.Shared;

public class UserPreferences
{
  public bool IsDarkMode { get; set; } = false;
  public string LanguageCode { get; set; } = "ru"; // en, ru
  public bool AutoSave { get; set; } = true;
}
