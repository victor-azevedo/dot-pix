# DotPix Worker

This repository serves as the implementation of the **DotPix Worker**, which is part of the DotPix project. The DotPix
project simulates the functionality of the "Pix API" provided by the "Banco Central", and it is intended for educational
purposes only.

## Functionality

The main purpose of the DotPix Worker is to act as a worker service developed using .NET Core 8. It implements a message
broker service with RabbitMQ. The worker service consumes the "payments" queue from the DotPix API, forwarding requests
to third-party Payment Providers. This includes sending payment requests to the Payment Service Provider (PSP) of the
Pix destination and updating the payment status to the Payment Provider originating from the Pix request.

## How to Use

As an integral component of the DotPix project, this application is integrated into the Docker Compose setup of the
DotPix project.

### Database scaffold

Optionally, you can use the following command to scaffold the database context. This command is only necessary if you
need to generate the database context and entity models from an existing PostgreSQL database:

```bash
dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=dotpix_dev" Npgsql.Enti
tyFrameworkCore.PostgreSQL --data-annotations --context-dir Data --output-dir Models
```

_**Note:** This command should be used with caution_. Ensure that you are only using it in a development or testing
environment and not in a production environment. Also, be cautious with the database connection string parameters,
especially sensitive information like passwords, to avoid compromising the security of your application.

## References

For more information, please refer to the documentation of the DotPix project.

## Disclaimer

This application is for educational purposes only and should not be used in a production environment.