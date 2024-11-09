namespace Project.Configurations
{
  public static class CorsConfig
  {
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigins", builder =>
        {
          builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
      });
      return services;
    }
  }
}
