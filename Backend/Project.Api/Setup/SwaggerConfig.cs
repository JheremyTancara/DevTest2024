using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Project.Configurations
{
  public static class SwaggerConfig
  {
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Title = "Api Project - V1",
          Version = "v1",
          Description = "This API provides endpoints for managing products, including actions like creating, updating, deleting, and retrieving product details.",
          TermsOfService = new Uri("https://example.com/terms"),
          Contact = new OpenApiContact
          {
            Name = "Example Contact",
            Url = new Uri("http://localhost:5173/contact")
          },
          License = new OpenApiLicense
          {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
          }
        });
        c.SwaggerDoc("v2", new OpenApiInfo
        {
          Title = "Api Project - V2",
          Version = "v2",
          Description = "This version offers advanced product management features with enhanced data security and flexibility.",
          TermsOfService = new Uri("https://example.com/terms"),
          Contact = new OpenApiContact
          {
            Name = "Example Contact",
            Url = new Uri("http://localhost:5173/contact")
          },
          License = new OpenApiLicense
          {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
          }
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Description = "JWT Authorization header using the Bearer scheme",
          Type = SecuritySchemeType.Http,
          Scheme = "bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            },
            Array.Empty<string>()
          }
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        c.DocInclusionPredicate((version, apiDesc) =>
        {
          return apiDesc.GroupName != null && apiDesc.GroupName == version;
        });
      });

      return services;
    }      
  }
}
