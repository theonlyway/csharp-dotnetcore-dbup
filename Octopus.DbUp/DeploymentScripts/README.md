# Steps
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

## Example folder structure
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
