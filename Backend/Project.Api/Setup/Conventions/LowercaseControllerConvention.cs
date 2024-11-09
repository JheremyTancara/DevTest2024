using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Project.Configurations.Conventions
{
  public class LowercaseControllerConvention : IControllerModelConvention
  {
    public void Apply(ControllerModel controller)
    {
      controller.ControllerName = controller.ControllerName.ToLower();
      foreach (var selectorModel in controller.Selectors)
      {
        var attributeRouteModel = selectorModel.AttributeRouteModel;
        if (attributeRouteModel?.Template != null)
        {
          attributeRouteModel.Template = attributeRouteModel.Template.ToLower();
        }
      }
    }
  }
}
