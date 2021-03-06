# Neuralm-Server

The neuralm server creates and manages the neural networks in the neuralm project.

These neural networks are created and mutated on the server and then distributed to the client. This way, we can combine the processing power of multiple computers and run numerous training sessions at the same time.
This is needed to reduce the overall training time.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites
You will need the following tools:

* [Visual Studio 2019](https://www.visualstudio.com/downloads/)
* [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* [Docker](https://www.docker.com/)

### Setup
Follow these steps to get your development environment set up:

1. Update the `appsettings.Development.json` files in UserService, TrainingRoomService & RegistryService to valid database connection strings (example):
```
"Database": {
    "ConnectionString": "Server=(LocalDb)\\MSSQLLocalDB;Database={SERVICE}DbContext;User=sa;Password=<PASSWORD>;",
    "UseLazyLoading": true,
    "DbProvider": "mssql"
 }
```
For `DbProvider` mssql and mysql are supported.
NOTE: leaving the connection string and db provider empty will result in using an in memory db context.
## Running the tests
There are front-end and back-end tests.

### Running the front-end tests
1. Navigate to the `src/Neuralm.Presentation.Web/`folder.
2. Run the command: 
```
npm run test:unit
```

### Running the back-end tests
1. Navigate to the `src/` folder.
2. Run the command: 
```
dotnet test
```

## Deployment
To deploy the server:
```
docker-compose up
```

The website will be available at localhost/login.


## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/neuralm/Neuralm-Server/tags). 

## Authors

* **Glovali** - *Initial work* - [Metalglove](https://github.com/metalglove)
* **Suppergerrie2** - *Initial work* - [Suppergerrie2](https://github.com/suppergerrie2)

See also the list of [contributors](https://github.com/neuralm/Neuralm-Server/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments
