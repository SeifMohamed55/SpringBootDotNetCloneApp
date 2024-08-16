using Microsoft.AspNetCore.Mvc;

namespace EFCorePostgres.Attributes
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true)]
    public class SkipJwtTokenMiddleware : ControllerAttribute
    {
    }
}
