# Overview

These are several frameworks you can choose to build new Web application (single-page application).
The old architecture of ZohoTimesheet use native ASP.NET Core (.Net core 3) with custom Vue project template
to build client-side user interface (UI).

The customization does look more complex with advanced setup environment and build bundle also by use Webpack (thanks webpack).

Now it's 2022, the web technology have a lot framework and architecture to build new web app but outstanding is
upgrade of Vue 2 to Vue 3 and a newest bundle generate tooling on frontend that is Vite.

But the point is we want to stick with ASP.NET Core for building ZohoTimesheet app. We don't have so much experiences setting and config bundle on Webpack.
So the new idea and architecture to build new and refactor all ZohoTimeSheet app is using Vite for bundle and setting client-side to generate the files and connect to api also.

### Old ZohoTimeSheet

#### Tech stacks
- ASP.NET Core 3
- Vue 2
- Webpack
- Vuetify

#### Problems
- Always get accessToken before execute call Api.
- Loading pages slow

### New ZohoTimeSheet

#### Tech stacks
- ASP.NET Core 6
- Using Vue 3
- Fast bundle tooling by Vite
- Vuetify 3 beta

#### Features
- Authenticate via OAuth authorization giant code flow. Please check [OAuth](./OAUTH.md)
- Control accessToken via Cookie browser. Client-side javascript can not attack by code `document.getCookie`
- Store refreshToken by user on Azure KeyVault Secret
- Setup with run on each environment (Development and Production)
- Can use application as image container on Azure registry.
- Can deploy separate client-side and api.


## Setup

### Api

#### Spa client

1. Microsoft.AspNetCore.SpaServices.Extensions

This package allow to register middleware Spa client
```c#
dotnet add package Microsoft.AspNetCore.SpaServices.Extensions
```

Example:

this code register spa client run via localhost with port 3000

```csharp
spa.UseProxyToSpaDevelopmentServer("http://localhost:3000/")
```

2. Static files in ASP.NET Core

Add this code `app.UseSpaStaticFiles();`

Reference [static files](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-6.0#serve-files-outside-wwwroot-by-updating-iwebhostenvironmentwebrootpath)

3. Config custom out of `wwwRoot`

Add this code

```csharp
builder.Services.AddSpaStaticFiles(config => { config.RootPath = "dist"; });
```

`dist` folder is here get from client-side generation build (npm run build)

#### Azure keyVault

### Client

