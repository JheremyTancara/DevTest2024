namespace Project.Core.Utils.ErrorMsg.Interface
{
  public interface IRequiredValidationMsg
  {
    string GetRequiredFieldMsg(string propertyName);
    string GetMustBeAdultMsg(string propertyName);
  }
}
