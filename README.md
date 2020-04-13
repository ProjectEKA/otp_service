## :speech_balloon: Otp service

> Triggering message to user 

## Build Status

[![Build](https://github.com/ProjectEKA/otp_service/workflows/master/badge.svg)](https://github.com/ProjectEKA/otp_service/actions)

## :+1: Code Style

[C# Naming Conventions](https://github.com/ktaranov/naming-convention/blob/master/C%23%20Coding%20Standards%20and%20Naming%20Conventions.md)

## :tada: Language/Frameworks

-   [C#](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/)
-   [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-3.1)
-   [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
-   [Bogus](https://github.com/bchavez/Bogus)

## :checkered_flag: Requirements

-   [dotnet core >=3.1](https://dotnet.microsoft.com/download)
-   [docker >= 19.03.5](https://www.docker.com/)

## :whale: Running From The Docker Image

Create docker image

```
```

To run the image

```
```

To use docker compose locally

```
docker-compose up -d
```

To use docker compose to run pre-existing image from docker-hub

```
export {ENVIRONMENT_VARIABLE}={ENVIRONMENT_VALUE}
docker-compose -f docker-compose.yml -f docker-compose.{environment}.yml up -d

Example:
export IMAGE_TAG=13f9004
docker-compose -f docker-compose.yml -f docker-compose.development.yml up -d
```

## :rocket: Running From Source
To run 

```
```

## Running The Tests

To run the tests 
```
```

## Features

1.  Send and verify otp.
2.  Send notification to user on consent request.

## API Contract

Once ran the application, navigate to

```alpha
{HOST}/swagger/index.html
```
