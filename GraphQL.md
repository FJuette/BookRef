# GraphQL

## Use Cases

Get Books for user:

```graphql
query {
    books(where: { status: { eq: ACTIVE } }) {
        id
        status
        bookId
        book {
            title
            categories {
                name
            }
            authors {
                name
            }
        }
        currentPage
        format
        personalLibraryId
        startDate
        status
    }
}
```

Get a specific book by id:

```graphql
query {
    bookById(id: "Qm9vawpsMw==") {
        title
    }
}
```

Find all recommendations for a book:

```graphql
query {
    recommendationsForBook(id: "Qm9vawpsMg==") {
        sourceBook {
            title
        }
        bookRecommedations {
            id
            recommendedBook {
                title
            }
            note {
                content
            }
        }
        personRecommedations {
            id
            recommendedPerson {
                name
            }
            note {
                content
            }
        }
    }
}
```

All Categories ordered by name:

```graphql
query {
    categories(order: { name: ASC }) {
        id
        name
    }
}
```

## Testing

Example with filter:

```graphql
query {
    allBooks(where: { language: { eq: GERMAN } }) {
        nodes {
            title
        }
    }
}
```

Example with filter and order

```graphql
query {
    allBooks(where: { language: { eq: GERMAN } }, order: { isbn: ASC }) {
        nodes {
            title
        }
    }
}
```

Example for mutation

```graphql
mutation AddAuthor {
    addAuthor(input: { name: "Fabian Jütte" }) {
        author {
            id
        }
    }
}
```

Mutation with error response

```graphql
mutation {
    addAuthor(input: { name: "Xi" }) {
        author {
            id
            name
        }
        errors {
            code
            message
        }
    }
}
```

Add book recommendation

```graphql
mutation AddBookRec {
    addBookRecommendation(input: { sourceBookId: "Qm9vawpsMQ==", targetBookId: "Qm9vawpsMg==", note: "Test note" }) {
        data {
          note {
            content
          }
        }
        errors {
          message
        }
    }
}
```

Add book

```graphql
mutation AddBook {
  addBook(input: {isbn: "Optional", title: "Test buch", subtitle: "nur ein Test"}) {
    data {
      title
      subtitle
      id
    }
  }
}
```

Move book into library, e.g. from recommendation into active library or directly after creating the book.

```graphql
mutation MoveBookToLibrary {
  moveBookInLibrary (input: {bookId: "Qm9vawpsOQ==", status: ACTIVE, colorCode: "ffffff"}) {
    data {
      colorCode
      status
      book {
        title
      }
    }
  }
}
```

Add non existing author to book:

```graphql
mutation AddAuthor {
  addNewAuthor(input: {bookId: "Qm9vawpsOQ==", name: "Thomas Jeffersson"}) {
    data {
      authors {
        id
        name
      }
    }
  }
}
```

Attach a category to a book

```graphql
mutation AttachCategory {
  addCategory(input: {bookId: "Qm9vawpsOA==", categoryId: "Q2F0ZWdvcnkKbDQ2", isPrimary: true}) {
    data {
      categories {
        name
      }
    }
  }
}
```

Change personal book status (same for changing the color code):

```graphql
mutation ChangeStatus {
    changeBookStatus(input: {personalBookId: "UGVyc29uYWxCb29rCmwx", newStatus: DONE}) {
      data {
        id
        status
      }
    }
}
```

Add person

```graphql
mutation {
  addPerson(input: {name: "Thomas"}) {data {
    name
    id
    }
  }
}
```

Multi Queries

```graphql
query {
    a: recommendationsForBook(id: "Qm9vawpsMg==") {
        id
        sourceBook {
            title
            id
        }
        recommendedBook {
            title
        }
    }
    b: recommendationsForBook(id: "Qm9vawpsMQ==") {
        id
        sourceBook {
            title
            id
        }
        recommendedBook {
            title
        }
    }
}
```

Query for multiple Ids

```graphql
query {
    authorsById(ids: ["QXV0aG9yCmwx", "QXV0aG9yCmwy"]) {
        name
    }
}
```

Query with paging

```graphql
query GetFirstBook {
    books(first: 1) {
        edges {
            node {
                id
                title
            }
            cursor
        }
        pageInfo {
            startCursor
            endCursor
            hasNextPage
            hasPreviousPage
        }
    }
}
```

## References

### Used (HotChocolate)

-   https://github.com/ChilliCream/graphql-workshop/blob/master/docs/1-creating-a-graphql-server-project.md
-   https://dev.to/michaelstaib/get-started-with-hot-chocolate-and-entity-framework-e9i
-   https://github.com/ChilliCream/hotchocolate/tree/main/examples
-   Main Documentation: https://chillicream.com/docs/hotchocolate/

### General

-   https://fiyazhasan.me/graphql-with-net-core-part-v-fields-arguments-variables/
-   https://fiyazhasan.me/tag/graphql/
-   https://graphql-dotnet.github.io/docs/getting-started/databases
-   https://github.com/SimonCropp/GraphQL.EntityFramework/blob/master/docs/configuration.md#register-in-container
-   https://volosoft.com/blog/Building-GraphQL-APIs-with-ASP.NET-Core
