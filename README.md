# Neuralm-Server

The neuralm server creates and manages the neural networks in the neuralm project.

These neural networks are created and mutated on the server and then distributed to the client. This way, we can combine the processing power of multiple computers and run numerous training sessions at the same time.
This is needed to reduce the overall training time.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites
You will need the following tools:

* [Visual Studio 2019](https://www.visualstudio.com/downloads/)
* [.NET Core SDK 2.2](https://www.microsoft.com/net/download/dotnet-core/2.2)
* [SQLServer 2017](https://www.microsoft.com/nl-nl/sql-server/sql-server-downloads)


### Setup
Follow these steps to get your development environment set up:

1. Clone the repository.
2. Create an `appSettings.json` file at the root of the Presentation.CLI layer with the connectionstring to the new database:
    ```json
    {
        "Jwt": {
          "Secret": "{YOUR SECRET KEY, MINIMUM LENGTH OF 17!}" 
        },
        "Server": {
          "Port": 9999,
          "Host": "{COMMON NAME OR FULLY QUALLIFIED DOMAIN NAME}"
        },
        "NeuralmDb": {
          "ConnectionString": "Server={YOUR SERVER NAME\\MSSQLINSTANCE};Database=NeuralmDbContext;Trusted_Connection=True;MultipleActiveResultSets=true",
          "UseLazyLoading": false
        } 
    }
    ```
    NOTE: Keep `UseLazyLoading` on false for now!
3. Afterwards generate a self signed certificate using the script in powershell (Administrator mode!)
	```powershell
	New-SelfSignedCertificate -DnsName "{COMMON NAME OR FULLY QUALLIFIED DOMAIN NAME}" -CertStoreLocation "cert:\LocalMachine\My" -FriendlyName "NeuralmCert"
	```
4. Next, go to `Tools > NuGet Package Manager > Package Manager Console` in visual studio, To restore all dependencies:
    ```
    dotnet restore
    ```
    Followed by:
    ```
    dotnet build
    ```
    To make sure all dependencies were added succesfully, it should build without dependency warnings else you have probably not installed .NET core 2.2 SDK.
5. Once the build has run successfully, start the server to confirm that the database connection is successful either by hitting `F5` or go to `Debug > Start`. A console will launch and start initializing. Upon completion, the console will look like this:

  
 ![Screenshot](https://github.com/neuralm/Neuralm-Server/blob/master/docs/images/Successful%20installation.png?raw=true)

## Running the tests

**NOTE:** For the tests each method will create its own database with a random GUID and delete itself after completion of the test. This is done so that the tests can run in parallel and do not have any dependencies. Because InMemoryDatabase is not *yet* a relational-database provider, some tests will use the repository instead of the service to make the tests work as intended.

## Deployment

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
