# csharp-dotnetcore-dbup
## Overview
C# Dotnet Core 3.1 application built on [DBUp](https://dbup.github.io/) to deploy SQL based change sets to the major database engines.
## Requirements
### To run
* [.NET Core 3.1 Runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1)
### To build
* [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)
## Building
Navigate to the folder that contains the projects ```.sln``` file

* Build a project and its dependencies:
```
dotnet build
```
* Build a project and its dependencies using Release configuration:
```
dotnet build --configuration Release
```
* Build a project and its dependencies for a specific runtime (in this example, Ubuntu 18.04):
    * [List of runtime ID's](https://github.com/dotnet/runtime/blob/master/src/libraries/pkg/Microsoft.NETCore.Platforms/runtime.json)
```
dotnet build --runtime ubuntu.18.04-x64
```
## Steps
There are 5 steps configured which should cover 99% of scenarios

* Each steps runs in the order listed below.
* Each steps is essentially a folder within the relative path of the entry point
    * Folder names will be affixed as a prefix to the script name
    * If root folders don't exist at launch the app will create empty ones
* The application will recurse through the folders
* The application only looks for the following types of files ```*.sql```
    * Scripts inside folders will be executed in numerical order so prefix them with the numerical order you want them to be executed in

1. RunAlwaysPreScripts
    * Scripts in step will always run. What this means is this step does not journal it's results to the schema table so it will always execute the scripts in this folder regardless
2. PreDeploymentScripts
    * Scripts in step will only ever execute a script once
3. DeploymentScripts
    * Scripts in step will only ever execute a script once
4. PostDeploymentScripts
    * Scripts in stepwill only ever execute a script once
5. RunAlwaysPostScripts
    * Scripts in step will always run. What this means is this step does not journal it's results to the schema table so it will always execute the scripts in this folder regardless

### Example folder structure
```
|- RunAlwaysPreScripts (required)
   |- 01_Change_script_at_root.sql (file, optional)
   |- 02_Change_script_at_root.sql (file, optional)
   |- AZDO-12345 (folder, optional, Unique name for the folder used to track changeset grouping)
       |- README.md (file, optional)
       |- 01_Change_script.sql (file, optional)
       |- 02_Change_script.sql (file, optional)
|- PreDeploymentScripts (required)
   |- 01_Change_script_at_root.sql (file, optional)
   |- 02_Change_script_at_root.sql (file, optional)
   |- AZDO-12345 (folder, optional, Unique name for the folder used to track changeset grouping)
       |- README.md (file, optional)
       |- 01_Change_script.sql (file, optional)
       |- 02_Change_script.sql (file, optional)
|- DeploymentScripts (required)
   |- 01_Change_script_at_root.sql (file, optional)
   |- 02_Change_script_at_root.sql (file, optional)
   |- AZDO-12345 (folder, optional, Unique name for the folder used to track changeset grouping)
       |- README.md (file, optional)
       |- 01_Change_script.sql (file, optional)
       |- 02_Change_script.sql (file, optional)
|- PostDeploymentScripts (required)
   |- 01_Change_script_at_root.sql (file, optional)
   |- 02_Change_script_at_root.sql (file, optional)
   |- AZDO-12345 (folder, optional, Unique name for the folder used to track changeset grouping)
       |- README.md (file, optional)
       |- 01_Change_script.sql (file, optional)
       |- 02_Change_script.sql (file, optional)
|- RunAlwaysPostScripts (required)
   |- 01_Change_script_at_root.sql (file, optional)
   |- 02_Change_script_at_root.sql (file, optional)
   |- AZDO-12345 (folder, optional, Unique name for the folder used to track changeset grouping)
       |- README.md (file, optional)
       |- 01_Change_script.sql (file, optional)
       |- 02_Change_script.sql (file, optional)
```
## Application settings
In the root of the application there is a ```appsettings.json``` file that contains the following JSON object
```json
{
  "DbUpDataSource": "",
  "DbEngineType": "",
  "DbUpTimeout": 10
}
```
* ```appsettings.json``` is a required file
* ```DbUpDataSource``` is the datasource that will be used to initate the connection to your database engine
    * Below is a list of connection strings that has been tested against the supported database engines
* ```DbEngineType``` is the type of database engine
    * Below is a list of supported database engines
* ```DbUpTimeout``` set to define the timeout value in minutes for scripts being executed
### Database engines
### MSSQL
* ```mssql```
#### Connection string
[Connection string parameters](https://www.connectionstrings.com/all-sql-server-connection-string-keywords/)
```
Server=<host>,<port>;Database=<database>;User Id=<username>;Password=<password>;
```
### MySQL
* ```mysql```
#### Connection string
[Connection string parameters](https://dev.mysql.com/doc/connector-net/en/connector-net-6-10-connection-options.html)
```
Server=<host>;Port=<port>;Database=<database>;Uid=<username>;Pwd=<password>;
```
### Oracle
* ```oracle```
#### Connection string
[Connection string parameters](https://docs.oracle.com/cd/E85694_01/ODPNT/ConnectionConnectionString.htm)
```
Data Source=<host>:<port>/<SID>;User Id=<username>;Password=<password>;
```
### Postgres
#### Engine name
* ```postgres```
#### Connection string
[Connection string parameters](https://www.npgsql.org/doc/connection-string-parameters.html)
```
Host=<host>;Port=<port>;Database=<database>;Username=<username>;Password=<password>;SSL Mode=Require;Trust Server Certificate=true
```
### Redshift
#### Engine name
* ```redshift```
#### Connection string
[Connection string parameters](https://www.npgsql.org/doc/connection-string-parameters.html)
```
Host=<host>;Port=<port>;Database=<database>;Username=<username>;Password=<password>;SSL Mode=Require;Trust Server Certificate=true;Server Compatibility Mode=Redshift
```
