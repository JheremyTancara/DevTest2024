namespace Project.Core.Utils.ErrorMsg.Interface
{
  public interface ITypeValuesValidationMsg
  {
    string GetInvalidEnumValueMsg(string propertyName);
    string GetValidateDoubleMsg(string propertyName);
    string GetValidateIntMsg(string propertyName);
    string GetValidateStringMsg(string propertyName);
    string GetValidateBooleanMsg(string propertyName);
  }
}
