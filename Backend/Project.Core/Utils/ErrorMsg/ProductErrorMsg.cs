using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Products.Models;
using Project.Core.Validations;
using System.Reflection;

namespace Project.Core.Utils
{
  public static class ProductErrorMessages
  {
    public static NotFoundObjectResult ProductNotFound(Guid id)
    {
      return new NotFoundObjectResult(
        ResponsePayload.FormatError(
          status: "404",
          pointer: "/data/attributes/productId",
          title: "Resource Not Found",
          detail: $"The product with ID = {id} doesn't exist."
        )
      );
    }
    
    public static ConflictObjectResult UniqueNameConflict(string name)
    {
      return new ConflictObjectResult(
        ResponsePayload.FormatError(
          status: "409",
          pointer: "/data/attributes/name",
          title: "Resource Conflict",
          detail: $"The product name '{name}' already exists. Please provide a unique name."
        )
      );
    }

    public static ConflictObjectResult UniqueVoterEmailConflict(string voterEmail)
    {
      return new ConflictObjectResult(
        ResponsePayload.FormatError(
          status: "409",
          pointer: "/data/attributes/voter-email",
          title: "Resource Conflict",
          detail: $"The email name '{voterEmail}' already exists. Please provide other email."
        )
      );
    }

    public static ConflictObjectResult ObjectAlreadyActivated(Guid id)
    {
      return new ConflictObjectResult(
        ResponsePayload.FormatError(
          status: "409",
          pointer: "/data/attributes/id/restore",
          title: "Resource Conflict",
          detail: $"The product with id '{id}' is already activated. Please provide a different name or update the existing product."
        )
      );
    }

    public static BadRequestObjectResult? InvalidSortParameter(string sort)
    {
      var cleanSort = sort.TrimStart('+', '-');
      var validProperties = typeof(Product).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                           .Where(p => IsValidSortType(p.PropertyType))
                                           .Select(p => p.Name)
                                           .ToList();

      if (!validProperties.Contains(cleanSort))
      {
        var matchingProperty = validProperties.FirstOrDefault(p => string.Equals(p, cleanSort, StringComparison.OrdinalIgnoreCase));

        if (matchingProperty != null)
        {
          return new BadRequestObjectResult(
            ResponsePayload.FormatError(
              status: "400",
              pointer: "/data/attributes/sort",
              title: "Case Mismatch in Sort Parameter",
              detail: $"The sort value '{sort}' seems to have a case mismatch. The correct property name is '{matchingProperty}'."
            )
          );
        }

        return new BadRequestObjectResult(
          ResponsePayload.FormatError(
            status: "400",
            pointer: "/data/attributes/sort",
            title: "Invalid Sort Parameter",
            detail: $"The sort value '{sort}' is not valid. Allowed values are: {string.Join(", ", validProperties)}."
          )
        );
      }

      return null;
    }

    private static bool IsValidSortType(Type propertyType)
    {
      var validTypes = new[] { typeof(string), typeof(int), typeof(decimal), typeof(double), typeof(DateTime), typeof(DateTimeOffset) };
      return validTypes.Contains(propertyType);
    }
  }
}
