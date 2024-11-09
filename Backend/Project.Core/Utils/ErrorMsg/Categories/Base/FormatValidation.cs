using Project.Core.Utils.ErrorMsg.Interface;

namespace Project.Core.Utils.ErrorMsg.Base
{
  public class FormatValService : IFormatValidationMsg
  {
    public string GetInvalidEmailFormatMsg(string propertyName) => 
      FormatValidationMsg.InvalidEmailFormat.Replace("{PropertyName}", propertyName);

    public string GetInvalidPasswordFormatMsg(string propertyName) => 
      FormatValidationMsg.InvalidPasswordFormat.Replace("{PropertyName}", propertyName);

    public string GetInvalidTimeFormatMsg(string propertyName) => 
      FormatValidationMsg.InvalidTimeFormat.Replace("{PropertyName}", propertyName);

    public string GetInvalidImageUrlMsg(string propertyName) => 
      FormatValidationMsg.InvalidImageUrl.Replace("{PropertyName}", propertyName);

    public string GetInvalidYouTubeUrlMsg(string propertyName) => 
      FormatValidationMsg.InvalidYouTubeUrl.Replace("{PropertyName}", propertyName);
  }
}
