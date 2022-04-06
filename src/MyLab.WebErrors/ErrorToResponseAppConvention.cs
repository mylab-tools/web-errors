using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MyLab.WebErrors
{
    class ErrorToResponseAppConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var controllerAction in controller.Actions)
                {
                    if (!ErrorToResponseMap.IsActionPropertiesContainsMap(controllerAction.Properties))
                    {
                        var map = ErrorToResponseMap.LoadFromMethod(controllerAction.ActionMethod);
                        map.AddToActionProperties(controllerAction.Properties);
                    }
                }
            }
        }
    }
}