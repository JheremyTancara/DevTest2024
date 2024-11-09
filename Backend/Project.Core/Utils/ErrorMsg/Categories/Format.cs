namespace Project.Core.Utils.ErrorMsg
{
  public class FormatValidationMsg
  {
    public const string InvalidEmailFormat = "{PropertyName} must be a valid Email address ending with '@gmail.com'.";
    public const string InvalidPasswordFormat = "{PropertyName} must contain at least one special character, at least one number, and at least two uppercase letters.";
    public const string InvalidTimeFormat = "{PropertyName} must be in the format HH:mm:ss.";
    public const string InvalidImageUrl = "{PropertyName} must be a valid image URL. It should include a proper image extension (e.g., .png, .jpg, .gif, .bmp) or just a valid URL without an extension.";
    public const string InvalidYouTubeUrl = "{PropertyName} must be a valid YouTube URL. It should start with 'https://', include 'youtube.com' or 'youtu.be', and adhere to the appropriate format.";
  }
}
