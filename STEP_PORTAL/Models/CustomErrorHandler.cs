/*using System.Web.Mvc;

public class CustomErrorHandler : IExceptionFilter
{
    public void OnException(ExceptionContext filterContext)
    {
        // Log the exception if needed
        // Log.Error("An error occurred", filterContext.Exception);

        // Set the result to a view displaying an error message
        filterContext.Result = new ViewResult
        {
            ViewName = "Error",
            ViewData = new ViewDataDictionary(filterContext.Exception)
        };

        // Mark the exception as handled
        filterContext.ExceptionHandled = true;
    }
}
*/