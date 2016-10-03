# AngularPlayground

A playground to try out Angular 2.0, ASP.NET Core and Entity Framework Core. This project was initially created following the steps from the following blog posts by [@chsakellsBlog](https://twitter.com/chsakellsBlog):
* [https://chsakell.com/2016/06/23/rest-apis-using-asp-net-core-and-entity-framework-core/](https://chsakell.com/2016/06/23/rest-apis-using-asp-net-core-and-entity-framework-core/)
* [https://chsakell.com/2016/06/27/angular-2-crud-modals-animations-pagination-datetimepicker/](https://chsakell.com/2016/06/27/angular-2-crud-modals-animations-pagination-datetimepicker/)

## Prerequisites
* Visual Studio 2015 with Update 3 (Community will be fine) OR
* A text editor with OmniSharp installed (eg. VS Code, or Atom)
* .NET Core 1.0.1
* SQL Server LocalDB
* nodejs
* Bower

## Running the code in this project

### Run the API
There are two folders in the project. One folder consists of a Visual Studio Solution file with three projects:
* Scheduler.Data
* Scheduler.Model
* Scheduler.API

These projects can be opened and built in Visual Studio by opening Scheduler.sln in src\Scheduler\

If you're running Visual Studio, ensure the Scheduler.API project is set as your startup project, and run.

**Note:** Take a note of the address IISExpress uses to host the API project.  You will need this for the next section.

### Run the SPA
There is another folder called Scheduler.SPA, which houses an app written in Angular 2.0 and TypeScript

Go to the file ``` shared\utils\config.service.ts ``` and update the following line:

```
this._apiURI = 'http://localhost:26372/api/';
```
to whatever the url of the API is on your machine. 

To build Scheduler.SPA, run the following commands:
```
npm install
bower install
```

When the dependencies have installed, run the following:
```
npm run lite
```

Navigate to ``` http://localhost:3000 ``` and enjoy!

## Roadmap
* Get Kestrel working
* Use NMockaroo to build mock domain objects
* Build and run on Visual Studio Online
* Host on Azure
* Lots of playing around

## Copyright
Source code based almost entirely on [https://github.com/chsakell/angular2-features](https://github.com/chsakell/angular2-features)

Distributed under the MIT License.