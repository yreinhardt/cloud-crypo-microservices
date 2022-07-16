# cloud-crypo-microservices

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

