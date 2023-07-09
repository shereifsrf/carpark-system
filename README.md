## Usages
- Install docker from [here](https://docs.docker.com/engine/install/)
- Rename .env.example to .env
- make sure directory is CPM then run below command

````
$ make run
````
### thats it. [localhost:9000](http://localhost:9000) is ready to use
<br/>

## Modules
### 1. Client
- Client is a web application which is used to interact with the server.
- Client is built using Blazor WebAssembly.
- Client is hosted on the server itself.
- Client is accessible at [localhost:9000](http://localhost:9000)

### 2. Server
- Web application which is used to serve the client.
- Built using ASP.NET Core.
- Hosted on the docker container.
- PostgreSQL is used to store data.
- Database is hosted on the docker container.
- Entity Framework Core is used to interact with the database.

### 3. Shared
- Shared is a class library which is used to share common code between client and server.
- Shared is built using .NET Standard.

### 4. WPF.Client
- WPF.Client is a desktop application which is used to interact with the server.
- WPF.Client is built using WPF.
- WPF.Client is hosted on the client machine itself.

### 5. Server.Tests
- Server.Tests is a class library which is used to test the server.
- Server.Tests is built using .NET Core.


