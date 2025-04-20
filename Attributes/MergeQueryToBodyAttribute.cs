using Microsoft.AspNetCore.Mvc.Filters;

namespace KOAHome.Attributes
{
  public class MergeQueryToBodyAttribute : ActionFilterAttribute
  {
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      var query = context.HttpContext.Request.Query
                   .ToDictionary(k => k.Key, v => v.Value.ToString());

      // Nếu action có param Dictionary<string,string>, gán vào
      var param = context.ActionDescriptor.Parameters
          .FirstOrDefault(p => p.ParameterType == typeof(Dictionary<string, string>));

      if (param != null)
        context.ActionArguments[param.Name!] = query;

      await next();
    }
  }
}
