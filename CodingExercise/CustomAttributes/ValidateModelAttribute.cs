using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace CodingExercise.CustomAttributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {
                string message = ExtractMessageFromModelStateErrors(context.ModelState);            

                context.Result = new BadRequestObjectResult(message);
            }
        }
        
        private string ExtractMessageFromModelStateErrors(ModelStateDictionary modelState)
        {
            var result = "";

            var errors = modelState.Select(x => x.Value.Errors).ToList();

            foreach (var error in errors)
            {
                var errorMessages = error.Select(x => x.ErrorMessage).ToList();
                foreach (var message in errorMessages)
                {
                    result += message + " ";
                }
            }

            return result;
        }
    }
}
