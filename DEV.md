# Development

## Local Docker build

> docker build -t bookref .
> docker run --rm -p 5544:80 -e ASPNETCORE_ENVIRONMENT=Development bookref

## ES References

Use docker-compose.yml to run the eventstore server.
Url localhost: <http://localhost:2113/web/index.html#/dashboard>

EventStore 20.6 - NOT WORKING atm

DI Injection
> https://developers.eventstore.com/clients/dotnet/generated/v20.6.1/connecting/di-extensions.html

### Used

- https://www.ahmetkucukoglu.com/en/event-sourcing-with-asp-net-core-01-store/#7--422-reading-an-event-read-events-from-stream-
