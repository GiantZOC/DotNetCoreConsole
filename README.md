# DotNetCoreConsole

This is a sample .NET Core 2.0 console application that reads in settings from a json file, creates services for logging and ASP.NET identity, and runs a sample async method.  The WebApplication included was created using the DotNetCore WebApplication template with individual user accounts.  The console application should be able to interact with user accounts through the UserManager.

Nuget Packages Added To Console Application:
Microsoft.AspNetCore.Identity: Needed for Identity
Microsoft.AspNetCore.Identity.EntityFrameworkCore: Needed to read from the Identity Stores
Microsoft.EntityFrameworkCore: Needed to read from the identity stores
Microsoft.Extensions.Configuration: Base configuration
Microsoft.Extensions.Configuration.Json: Needed to read json file
Microsoft.Extensions.DependencyInjection: Need to perform dependency injection
Microsoft.Extensions.Logging.EventSource: Needed for logging
