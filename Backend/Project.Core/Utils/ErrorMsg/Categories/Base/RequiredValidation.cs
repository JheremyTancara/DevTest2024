using Project.Core.Utils.ErrorMsg.Interface;

namespace Project.Core.Utils.ErrorMsg.Base
{
  public class RequiredValService : IRequiredValidationMsg
  {
    public string GetRequiredFieldMsg(string propertyName) => 
      RequiredValidationMsg.RequiredField.Replace("{PropertyName}", propertyName);

    public string GetMustBeAdultMsg(string propertyName) => 
      RequiredValidationMsg.MustBeAdult.Replace("{PropertyName}", propertyName);
  }
}
