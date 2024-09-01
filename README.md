# TacTicA .NET Server

Event driven distributed system using a microservices architecture.

## How it works

Overall, solution is based on microservices and utilizes an [event-driven approach](https://martinfowler.com/eaaDev/EventNarrative.html).
Each microservice reacts to events from the message queue. This approach allows to decouple microservices and run within its own environment by using containerization.

## Components

### TacTicA.Api

REST API Gateway. Most of API calls actually raise an event that is going to be catched and handled by OTHER service.

### TacTicA.Services.Identity

Stores user identities and authenticating requests using Custom Authentication solution based on JWT tokens. This service keeps the logic of sign up (register) and sign in (login) of the users.

### TacTicA.Services.Items

Domain service to keep some stored items.

### TacTicA.Common

Shared library. 

## Technologies stack used

Back-End:
- ASP .NET Core 8

Storage:
- NoSQL database MongoDB
- PostgreSql

Message queue:
- RabbitMQ using MassTransit

Delivery:
- Docker Compose

Testing:
- xUnit

##Patterns

1. CQRS:
- Command Handlers
- Event Handlers
- Flattened DTO's
- Separately running microservices

2. JWT Authentication

## How to test alive

### For development

In Visual Studio 2022 simply select Docker-Compose profile and run by F5. It will run all containers and then opens [Api Gateway Swagger file](https://github.com/optiklab/TacTicA.webapp.backend/blob/main/docs/swagger.api.json) on (https://localhost:5000/swagger/index.html):

Other swagger files available for [Identity server](https://github.com/optiklab/TacTicA.webapp.backend/blob/main/docs/swagger.identity.json) on (https://localhost:5050/swagger/index.html) and [Cities](https://github.com/optiklab/TacTicA.webapp.backend/blob/main/docs/swagger.cities.json) server on (https://localhost:5010/swagger/index.html), etc.

Also, [Postman collection available](https://github.com/optiklab/TacTicA.webapp.backend/blob/main/docs/tactica.postman_collection.json).

### Run on production

For example, you can run it on Linux Ubuntu 2020 in this way:
```bash
$> sudo apt-get install docker.io
$> sudo usermod -aG docker $USER
$> newgrp docker
$> docker ps 
$> sudo apt install -y docker-compose-v2
$> docker compose up
```

## Copyright

Copyright © 2024 Anton Yarkov. All rights reserved.
Contacts: anton.yarkov@gmail.com

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.