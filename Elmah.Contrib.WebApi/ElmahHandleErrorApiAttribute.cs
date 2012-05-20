using System;
using System.Web;
using System.Web.Http.Filters;

namespace Elmah.Contrib.WebApi
{
    /// <summary>
    /// ASP.NET Web API exception filter for logging errors to ELMAH. This
    /// assumes an HttpContext is present (i.e., running under ASP.NET - it
    /// will not work as a self-hosted WCF application).
    /// </summary>
    /// <remarks>
    /// Original implementation from http://stackoverflow.com/questions/766610/how-to-get-elmah-to-work-with-asp-net-mvc-handleerror-attribute/779961#779961.
    /// Ported from the Elmah.Contrib.Mvc package on NuGet.
    /// </remarks>
    public class ElmahHandleErrorApiAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);

            var e = actionExecutedContext.Exception;
            if (RaiseErrorSignal(e)      // prefer signaling, if possible
                || IsFiltered(actionExecutedContext))     // filtered?
                return;

            LogException(e);
        }

        private static bool RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            if (context == null)
                return false;
            var signal = ErrorSignal.FromContext(context);
            if (signal == null)
                return false;
            signal.Raise(e, context);
            return true;
        }

        private static bool IsFiltered(HttpActionExecutedContext context)
        {
            var config = HttpContext.Current.GetSection("elmah/errorFilter")
                         as ErrorFilterConfiguration;

            if (config == null)
                return false;

            var testContext = new ErrorFilterModule.AssertionHelperContext(
                                      context.Exception, HttpContext.Current);

            return config.Assertion.Test(testContext);
        }

        private static void LogException(Exception e)
        {
            var context = HttpContext.Current;
            ErrorLog.GetDefault(context).Log(new Error(e, context));
        }
    }
}
