# PSP API Mock

This repository serves as a mock for the PSP (Payment Service Provider) API, specifically tailored for handling PIX
payments.

## Functionality

The main purpose of this mock is to simulate the behavior of a PSP API.

* **GET _/health_**: This route is used to check the health status of the API.


* **POST _/payments/pix_**: This route simulates the creation of PIX payments. It generates random waiting times to
  emulate varying response times.


* **PATCH _/payments/pix_**: This route simulates updating PIX payments.

## How to Use

As an integral component of the DotPix project, this application is integrated into the Docker Compose setup of the
DotPix project.

However, it can also be run independently using the provided Dockerfile.

Run the Docker container to start the mock server.

```bash
docker build -t psp-mock .
docker run -p 'choice_a_port':8080 psp-mock
```

Make requests to the specified endpoints (**/health**, **/payments/pix**) to interact with the mock PSP API.

Observe the varying response times as part of the simulation.

## References

For more information, refer to the original repository: [PSP Mock Repository](https://github.com/DiegoPinho/psp-mock)

## Disclaimer

This mock is for testing and development purposes only. It is not intended for use in production environments.