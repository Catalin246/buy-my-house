# BuyMyHouse

## Overview

BuyMyHouse is an application that serves as an estate agent listing various houses for sale. Additionally, it handles mortgage applications through a backend Web API and Azure Functions. The application can be run locally with .NET commands or within a Docker environment using `docker-compose`.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local)
- [Docker](https://docs.docker.com/get-docker/)
- [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

## Run without Docker

To run the API and Functions locally without Docker, follow these steps:

### 1. Run the API

In the `buy-my-house.api` project directory, run:

```bash
dotnet run
```

The API should now be accessible at `http://localhost:5000`.

### 2. Run the Functions

In the `buy-my-house.functions` project directory, run:

```bash
func start
```

This starts the Azure Functions locally on `http://localhost:5002`.

## Run with Docker

The project can be run using Docker Compose, which will set up the Web API, Azure Functions, SQL Server, and Azurite storage emulator in a connected environment.

### Steps to Run in Docker

1. **Build and Run**:

   ```bash
   docker-compose up --build
   ```

   This will build the Docker images and start the containers for all services.

2. **Shut Down**:

   To stop the project, press `CTRL + C` in the terminal where it's running. To fully remove the containers and networks, run:

   ```bash
   docker-compose down
   ```

   This stops and removes all containers, networks, and volumes associated with the project.

## Connecting to Azurite and SQL Server

To interact with the storage emulator (Azurite) and SQL Server running in Docker, follow these steps:

### Connect to Azurite using Azure Storage Explorer

1. **Open Azure Storage Explorer**.

2. **Add a Local Emulator**:
   - Click **"Connect to Azure Resources"** in Azure Storage Explorer.
   - Choose **"Attach to a local emulator"** from the options.

3. **Configure Emulator Connection**:
   - Set the **Display Name** as `Azurite`.
   - Use the following endpoints based on the Docker setup:

     - **Blob Endpoint**: `http://127.0.0.1:10000/devstoreaccount1`
     - **Queue Endpoint**: `http://127.0.0.1:10001/devstoreaccount1`
     - **Table Endpoint**: `http://127.0.0.1:10002/devstoreaccount1`

   - **Account Name**: `devstoreaccount1`
   - **Account Key**: Use the default Azurite key:

     ```
     Eby8vdM02xNOcqFeZbP==;
     ```

4. **Connect**:
   - Click **Next**, then **Connect**. You should now see `Azurite` in the **Local & Attached > Storage Accounts** section in Azure Storage Explorer.

### Connect to SQL Server using SQL Server Management Studio (SSMS)

1. **Open SQL Server Management Studio (SSMS)**.

2. **Connect to SQL Server**:
   - **Server name**: Enter `localhost,1433`
   - **Authentication**: Choose `SQL Server Authentication`.
   - **Login**: Enter `sa`.
   - **Password**: Use the password specified in `docker-compose.yml` (e.g., `YourPassword123`).

3. **Connect**:
   - Click **Connect** to access the SQL Server instance running in Docker.

4. **Add and Apply Entity Framework Migrations**

   Before adding and applying Entity Framework migrations, ensure the database buymyhousedb exists. If it is not yet created, you can create it manually using the provided initial-database.sql script in SQL Server Management Studio (SSMS).

   - Create a migration "Migration", which generates code to create tables based on your models:
   bash
   ```
   dotnet ef migrations add Migration
   ```
   - Apply the migration to update the database schema:
   bash
   ```
   dotnet ef database update
   ```

## Environment Variables

The following environment variables are used within the Docker setup to connect the API and Functions to SQL Server and Azurite:

- **SQL Server Connection String**: 
  ```
  Server=sqlserver,1433;Database=buymyhousedb;User Id=sa;Password=YourPassword123;
  ```

- **Azurite Storage Connection String**:
  ```
  DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFeZbP==;BlobEndpoint=http://azurite:10000/devstoreaccount1;TableEndpoint=http://azurite:10002/devstoreaccount1;
  ```

## Execute Tests
To run the unit tests for the BuyMyHouse project, follow these steps:

1. **Navigate to the Test Project Directory**
Move to the directory containing the test project (buy-my-house.tests):

bash
```
cd buy-my-house.tests
```
2. **Run Tests**
Use the dotnet test command to execute all tests:

bash
```
dotnet test
```
3. **View Test Results**
After the command completes, you will see a summary of the test results in the terminal, including the number of tests passed, failed, and skipped.
For detailed results, use the --logger option:
bash
```
dotnet test --logger "console;verbosity=detailed"
```
4. **Run Specific Tests**
To run a specific test class or method, use the --filter option:

bash
```
dotnet test --filter "FullyQualifiedName~Namespace.ClassName.MethodName"
```