using System.Globalization;
using System.Web.Mvc;

namespace PAP.Mvc.ModelBinders
{
    /// <summary>
    /// HTML number inputs always send a dot as decimal separator ("12.50").
    /// This binder reads decimals that way, whatever the culture of the server is.
    /// </summary>
    public class InvariantDecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var isNullable = bindingContext.ModelType == typeof(decimal?);

            if (valueResult == null)
            {
                return isNullable ? (object)null : 0m;
            }

            if (string.IsNullOrWhiteSpace(valueResult.AttemptedValue))
            {
                if (isNullable)
                {
                    return null;
                }

                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "El campo " + bindingContext.ModelName + " es obligatorio.");
                return 0m;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

            decimal value;
            var raw = valueResult.AttemptedValue.Trim();
            if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "El valor de " + bindingContext.ModelName + " no es un número válido.");
            return isNullable ? (object)null : 0m;
        }
    }
}
