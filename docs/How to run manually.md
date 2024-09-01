# TacTicA .NET Server

How to test alive
-----------------

1. Run mongo db instance and rabbit mq:
```bash
$>docker run -d -p 27017:27017 --hostname tactica-mongo --name tactica-mongo mongo
@>docker run -d --rm -p 15672:15672 -p 5673:5672 -p 5671:5671 --hostname tactica-rabbitmq --name tactica-rabbit rabbitmq
```

2. Go to Api project and run on HTTP port 5000:
```bash
$>dotnet run
```
3. Go to Items project and run on HTTP port 5005:
```bash
$>dotnet run --urls "http://+:5005"
```
Result: Check that default Categories collection is created in MongoDb

4. Go to Identity project and run on HTTP port 5050:
```bash
$>dotnet run --urls "http://+:5050"
```
5.  Run Postman and execute:
```bash
GET http://localhost:5000/api/items HTTP/1.1
Host: localhost:5000

Response: 401 unauthorized
```
6.  Run Postman and execute to create first user:
```bash
POST http://localhost:5050/api/accounts/signup HTTP/1.1
Host: localhost:5050
Content-Type: application/json
cache-control: no-cache
body:
{
	"email":"user1@email.com",
	"name":"anton",
	"password":"test1234"
}
```
Result: Check that User is created in MongoDb

7.  Run Postman and execute:
```bash
POST http://localhost:5050/api/accounts/signin HTTP/1.1
Host: localhost:5050
Content-Type: application/json
cache-control: no-cache
body:
{
	"email":"user1@email.com",
	"password":"test1234"
}

Response:
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmM3MDZhNi1jOGI5LTQ4NGItOTIzMC0zNjU2Y2UzM2JiOTMiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwNTAiLCJpYXQiOjE1NzIxMjk5MzcsImV4cCI6MTU3MjEzMDIzNywidW5pcXVlX25hbWUiOiJiNmM3MDZhNi1jOGI5LTQ4NGItOTIzMC0zNjU2Y2UzM2JiOTMifQ.lXQ5rrVyANYFmvIa8s6vJts165U2E7Q8sQtzfEUugjw",
    "expires": 1572130237
}
```
You can check content of token on https://jwt.io/

8.
```bash
GET http://localhost:5000/api/items HTTP/1.1
Host: localhost:5000
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmM3MDZhNi1jOGI5LTQ4NGItOTIzMC0zNjU2Y2UzM2JiOTMiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwNTAiLCJpYXQiOjE1NzIxMjk5MzcsImV4cCI6MTU3MjEzMDIzNywidW5pcXVlX25hbWUiOiJiNmM3MDZhNi1jOGI5LTQ4NGItOTIzMC0zNjU2Y2UzM2JiOTMifQ.lXQ5rrVyANYFmvIa8s6vJts165U2E7Q8sQtzfEUugjw
cache-control: no-cache

Response:
HTTP 200 OK
Secured content!
```
9. Run Postman and execute to create an Item inside of existing Category:
```bash
POST http://localhost:5000/api/items HTTP/1.1
Host: localhost:5000
Content-Type: application/json
cache-control: no-cache
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyNGU0YzExYy05MTlmLTRiYjYtYTk0ZS1lODlhZTAyZTkxZTciLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJpYXQiOjE1ODI1ODAxOTUsImV4cCI6MTU4MjU4MDQ5NSwidW5pcXVlX25hbWUiOiIyNGU0YzExYy05MTlmLTRiYjYtYTk0ZS1lODlhZTAyZTkxZTcifQ.rNM4ITesCOg9GRFc_fLN1Op9N3Bdl9gbfzPiXE_GBRs
body:
{
    "category": "hobby",
    "name": "blah blah1"
}
```
Result: Check that Item is created in MongoDb

10. Run:
```bash
GET http://localhost:5000/api/items/a21a15b9-420b-4c2a-8f63-c7d1d6399d2c  HTTP/1.1
Host: localhost:5000
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyNGU0YzExYy05MTlmLTRiYjYtYTk0ZS1lODlhZTAyZTkxZTciLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJpYXQiOjE1ODI1ODAxOTUsImV4cCI6MTU4MjU4MDQ5NSwidW5pcXVlX25hbWUiOiIyNGU0YzExYy05MTlmLTRiYjYtYTk0ZS1lODlhZTAyZTkxZTcifQ.rNM4ITesCOg9GRFc_fLN1Op9N3Bdl9gbfzPiXE_GBRs
cache-control: no-cache
```

Result: this item returned

