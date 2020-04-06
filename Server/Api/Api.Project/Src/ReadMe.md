# FocusMark Project API

### Overview

The Project API provides REST endpoints for executing commands and performing queries against the Project resource. The below endpoints are available.

| Verb   | EndPoint      | Response                                                                          |
|--------|---------------|-----------------------------------------------------------------------------------|
| GET    | /project      | 200 OK - Collection of projects for user                                          |
| GET    | /project/{id} | 200 OK - Single project resource with matching Id                                 |
| POST   | /project      | 202 Accepted - Empty body with Location header of the eventually persisted record |
| PUT    | /project/{id} | 202 Accepted - Empty body with Location header of the eventually updated record   |
| DELETE | /project/{id} | 202 Accepted - Empty body. Record eventually deleted                              |

### Constructs

The API is designed with an implementation of CQRS. The `POST`, `PUT` and `DELETE` verbs are used in combination with a `command` header. All of the commands are published for downstream consumers, with consumers being responsible for persisting the creation of records, updates to records or deletion or records.

Commands can not be used interchangeably with Verbs. Each Verb has a dedicated set of commands that can be used with them; commands can not be used in combination with a Verb that it is not intended to be used with.

| VERB   | EndPoint      | Command              |
|--------|---------------|----------------------|
| POST   | /project      | create-project       |
| PUT    | /project/{id} | archive-project      |
| PUT    | /project/{id} | set-project-priority |
| PUT    | /project/{id} | set-project-dates    |
| PUT    | /project/{id} | set-project-progress |
| PUT    | /project/{id} | set-project-title    |
| DELETE | /project/{id} | delete-project       |

When a command is executed it is not guaranteed that the command for that record will take immediate effect. Processing the changes happen asynchronously and any client application interacting with the API must be aware of this and handle the asynchronous nature of the API.

### Debugging

Debugging the application can be done with the Dotnet CLI and the Dotnet HTTP REPL. The REPL can be installed with the `dotnet tool install -g Microsoft.dotnet-httprepl` command.

Once the REPL is installed you can run the API project using `dotnet run` and then interact with it via the REPL. 

You will need to ensure that you have an SNS Topic created in AWS and the Topic ARN stored in configuration path `AWS:SNS:Topics:ApiProjectArn`.