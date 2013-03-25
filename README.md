Elmah.Contrib.WebApi
====================

**Elmah.Contrib.WebApi** provides an ASP.NET Web API [exception filter](http://www.asp.net/web-api/overview/web-api-routing-and-actions/exception-handling), `ElmahHandleErrorApiAttribute`, for logging errors directly to [ELMAH](http://code.google.com/p/elmah/).

### >>> [Get Elmah.Contrib.WebApi via NuGet](https://nuget.org/packages/Elmah.Contrib.WebApi)

The latest package is also available as a zip from the [downloads page](https://github.com/rdingwall/elmah-contrib-webapi/downloads).

# Usage

Simply register it during your application's start up, or on a controller-by-controller basis.

```csharp
protected void Application_Start()
{
    GlobalConfiguration.Configuration.Filters.Add(new ElmahHandleErrorApiAttribute());

    ...
}
```

Note this filter requires ASP.NET (it passes the HttpContext directly to ELMAH to record info about the HTTP request). It will not work in a self-hosted WCF application.

# Acknowledgements

This library is a port of Fabian Vilers' [Elmah.Contrib.Mvc](http://nuget.org/packages/Elmah.Contrib.Mvc) to ASP.NET Web API. The original implementation (for MVC) can be found [here](http://stackoverflow.com/questions/766610/how-to-get-elmah-to-work-with-asp-net-mvc-handleerror-attribute/779961#779961).

# License

As a derivative work of Elmah.Contrib.Mvc, this library is available under the same [MS-PL license](http://www.opensource.org/licenses/ms-pl).

# Release History / Changelog

### 1.0.5.0 - March 25, 2013
Patch from shchahrykovich to not throw a NullReferenceException in the event that HttpContext.Current.ApplicationInstance is null.

### 1.0.4.0 - October 2, 2012
Added missing .nuspec package dependencies.

### 1.0.2.0 - September 30, 2012
Fix for issue #2, updated to use new Microsoft.AspNet.WebApi.Core NuGet package.

### 1.0.1.0 - July 8, 2012
Minor fix issue #1, removed Elmah and System.Web.Http assemblies from NuGet package.

### 1.0.0.0 - May 20, 2012
Initial release.
