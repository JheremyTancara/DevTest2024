using Project.Core.Utils.ErrorMsg.Interface;

namespace Project.Core.Utils.ErrorMsg.Base
{
  public class TypeValuesValService : ITypeValuesValidationMsg
  {
    public string GetInvalidEnumValueMsg(string propertyName) =>
      TypeValuesValidationMsg.TypeValues.InvalidEnumValue.Replace("{PropertyName}", propertyName);
    
    public string GetValidateDoubleMsg(string propertyName) =>
      TypeValuesValidationMsg.TypeValues.ValidateDouble.Replace("{PropertyName}", propertyName);
    
    public string GetValidateIntMsg(string propertyName) =>
      TypeValuesValidationMsg.TypeValues.ValidateInt.Replace("{PropertyName}", propertyName);
    
    public string GetValidateStringMsg(string propertyName) =>
      TypeValuesValidationMsg.TypeValues.ValidateString.Replace("{PropertyName}", propertyName);
    
    public string GetValidateBooleanMsg(string propertyName) =>
      TypeValuesValidationMsg.TypeValues.ValidateBoolean.Replace("{PropertyName}", propertyName);
  }
}
