namespace Project.Core.Utils.ErrorMsg.Interface
{
  public interface IRangeValidationMsg
  {
    string GetRangeErrorMsg(string propertyName, int valueMin, int valueMax);
    string GetRangeErrorMsg(string propertyName, double valueMin, double valueMax);
    string GetGreaterThanOrEqualErrorMsg(string propertyName, int value);
    string GetLessThanOrEqualErrorMsg(string propertyName, int value);
    string GetRangeLengthMsg(string propertyName, int minLength, int maxLength);
  }
}
