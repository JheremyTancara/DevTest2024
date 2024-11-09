namespace Project.Core.Utils.ErrorMsg
{
  public class RangeValidationMsg
  {
    public const string RangeErrorMessage = "The {PropertyName} must be between {valueMin} and {valueMax}.";
    public const string GreaterThanOrEqualError = "{PropertyName} must be greater than or equal to {value}.";
    public const string LessThanOrEqualError = "{PropertyName} must be less than or equal to {value}.";
    public const string RangeLength = "{PropertyName} must be between {minLength} and {maxLength} characters long.";
  }
}
