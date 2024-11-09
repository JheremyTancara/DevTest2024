namespace Project.Core.Validations
{
  public static class ResponsePayload
  {
    public static object FormatError(string status, string pointer, string title, string detail)
    {
      return new
      {
        error = new[]
        {
          new
          {
            status,
            pointer,
            title,
            detail
          }
        }
      };
    }
  }
}
