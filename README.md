# Blog - Backend

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li><a href="#built-with">Built With</a></li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#dependencies">Dependencies</a></li>
        <li><a href="#execution">Execution</a></li>
      </ul>
    </li>
  </ol>
</details>

## About The Project

This project is a RESTful API to manage a blog, allows to create, edit and publish posts.

The application has 3 types of user:

- Public: can only get published posts and add comments to it
- Writer: can create, edit and submit posts
- Editor: can approve/reject submitted posts

Detailed documentation of the API endpoints -> [API documentation](https://laryssacarvalho.github.io/blog-backend/)

There is also a simple frontend application that uses this API ([click here](https://github.com/laryssacarvalho/blog-frontend) to access the frontend repository).

## Built With

The project was built using a Web API from [.NET 6](https://dotnet.microsoft.com/en-us/) and a SQL Server database.

I chose .NET because it's the backend framework I have the most experience with.

## Getting Started

~~This project is deployed using an Azure App Service, so you can make the requests to the following URL: https://laryssablog.azurewebsites.net/~~

You can also run this project on your machine.

### Dependencies

If you are going to execute the project on your machine, then you will need to install [.NET](https://dotnet.microsoft.com/en-us/download) and a SQL Server.

This is not a actually a dependency (since you can make requests directly to the endpoint) but if you want, you can execute the frontend application to see both of them running together (check the project documentation to see how to execute it):

* [Frontend project](https://github.com/laryssacarvalho/blog-frontend)
  
### Execution

#### Sample credentials

If you are using the API deployed in Azure, you can use the following user credentials for testing:

| Role | Email | Password |
|---|---|---|
| Public | public@public.com | public |
| Writer | writer@writer.com | writer |
| Editor | editor@editor.com | editor |

#### On your machine

After cloning this repository you will need to run the *roles.sql* from the Scripts folder on your database to create the user roles. 
If you want to use the same sample users provided above, run the *users.sql* on your database as well.

Then execute the following commands on the root folder:

```
dotnet restore
dotnet build
```

Access the Web API project folder (/BlogApi) and execute this one to run the project.
```
dotnet run
```
The API is now running on https://localhost:7273.


*It took me about 10 hours to finish this API, from the code to the deploy.*
