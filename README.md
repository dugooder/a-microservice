# a-microservice Template by Dugooder 

`Not ready for use. Under development` 

##Usage
Use this application as a template for creating micro-service.  

##Design
In progress

##Components

* [Ninject](http://www.ninject.org/) - dependency injection container to load the web services and depedencies.
* [Log4Net](https://logging.apache.org/log4net/) - for logging services.
* [Xunit](http://xunit.github.io/) - for unit testing.
* [Nuget](http://nuget.org/) - for package management.
* [Node](https://nodejs.org/) / [NPM](https://www.npmjs.com/) / [Gulp](http://gulpjs.com/) - for building the application.
* [NancyFx](http://nancyfx.org/) - a lightweight, low-ceremony, framework for building HTTP based services.
* [Topshelf](http://topshelf-project.com/) - a library to faciliate the implementation of a windows service.
* [Fluent Validation](https://github.com/JeremySkinner/FluentValidation) - A small validation library.

##Solution

###Projects
* `service` - is the 'runner.  

* `common` - common classes 

* `common.tests` -  unit tests for the common project

* `service.health` - a 'heart beat' service

* `service.health.tests` - a 'heart beat' service

##Build Setup
1. Download and install Nodejs.
2. Download the source from GitHub.
3. In the source directory use the command ```npm install``` to install the build's components.
4. To build the application use the command ```gulp build```.  The build must be run from the Visual Studio Command Prompt (2015 community edition used).

I suggest exploring the gulpfile.js for other gulp tasks like test, compile, and package.