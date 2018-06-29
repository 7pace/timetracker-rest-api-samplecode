## Description
This example application will make a series of requests to available API endpoints. Current endpoints:
* /me
* /users
* /activityTypes
* /workLogs

All, except **workLogs**, are currently read only.

For **workLogs** following list of VERB are available:
* **POST** to create new worklog
* **PATCH** to partially update existing worklog
* **DELETE** to delete worklog from system

## Running
Application can connect either to VSTS or to TFS.

To run application to connect to VSTS use console with arguments:

```console 
dotnet.exe 7pace.Timetracker.RestApiExample.dll https://[yourVstsAccountName].timehub.7pace.com/api -t [yourApiToken]
```
To run application to connect to TFS use console with arguments (it will use NTLM authorization and your current logged-in windows user):

```console 
dotnet.exe 7pace.Timetracker.RestApiExample.dll [yourTimetrackerServiceUrl]/api/[CollectionName] -w
```

## Execution result
Application will output to console VERB, path and result of each request. It will create, update and delete worklog, so it will leave no trash data.
Each execution is appended in a file "appLog.json" so you will have a history of all runs.