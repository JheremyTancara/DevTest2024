using Project.Core.Utils.ErrorMsg.Interface;

namespace Project.Core.Utils.ErrorMsg.Base
{
  public class RangeValService : IRangeValidationMsg
  {
    public string GetRangeErrorMsg(string propertyName, int valueMin, int valueMax) =>
      RangeValidationMsg.RangeErrorMessage
        .Replace("{PropertyName}", propertyName)
        .Replace("{valueMin}", valueMin.ToString())
        .Replace("{valueMax}", valueMax.ToString());

    public string GetRangeErrorMsg(string propertyName, double valueMin, double valueMax) =>
      RangeValidationMsg.RangeErrorMessage
        .Replace("{PropertyName}", propertyName)
        .Replace("{valueMin}", valueMin.ToString())
        .Replace("{valueMax}", valueMax.ToString());

    public string GetGreaterThanOrEqualErrorMsg(string propertyName, int value) =>
      RangeValidationMsg.GreaterThanOrEqualError
        .Replace("{PropertyName}", propertyName)
        .Replace("{value}", value.ToString());

    public string GetLessThanOrEqualErrorMsg(string propertyName, int value) =>
      RangeValidationMsg.LessThanOrEqualError
        .Replace("{PropertyName}", propertyName)
        .Replace("{value}", value.ToString());

    public string GetRangeLengthMsg(string propertyName, int minLength, int maxLength) =>
      RangeValidationMsg.RangeLength
        .Replace("{PropertyName}", propertyName)
        .Replace("{minLength}", minLength.ToString())
        .Replace("{maxLength}", maxLength.ToString());
  }
}
