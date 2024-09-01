using Microsoft.AspNetCore.Mvc;

namespace SpringBootCloneApp.Attributes
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true)]
    public class SkipJwtTokenMiddleware : ControllerAttribute
    {
    }
}
