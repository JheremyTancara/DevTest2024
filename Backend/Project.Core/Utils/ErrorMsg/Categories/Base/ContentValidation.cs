using Project.Core.Utils.ErrorMsg.Interface;

namespace Project.Core.Utils.ErrorMsg.Base
{
  public class ContentValService : IContentValidationMsg
  {
    public string GetNoNumbersMsg(string propertyName) => 
      ContentValidationMsg.NoNumbers.Replace("{PropertyName}", propertyName);

    public string GetNoSpacesMsg(string propertyName) => 
      ContentValidationMsg.NoSpaces.Replace("{PropertyName}", propertyName);

    public string GetNoSpecialCharactersMsg(string propertyName) => 
      ContentValidationMsg.NoSpecialCharacters.Replace("{PropertyName}", propertyName);

    public string GetValidateNumericStringMsg(string propertyName) => 
      ContentValidationMsg.ValidateNumericString.Replace("{PropertyName}", propertyName);
  }
}
