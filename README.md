# cloud-crypo-microservices

Cloud-crypo-microservices is a backend project that using .NET 6, Docker and AWS. 4 Microservices are build (Coin Service, Portfolio Service, Report Service, User Service) using microservice archtiecture. Each service is connected to their own PostgreSQL instance and is containerized with Docker. In consequence each service is a single application. Every Microservice is deployed to AWS Cloud using AWS Fargate. AWS Fargate is a serverless compute engine where Amazon ECS is used to run containers. The biggest advantage of AWS Fargate is that no underlying Amazon EC2 instances need to be managed.

## start local

Postgres running as service with docker compose.

Volume is added to persist data over container lifecycle.

```console
docker-compose up -d
```

Check image and ports.

```console
docker ps
```

The upcoming commands must be executed for each microservice. To do this, change to the respective path.

Execute first migration.

```console
dotnet ef migrations add initial-migration
```

Create database with schema

```console
dotnet ef database update
```

