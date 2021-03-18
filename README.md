# Version-Server
This application determines the next semver version number based on the branch name
It is very specific for the following workflow and if this is how you work as well then this code may work for you as well.

# Swagger File
Normally when we run the version.api from Visual Studio it by defaults will bring up the Swagger file which is a really easy way to see and test the interface.  However if I am running this from VS Code using a run command it of course does not bring up this page and I can never seem to remember what the actual path is cause I just don't do it enough is my guess.  So here is the full path.
```
https://localhost:5001/swagger/index.html
```

# Workflow
_This does **NOT** follow or support git-flow, this workflow is based around the idea that we work on two kinds work:_
1. **Features** which is new functionality, adding things to your program that it never did before.
1. **Bugs** which is not new functionality but where we fix something that did not behave quite as expected.

_In addition to the type of work that this solution supports it also has the following assumptions:_
1. You work on each item, one at a time.  You do not start a bunch of items and try to work on them at the same time or in a batch.  This process follows one item to completion.
1. You deploy the code through the pipeline all the way to production when it is done.  Small changes and small deploymnents.
1. You create a new branch for every work item that you are working on (the combination or feature and bug branches we will refer to as topic branches)
1. At the completion of a topic branch you do a pull request back to the master branch.
1. Code that lands in Production will only come from the master branch.
1. You only use one CI/CD pipeline for the product no matter what branch you use.
1. The branch names follow a very specific pattern: bug/<short decription or issue id> feature/<short description or issue id>

# Special Features
One of the things that we have implemented recently is the idea of supporting breaking changes.  We supprt that by using the **Major** suffix for the topic branch.  This will increment the Major number and reset all the other numbers to zero.
Example: 
```
Major/UpdatedFramework
```
# Swagger Pages
In order to test the initial page on docker you will want to run the swagger pages which will give you a visual if the site is at least up and running and the api itself really has to GUI interface.  Here is how you would see this on the staging docker instance.
```
https://version.t3winc.com/swagger/index.html
```
