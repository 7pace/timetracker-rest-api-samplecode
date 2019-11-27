## Description
This example application will make a series of requests to available API endpoints. REST API has "[TimetrackerServiceUrl]/api/rest" prefix before endpoints. Current endpoints (GET) are:
* /me
* /users
* /activityTypes
* /workLogs

All, except **workLogs**, are currently read only.

For **workLogs** following list of VERBs are available:
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
Here you'll find cutdown result of single application run:
```json
Execution started at 29.06.2018 15:36:11
GET http://tfs2018:8090/api/DefaultCollection/rest/me?api-version=3.0
{
  "data": {
    "user": {
      "uniqueName": "7PACE\\eugene",
      "vstsId": "b17afa0a-6803-47ab-a11b-61051f33fd00",
      "vstsCollectionId": "bb251adf-c518-445a-b1a3-bfd5ba1097d9",
      "name": "Eugene Kolomytsev",
      "id": "0d63c0cf-4e66-4a63-9fce-ea5971b783ce"
    },
    "account": {
      "vstsId": "7b818832-876f-4f2a-bcf1-bd1186ff06b1",
      "vstsCollectionId": "4c9c0ceb-c7a1-42b7-b59e-eab6ccad0db4",
      "name": "DefaultCollection",
      "id": "0f0ef946-7845-4ec9-bc75-63bca47ba4c9"
    },
    "defaultActivityType": null,
    "timeZone": 0
  }
}

Press any key to continue

GET http://tfs2018:8090/api/DefaultCollection/rest/users?api-version=3.0
{
  "data": [
    {
      "uniqueName": "Domain\\JohnDoe",
      "vstsId": "68132b77-2f9c-4542-886a-d8b671b1d193",
      "vstsCollectionId": "69aa82b6-ab63-46b8-9910-f9e1d1b3dbe4",
      "name": "John Doe",
      "id": "5d754739-f4fc-e711-8418-00155d0a6b50"
    },
    {
      "uniqueName": "7PACE\\eugene",
      "vstsId": "b17afa0a-6803-47ab-a11b-61051f33fd00",
      "vstsCollectionId": "bb251adf-c518-445a-b1a3-bfd5ba1097d9",
      "name": "Eugene Kolomytsev",
      "id": "0d63c0cf-4e66-4a63-9fce-ea5971b783ce"
    },   
  ]
}

Press any key to continue

GET http://tfs2018:8090/api/DefaultCollection/rest/activityTypes?api-version=3.0
{
  "data": {
    "enabled": true,
    "activityTypes": [
      {
        "color": "ffffff",
        "name": "[Not Set]",
        "id": "00000000-0000-0000-0000-000000000000"
      },
      {
        "color": "ec654c",
        "name": "Deployment",
        "id": "cee030e2-b8b4-e711-8418-00155d0a6b50"
      },
      {
        "color": "54c270",
        "name": "Design",
        "id": "cfe030e2-b8b4-e711-8418-00155d0a6b50"
      },
      {
        "color": "46aee8",
        "name": "Development",
        "id": "d0e030e2-b8b4-e711-8418-00155d0a6b50"
      },
      {
        "color": "a2a4a7",
        "name": "Documentation",
        "id": "d1e030e2-b8b4-e711-8418-00155d0a6b50"
      },
      {
        "color": "4765a9",
        "name": "Planning",
        "id": "d2e030e2-b8b4-e711-8418-00155d0a6b50"
      },
      {
        "color": "8c4093",
        "name": "Requirements",
        "id": "d3e030e2-b8b4-e711-8418-00155d0a6b50"
      },
      {
        "color": "f8b63a",
        "name": "Testing",
        "id": "d4e030e2-b8b4-e711-8418-00155d0a6b50"
      }
    ],
    "systemDefaultActivityTypeId": "00000000-0000-0000-0000-000000000000"
  }
}

Press any key to continue

GET http://tfs2018:8090/api/DefaultCollection/rest/workLogs?api-version=3.0&$fromTimestamp=2018-05-01&$count=10
{
  "data": [
	  {
            "timestamp": "2019-08-31T14:45:00",
            "length": 3600,
            "billableLength": null,
            "workItemId": 8948,
            "comment": "",
            "user": {
                "uniqueName": "7PACE\\eugene",
                "vstsId": "b17afa0a-6803-47ab-a11b-61051f33fd00",
                "vstsCollectionId": "bb251adf-c518-445a-b1a3-bfd5ba1097d9",
                "name": "Eugene Kolomytsev",
                "id": "0d63c0cf-4e66-4a63-9fce-ea5971b783ce"
            },
            "addedByUser":{
                "uniqueName": "7PACE\\eugene",
                "vstsId": "b17afa0a-6803-47ab-a11b-61051f33fd00",
                "vstsCollectionId": "bb251adf-c518-445a-b1a3-bfd5ba1097d9",
                "name": "Eugene Kolomytsev",
                "id": "0d63c0cf-4e66-4a63-9fce-ea5971b783ce"
            },
            "createdTimestamp": "2018-03-28T14:45:38.107",
            "editedTimestamp": "2018-03-28T14:45:38.107",
            "activityType": {
                "color": "ffffff",
                "name": "[Not Set]",
                "id": "00000000-0000-0000-0000-000000000000"
            },
            "flags": {
            "isTracked": false,
            "isManuallyEntered": true,
            "isChanged": false,
            "isTrackedExtended": false,
            "isImported": false,
            "isFromApi": false,
            "isBillable": false
            },
            "id": "dc54d553-a762-47c3-a68f-80d64bdd2a66"
        },
        {
            "timestamp": "2019-08-31T14:35:00",
            "length": 3600,
            "billableLength": null,
            "workItemId": 8946,
            "comment": "",
            "user": {
                "uniqueName": "7PACE\\eugene",
                "vstsId": "b17afa0a-6803-47ab-a11b-61051f33fd00",
                "vstsCollectionId": "bb251adf-c518-445a-b1a3-bfd5ba1097d9",
                "name": "Eugene Kolomytsev",
                "id": "0d63c0cf-4e66-4a63-9fce-ea5971b783ce"
            },
            "addedByUser": {
                "uniqueName": "7PACE\\eugene",
                "vstsId": "b17afa0a-6803-47ab-a11b-61051f33fd00",
                "vstsCollectionId": "bb251adf-c518-445a-b1a3-bfd5ba1097d9",
                "name": "Eugene Kolomytsev",
                "id": "0d63c0cf-4e66-4a63-9fce-ea5971b783ce"
            },
            "createdTimestamp": "2018-03-28T14:36:26.467",
            "editedTimestamp": "2018-03-28T14:36:26.467",
            "activityType": {
                "color": "ffffff",
                "name": "[Not Set]",
                "id": "00000000-0000-0000-0000-000000000000"
            },
            "flags": {
            "isTracked": false,
            "isManuallyEntered": true,
            "isChanged": true,
            "isTrackedExtended": false,
            "isImported": false,
            "isFromApi": false,
            "isBillable": false
            },
            "id": "a3344f33-e8c9-413a-a142-8bfc186ca323"
        }    
	]
}

Press any key to continue

POST http://tfs2018:8090/api/DefaultCollection/rest/workLogs?api-version=3.0
{
  "data": {
    "timestamp": "2018-06-29T05:36:21.3890636",
    "length": 3600,
    "billableLength": null,
    "workItemId": null,
    "comment": "test created",
    "user": {
      "uniqueName": "7PACE\\eugene",
      "vstsId": "b17afa0a-6803-47ab-a11b-61051f33fd00",
      "vstsCollectionId": "bb251adf-c518-445a-b1a3-bfd5ba1097d9",
      "name": "Eugene Kolomytsev",
      "id": "0d63c0cf-4e66-4a63-9fce-ea5971b783ce"
    },
    "addedByUser": {
      "uniqueName": "7PACE\\eugene",
      "vstsId": "b17afa0a-6803-47ab-a11b-61051f33fd00",
      "vstsCollectionId": "bb251adf-c518-445a-b1a3-bfd5ba1097d9",
      "name": "Eugene Kolomytsev",
      "id": "0d63c0cf-4e66-4a63-9fce-ea5971b783ce"
    },
    "createdTimestamp": "2018-06-29T05:36:21.3890636",
    "editedTimestamp": "2018-06-29T05:36:21.3890636",
    "activityType": {
      "color": "ffffff",
      "name": "[Not Set]",
      "id": "00000000-0000-0000-0000-000000000000"
    },
    "flags": {
      "isTracked": false,
      "isManuallyEntered": false,
      "isChanged": false,
      "isTrackedExtended": false,
      "isImported": false,
      "isFromApi": false,
      "isBillable": false
    },
    "id": "a4919706-997b-e811-8425-00155d0a6b50"
  }
}

Press any key to continue

PATCH http://tfs2018:8090/api/DefaultCollection/rest/workLogs/a4919706-997b-e811-8425-00155d0a6b50?api-version=3.0
{
  "data": {
    "timestamp": "2018-06-29T05:36:21.39",
    "length": 7200,
    "billableLength": null,
    "workItemId": null,
    "comment": "test updated",
    "user": {
      "uniqueName": "7PACE\\eugene",
      "vstsId": "b17afa0a-6803-47ab-a11b-61051f33fd00",
      "vstsCollectionId": "bb251adf-c518-445a-b1a3-bfd5ba1097d9",
      "name": "Eugene Kolomytsev",
      "id": "0d63c0cf-4e66-4a63-9fce-ea5971b783ce"
    },
    "addedByUser": {
      "uniqueName": "7PACE\\eugene",
      "vstsId": "b17afa0a-6803-47ab-a11b-61051f33fd00",
      "vstsCollectionId": "bb251adf-c518-445a-b1a3-bfd5ba1097d9",
      "name": "Eugene Kolomytsev",
      "id": "0d63c0cf-4e66-4a63-9fce-ea5971b783ce"
    },
    "createdTimestamp": "2018-06-29T05:36:21.39",
    "editedTimestamp": "2018-06-29T05:36:22.6424637",
    "activityType": {
      "color": "ffffff",
      "name": "[Not Set]",
      "id": "00000000-0000-0000-0000-000000000000"
    },
    "flags": {
      "isTracked": false,
      "isManuallyEntered": false,
      "isChanged": false,
      "isTrackedExtended": false,
      "isImported": false,
      "isFromApi": false,
      "isBillable": false
    },
    "id": "a4919706-997b-e811-8425-00155d0a6b50"
  }
}

Press any key to continue

DELETE http://tfs2018:8090/api/DefaultCollection/rest/workLogs/a4919706-997b-e811-8425-00155d0a6b50?api-version=3.0
{
  "data": null
}

Press any key to continue

Finished. Press any key to close
```
