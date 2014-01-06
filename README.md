Elmah.Contrib.WebApi
====================

**Elmah.Contrib.WebApi** provides an ASP.NET Web API [exception filter](http://www.asp.net/web-api/overview/web-api-routing-and-actions/exception-handling), `ElmahHandleErrorApiAttribute`, for logging errors directly to [ELMAH](http://code.google.com/p/elmah/).

### >>> [Get Elmah.Contrib.WebApi via NuGet](https://nuget.org/packages/Elmah.Contrib.WebApi)

The latest package is also available as a zip from the [releases page](https://github.com/rdingwall/elmah-contrib-webapi/releases).

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

See the [Releases page](https://github.com/rdingwall/elmah-contrib-webapi/releases).
