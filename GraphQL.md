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
