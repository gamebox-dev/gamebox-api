# GameBox API

Standalone backend for the Gamebox app.

## Requirements

* [dotnet 8](https://dotnet.microsoft.com/)

## Usage

### Visual Studio

Open the `gamebox.sln` file in Visual Studio and build/run it there.

### Command Line

Go into the `gamebox` directory:

```shell
cd gamebox
```

Build the application:

```shell
dotnet build --configuration Release
```

Run the resulting application:

```shell
./bin/Release/net8.0/gamebox
```

The app should be available on [localhost:5000](http://localhost:5000) which can
be verified using the `GET /hello` endpoint:

```shell
curl localhost:5000/hello
```

#### Alternatively

Run it directly via `dotnet run`:

```shell
dotnet run
```

### Environment Variables

#### IGDB

- Go to the Twitch dev console (https://dev.twitch.tv/) and sign in/sign up.

- Register a new application in the Applications section of the dashboard.

- Copy the Client ID, request a new Client Secret and note them down.

- Add IGDB_CLIENT_ID and IGDB_CLIENT_SECRET to the user environment variables on Windows and set them to the noted Client ID and Client Secret respectfully.

### API Documentation

Running it in development mode results in the Swagger/OpenAPI documentation
being available on [/swagger](http://localhost:5049/swagger/index.html).
