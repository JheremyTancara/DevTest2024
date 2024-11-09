namespace Project.Core.Utils.ErrorMsg.Interface
{
  public interface IContentValidationMsg
  {
    string GetNoNumbersMsg(string propertyName);
    string GetNoSpacesMsg(string propertyName);
    string GetNoSpecialCharactersMsg(string propertyName);
    string GetValidateNumericStringMsg(string propertyName);
  }
}
