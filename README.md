# MyLab.WebErrors

For .NET Core 2.1+

[![NuGet](https://img.shields.io/nuget/v/MyLab.WebErrors.svg)](https://www.nuget.org/packages/MyLab.WebErrors/)

Provides abilities to manage unhandled exception in .NET Core web projects.

Primary features:

* exception to response mapping
* unhandled exception logging
* unhandled exception hiding
* data contract for interlevel errors



## Beginning

To enable abilities you should integrate their into .NET Core request processing pipeline when configure the services:  

```c#
public class Startup
{
    // ...
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc(o => o.AddExceptionProcessing());
    }

    // ...
}
```

These work required for all features instead `exception to response mapping`.



## Exception to Response

When you design a web service, you typically have several separated layers such as `data access layer`, `business logic` and `presentation layer`.  Business logic should decide when an exceptional situation occurs. And it should throw proper `exception` at that moment. Presentation layer should interpret these exceptions as web-service response in accordance with specification.

`MyLab.WebErrors` offers you a declaration-way to describe controller behavior based on exceptions from lower layers.

Use `ErrorToReponsAttribute `to map exception to response. The following example shows how use these attribute with controller method. In these case we map `NullreferenceException` to `404 (NotFound)` response with message from exception:

 ```C#
[ErrorToResponse(typeof(NullReferenceException), HttpStatusCode.NotFound)]
public ActionResult GetWithoutMessage()
{
	throw new NullReferenceException("bar");
}
 ```

Response:

```
404 (NotFound)

bar
```



Also you can define override message which will be passed in response body:

```C#
[ErrorToResponse(typeof(NullReferenceException), HttpStatusCode.NotFound, "foo")]
public ActionResult GetWithMessage()
{
	throw new NullReferenceException("bar");
}
```

Response:

```
404 (NotFound)

foo
```



Attribute `ErrorToResponseAttribute`may be used multiple. Also you can use it with controller class to define map for all methods.



## Hiding

Unhandled exception hiding is a behavior by default. It means that all unhandled exception captured out of controller method will bу converted to special message. For this case response http code is 500. 



The following example shows how looks like unexpected exception hiding:

```C#
[HttpGet]
public ActionResult Get()
{
	throw new NullReferenceException("bar");
}
```

Response:

```json
500 (InternalServerError)

{
  "id": "d797b160-929d-4d77-9a79-3f80617241dc",
  "msg": "An error occurred during the operation"
}
```



Also you can set custom message:

```C#
services.Configure<ExceptionProcessingOptions>(o => o.HidesMessage = "foo");
```

Response:

```json
500 (InternalServerError)

{
  "id": "da80cdd4-2a0f-42fc-9174-076343211a0d",
  "msg": "foo"
}
```



## Passing

An unhandled exception passing through levels my be useful in cases such as debugging or test stage. In this mode an unhandled exception will be converted in the same data contract as in hiding mode. But exception message and stack-trace will be passed:

```C#
[HttpGet]
public ActionResult Get()
{
	throw new NullReferenceException("bar");
}
```

Response:

```json
{
  "id": "b6ba59d2-86e9-4872-8de8-c4b4c143ce7d",
  "msg": "bar",
  "tech": "   at TestServer.Controllers.ExceptionHidingController.Get() in D:\\Projects\\my\\mylab-web-errors\\src\\TestServer\\Controllers\\ExceptionHidingController.cs:line 17\r\n   at lambda_method(Closure , Object , Object[] )\r\n   at Microsoft.AspNetCore.Mvc.Internal.ActionMethodExecutor.SyncActionResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\r\n   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeActionMethodAsync()\r\n   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeNextActionFilterAsync()\r\n   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.Rethrow(ActionExecutedContext context)\r\n   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeInnerFilterAsync()\r\n   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeNextExceptionFilterAsync()"
}
```

To enable this mode you should disable unexpected exception hiding:

```C#
services.Configure<ExceptionProcessingOptions>(o => o.HideError = false);
```



## Log

All outgoing messages which was processed are write to log. The same identifier uses in both case: response and log message. It useful for debugging and researching.



## PS

All examples above you can find in test projects.

