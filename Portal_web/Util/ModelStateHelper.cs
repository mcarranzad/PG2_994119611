using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace sgc.ml.Util
{
    public static class ModelStateHelper
    {
        public static string Errors(this ModelStateDictionary modelState)
        {
            var errors = new List<string>();
            foreach (var state in modelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    if (!SgcFunctions.Isnull(error.ErrorMessage) && !string.IsNullOrWhiteSpace(error.ErrorMessage))
                    {
                        errors.Add(error.ErrorMessage);
                    }
                    else
                    {
                        if (!SgcFunctions.Isnull(error.Exception.Message) && !string.IsNullOrWhiteSpace(error.Exception.Message))
                            errors.Add(error.Exception.Message);
                    }
                }
            }

            if (errors.Count() > 0)
                return errors[0];

            return "Error no definido";
        }
    }
}
