using FluentValidation;
using FluentValidation.AspNetCore;
using Project.Core.Entities.Products.Validation.DTOs.Products;

namespace Project.Configurations
{
  public static class ValidationConfig
  {
    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
      services.AddFluentValidationAutoValidation();
      services.AddFluentValidationClientsideAdapters();
      services.AddValidatorsFromAssemblyContaining<RegisterProductDTOValidator>();
      return services;
    }
  }
}
