# GraphQL

Example with query:

```graphql
query {
  books (where: { status: { eq: ACTIVE } } ) {
    userId
    currentPage
    status
    book {
      language
      title
      bookAuthors {
        author {
          name
        }
      }
      created
      isbn
      bookCategories {
        category {
          name
        }
      }
    }
  }
}
```

```graphql
query {
  people (where: {id: { eq: 1 } }) {
    id
    name
  }
}
```

## References

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
