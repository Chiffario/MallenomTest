# Setup

## Getting started

## Server

### Prerequisites

1. [Docker](https://www.docker.com/get-started/)
2. [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools)
### How-to

1. Run `docker compose up` in the project root (you can also add `-d` to detach the containers and let them run in the background)
  1.1. Optionally - inspect your docker processes via `docker ps`. You should see two containers - `mallenomtest` (the server) and `alpine:postgres` (the database)
  1.2. You can also verify healthiness of the server by making a GET request to `http://localhost:5141/health`. If the response is `Healthy`, the server is up and running
2. For the initial setup - apply migrations by using `dotnet ef database update`. This only needs to be done once
3. If you want to shutdown the database and the server while they are detached - run `docker compose down` in the project root

## Client

### Prerequisites

1. [.NET SDK 9](https://dotnet.microsoft.com/en-us/download)

### How-to

Simply run `dotnet run` in `MallenomTest.Client` (or `dotnet run --project ./MallenomTest.Client/` in project root) :)
