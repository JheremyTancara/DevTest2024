namespace Project.Core.Utils.ErrorMsg.Interface
{
  public interface IFormatValidationMsg
  {
    string GetInvalidEmailFormatMsg(string propertyName);
    string GetInvalidPasswordFormatMsg(string propertyName);
    string GetInvalidTimeFormatMsg(string propertyName);
    string GetInvalidImageUrlMsg(string propertyName);
    string GetInvalidYouTubeUrlMsg(string propertyName);
  }
}
