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
<br>eg: Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJHYXNwYXJpIiwianRpIjoiODhlZmYyZDQtNGFhZC00MjE4LTk5MDItMGUxNGM0YzgzNmRmIiwiR2FzcGFyaUlkIjoiMTAwMCIsIm5iZiI6MTUxMTg2NzUyMywiZXhwIjoxNTExODY5MzIzLCJpc3MiOiJHYXNwYXJpIiwiYXVkIjoiR2FzcGFyaSJ9.WzP8dJK2lUuWKgUCUpWdqzxxVpiG-PRbLAooAJKZh14


Instead of using hard coded credential in appsettings.json
Entity Framework Core could be used.

NLog saves to txt and csv files, you can also add Database support.
<br>Take a look at the nlog.config file.
<br>In Program.cs I added a filter for core events:
    .ConfigureLogging(logging =>
	    logging
		.AddFilter("System", LogLevel.Warning)
		.AddFilter("Microsoft", LogLevel.Warning))

						
In order to have ALWAYS the Webservice responsive on IIS:
Create a new Pool (eg: core 2)
<br>Set the Idle Time to 1440 (minutes)
<br>Set Ricycle at 1 AM or whatever you want


