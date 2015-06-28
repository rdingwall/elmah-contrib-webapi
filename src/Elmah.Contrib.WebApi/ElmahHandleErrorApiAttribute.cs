using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

#pragma warning disable 1591

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
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ElmahHandleErrorApiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var e = actionExecutedContext.Exception;
            if (e != null)
            {
                RaiseOrLog(e, actionExecutedContext);
            }
            else if ((int)actionExecutedContext.Response.StatusCode >= 500)
            {
                RaiseOrLog(
                    new HttpException(
                        (int)actionExecutedContext.Response.StatusCode,
                        ResolveMessage(actionExecutedContext)),
                    actionExecutedContext);
            }
        }

        private string ResolveMessage(HttpActionExecutedContext actionExecutedContext)
        {
            const string messageKey = "Message";

            var defaultMessage = actionExecutedContext.Response.ReasonPhrase;
            var objectContent = actionExecutedContext.Response.Content as ObjectContent<HttpError>;
            if (objectContent == null) return defaultMessage;

            var value = objectContent.Value as HttpError;
            if (value == null) return defaultMessage;

            if (!value.ContainsKey(messageKey)) return defaultMessage;

            var message = value[messageKey] as string;
            return string.IsNullOrWhiteSpace(message) ? defaultMessage : message;
        }

        private void RaiseOrLog(Exception exception, HttpActionExecutedContext actionExecutedContext)
        {
            if (RaiseErrorSignal(exception) // prefer signaling, if possible
                || IsFiltered(actionExecutedContext)) // filtered?
                return;

            LogException(exception);
        }

        private static bool RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            if (context == null)
                return false;
            var application = HttpContext.Current.ApplicationInstance;
            if (application == null)
                return false;
            var signal = ErrorSignal.Get(application);
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

#pragma warning restore 1591
