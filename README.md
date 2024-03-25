# API DotPix

## Description
The DotPix project is an application developed to meet the demands of the Pix instant payment system, implemented by the Central Bank of Brazil. DotPix simulates Pix transactions, creation of Pix keys, payment registration between accounts, and payment reconciliation in financial institutions.

## Components
**DotPix API**: Core of the DotPix application. It is a Web API REST developed in .NET Core 8.0.2, with layered architecture and token authentication. Responsible for receiving requests from Payment Service Providers (PSPs), validating and responding, and forwarding long or third-party dependent processing to specific workers.

**DotPix Payment Worker**: Worker Service application developed in .NET Core 8.0.2. Receives already validated and processed data from the DotPix API regarding payment requests from an originating PSP, confirms the payment with a destination PSP API, and updates the payment status based on the destination PSP's result.

**DotPix Conciliation Worker**: Worker Service application developed in .NET Core 8.0.2. Responsible for the reconciliation process between operations performed by the DotPix API and operations recorded by the PSP.

## Technologies Used
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Docker
- Grafana
- k6

## How to Use
1. Clone este repositório: `git clone git@github.com:victor-azevedo/dot-pix.git`
2. Navegue até o diretório do projeto: `cd dot-pix/DotPix`
3. Execute a aplicação: `dotnet ef database update`
4. Volte para a raiz do projeto
5. Execute `docker compose up`


