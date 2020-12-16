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

## GraphQl References

### Used (HotChocolate)

- https://github.com/ChilliCream/graphql-workshop/blob/master/docs/1-creating-a-graphql-server-project.md
- https://dev.to/michaelstaib/get-started-with-hot-chocolate-and-entity-framework-e9i
- https://github.com/ChilliCream/hotchocolate/tree/main/examples

### General

- https://fiyazhasan.me/graphql-with-net-core-part-v-fields-arguments-variables/
- https://fiyazhasan.me/tag/graphql/
- https://graphql-dotnet.github.io/docs/getting-started/databases
- https://github.com/SimonCropp/GraphQL.EntityFramework/blob/master/docs/configuration.md#register-in-container
- https://volosoft.com/blog/Building-GraphQL-APIs-with-ASP.NET-Core
