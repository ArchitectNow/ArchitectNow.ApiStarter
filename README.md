# ArchitectNow.ApiStarter

This project is setup as a baseline ASP.NET Core 2.2 API demonstration used by ArchitectNow in a corresponding workshop or class.  This project is very opinionated and utilizes a number of 3rd party open source libraries to piece together a robust foundation on which to build APIs.

If there are any questions regarding this setup please don't hesitate to reach out at kvgros@architectnow.net.   If you are interested in any of ArchitectNow's workshops check out our website at www.architectnow.net/workshops.  

**Development Urls**

* Swagger UI: http://localhost:5000/docs
* Swagger JSON: http://localhost:5000/docs/1.0/swagger.json 
* Healthcheck UI:  http://localhost:5000/healthchecks-ui 

**Key Technologies Utilized**

* C#
* ASP.NET Core 2.2 - https://www.microsoft.com/net/download/core
* Autofac - https://autofac.org/
* Automapper - http://automapper.org/
* FluentValidation - https://fluentvalidation.net/
* Serilog - https://serilog.net/
* NSwag for ASP.NET Core  - https://github.com/RSuter/NSwag (for Swagger integration and NSwag support)
* xUnit for the unit test harness - https://xunit.github.io/ 

**Additional Tools, Libraries, and Concepts**

*Swagger/Open API*

The sample project exposes a Swagger compliant UI (3.0) and thus supports any external Swagger compliant tool or viewer.  More information on the Swagger and Open API specifications and tool ecosystem can be found here: https://swagger.io/

*Postman*

Many developers find it useful to use the cross-platform Postman tool (https://www.getpostman.com/) to interact with their API during development.

*API Versioning*

This version of the sample API utilizes Microsoft's new ASP.NET Core API Versioning libraries.  More information on these features can be found here: https://github.com/Microsoft/aspnet-api-versioning

*Health Checks*

This version of the sample API utilizes Microsoft's new ASP.NET Core Health Check infrastructure.  More information on these features can be found here: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2

*Database Layer*

Repository layer is set up for MongoDb (https://www.mongodb.com/) and utilizes the MongoDB.Driver library for .NET (https://docs.mongodb.com/ecosystem/drivers/csharp/)

*.NET Standard*

All libraries are NetCore or NetStandard 2.0 compliant and all original development was done on MacBook Pro's using Rider by Jetbrains (https://www.jetbrains.com/rider/)




