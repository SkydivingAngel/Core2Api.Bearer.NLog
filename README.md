# ASP.NET Core 2.0 Api with Bearer Authentication and NLog
ASP.NET Core 2.0 Api with Bearer Authentication and NLog

Configuration Values are in appsettings.json
eg: token lifetime
eg: username and password


1)Obtain the Token

Post to http://xyz/CreateBearerToken (works also on https)
the following class:

{
	"username": "user",
	"password": "pwd"
}

and you'll get the Token.

Set Content Type: Content-Type: application/json

2)Use the Token setting the Authorization Header with the Bearer Token
eg: Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJHYXNwYXJpIiwianRpIjoiODhlZmYyZDQtNGFhZC00MjE4LTk5MDItMGUxNGM0YzgzNmRmIiwiR2FzcGFyaUlkIjoiMTAwMCIsIm5iZiI6MTUxMTg2NzUyMywiZXhwIjoxNTExODY5MzIzLCJpc3MiOiJHYXNwYXJpIiwiYXVkIjoiR2FzcGFyaSJ9.WzP8dJK2lUuWKgUCUpWdqzxxVpiG-PRbLAooAJKZh14


Instead of using hard coded credential in appsettings.json
Entity Framework Core could be used.

NLog saves to txt and csv files, you can also add Database support.
take a look at the nlog.config file.
In Program.cs I added a filter for core events:
.ConfigureLogging(logging =>
                    logging
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("Microsoft", LogLevel.Warning))
