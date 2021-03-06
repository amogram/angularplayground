# AngularPlayground

A playground to try out Angular 2.0, ASP.NET Core and Entity Framework Core. This project was initially created following the steps from the following blog posts by [@chsakellsBlog](https://twitter.com/chsakellsBlog):
* [https://chsakell.com/2016/06/23/rest-apis-using-asp-net-core-and-entity-framework-core/](https://chsakell.com/2016/06/23/rest-apis-using-asp-net-core-and-entity-framework-core/)
* [https://chsakell.com/2016/06/27/angular-2-crud-modals-animations-pagination-datetimepicker/](https://chsakell.com/2016/06/27/angular-2-crud-modals-animations-pagination-datetimepicker/)

I'm now working through the Angular 2 Book to get my head around Angular. You can see more about the book [here](https://www.ng-book.com/2/?utm_source=Gumroad+Customers&utm_campaign=7d9153a8fe-ng_book_2_r42_update_email&utm_medium=email&utm_term=0_90f81615bc-7d9153a8fe-96949213&goal=0_90f81615bc-7d9153a8fe-96949213). Check out the branch ng-book to see my progress.

## Prerequisites
* Visual Studio 2015 with Update 3 (Community will be fine) OR
* A text editor with OmniSharp installed (eg. VS Code, or Atom)
* .NET Core 1.0.1
* SQL Server LocalDB
* nodejs
* Bower

## Running the code in this project

### 1. Set up the Database
The database I'm using for this project is just LocalDB. Feel free to change this to SQL Server if you want.  Connection info is stored in the ``` appsettings.json ``` file of Scheduler.API

1. Open a command window within Scheduler.Data
2. Run the following commands:

```
dotnet ef migrations add "initial"
dotnet ef database update
```

### 2. Run the API
There are two folders in the project. One folder consists of a Visual Studio Solution file with three projects:
* Scheduler.Data
* Scheduler.Model
* Scheduler.API

These projects can be opened and built in Visual Studio by opening Scheduler.sln in src\Scheduler. You can also run the API using one of the following options:

#### Option 1: Running in IIS Express
If you're running Visual Studio, ensure the Scheduler.API project is set as your startup project, and run.

**Note:** Take a note of the address IISExpress uses to host the API project.  You will need this for the next section.

#### Option 2: Running in Kestrel
Open a command prompt in the Scheduler.API folder and enter the following
```
dotnet restore
dotnet run
```

### 3. Run the SPA
There is another folder called Scheduler.SPA, which houses an app written in Angular 2.0 and TypeScript

Go to the file ``` shared\utils\config.service.ts ``` and update the following line:

```
this._apiURI = 'http://localhost:26372/api/';
```
to whatever the url of the API is on your machine. 

Repeat the process for ``` shared\services\data.service.ts ```

```
_baseUrl: string = 'http://localhost:26372/api/';
```

After any changes to your TypeScript files, transpile by hitting [CTRL]+[SHIFT]+[B]

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
* ~~Get Kestrel working~~
* Use NMockaroo to build mock domain objects
* Build and run on Visual Studio Online
* Host on Azure
* Lots of playing around

## Copyright
Source code based almost entirely on [https://github.com/chsakell/angular2-features](https://github.com/chsakell/angular2-features)

Distributed under the MIT License.
