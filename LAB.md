# Docker Lab Report

## Project Information

**Project Name:** MyAPI  
**Technology:** ASP.NET Core Web API (.NET 8)  
**Containerization:** Docker and Docker Compose

## Part 0: Environment Verification

**Commands used:**
- `docker --version`
- `docker compose version`
- `dotnet --version`
- `docker run hello-world`

Docker and Docker Compose were used to verify the containerization environment. The .NET SDK version was checked to confirm the development environment. The Hello World container was executed to verify that Docker was working correctly.

**Checkpoint outputs:**
- Docker version: [Insert actual output]
- Docker Compose version: [Insert actual output]
- .NET SDK version: [Insert actual output]
- Hello World test: Successful

## Part 1: Nginx Container

The Nginx container was downloaded and executed using the official Nginx image. The container was published on host port 8081 and tested through a web browser.

**Checkpoint:**
- Image: `nginx`
- Host port: `8081`
- Container port: `80`
- Test URL: `http://localhost:8081`

The Nginx page was checked in the browser, and the container was stopped and removed after testing.

## Part 2: Dockerfile and API Image

A multi-stage Dockerfile was created using the .NET 8 SDK image for building and publishing the application and the ASP.NET 8 runtime image for running it.

The Dockerfile exposes port 8080 and configures the application to listen on HTTP.

**Checkpoint:**
- Build stage: `mcr.microsoft.com/dotnet/sdk:8.0`
- Runtime stage: `mcr.microsoft.com/dotnet/aspnet:8.0`
- API port: `8080`
- API image: `myapi:1.0`
- Previously observed image disk usage: approximately 324 MB

The API image was built and executed successfully, and the application startup logs confirmed that Kestrel was listening on port 8080.

## Part 3: API and SQL Server

Docker Compose was configured to run the API and SQL Server together. A named volume, `sql_data`, was configured to preserve SQL Server data. The database health check and `depends_on` condition were used to ensure that the API waited for the database to become healthy.

The `MyApiDb` database contains an `Items` table with sample records: Keyboard, Mouse, and Monitor.

**Checkpoint:**
- Database service: `db`
- Database: `MyApiDb`
- Table: `Items`
- Named volume: `sql_data`
- API endpoint: `/api/items`

Database persistence was verified by stopping and recreating the containers without deleting the named volume. The sample items remained available after the restart.

## Part 4: Complete Multi-Service Stack

The final Docker Compose setup includes Nginx, three API replicas, SQL Server, and Redis.

**Services:**
- Nginx: host port 80
- API: internal port 8080, scaled to three replicas
- SQL Server: internal port 1433
- Redis: internal port 6379

Nginx forwards incoming HTTP requests to the API service. Docker Compose manages the services, networking, database volume, and API replicas.

**Commands used:**
- `docker compose up -d --build --scale api=3`
- `docker compose ps`
- `docker compose logs --tail=50 api`
- `docker stats --no-stream`

**Verification:**
- Three API containers were running.
- The SQL Server container was healthy.
- The Nginx and Redis containers were running.
- The `/api/health` endpoint was tested through Nginx.
- The `/api/items` endpoint returned the sample database records.

**Resource monitoring:** The three API replicas used approximately 23–28 MiB of memory each during the recorded snapshot. SQL Server used approximately 728 MiB. These figures represent one snapshot and may change while the containers are running.

## Reflection

This lab helped me understand how Docker containers package and run an application consistently. I learned how Docker Compose connects multiple services and how Nginx forwards requests to the API. I also learned how to use a named volume to preserve database data and how to scale the API to three replicas. Finally, I practiced checking container status, application logs, and resource usage to monitor the system.
