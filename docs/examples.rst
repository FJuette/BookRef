Examples
=========

.. contents:: Table of contents
   :local:
   :backlinks: none
   :depth: 3

New User
--------

Every API user needs an account for the api. To create one use the **newUser** mutation:

.. code::

    mutation {
        newUser(input: {username: "admin", password: "secrect", email: "admin@example.com"}) {
            data
            errors {
                message
            }
        }
    }


In return you get the data property containing the **libraryId** from the new user when successfull.
The libraryId is returned only for information and not needed in future requests.

SignIn
------

Most of the later reqeusts need a valid jwt token containing the personal libraryId.
To get the token use the **signIn** mutation:

.. code::

    mutation {
        singIn(input: {username: "admin", password: "secret"}) {
            data
            errors {
                message
            }
        }
    }


Data contains the jwt token as string.
Bad credentials will return an error message.

Adding books
------------

All following mutations need a valid jwt token send with them in the http-header, e.g.:

.. code:: json

    {
        "Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwiTGlicmFyeUlkIjoiZWU0NzExMTUtMDQyNS00ODliLTkzMWEtOGIzZjdmMTg3MjA1IiwibmJmIjoxNjEwNDgwOTczLCJleHAiOjE2MTA1MTA5NzMsImlhdCI6MTYxMDQ4MDk3MywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIn0.gCEmi7IYWlk3TbiuIH1j5kH-BNriYBeJSlTanmYqO80"
    }


Adding books is a 2 phase transaction.
First we need the book itself, use the **addBook** mutation to create one. Save the bookId (*data.id*) for the next request.

.. code::

    mutation {
        addBook(input: {identifier: "987-2342-345435", title: "Example book"}) {
            errors {
                message
                code
            }
                data {
                    id
                    title
            }
        }
    }

Alternative by providing the isbn:

.. code::

    mutation {
        addBookByIsbn(input: {isbn: "9783446419377"}) {
            errors {
                message
                code
            }
                data {
                    id
                    title
            }
        }
    }


Secondly we need to put the book into our library using the **moveBookInLibrary** mutation.
An error is returned when:

* the book already is in the users library

.. code::

    mutation {
        moveBookInLibrary(input: {bookId: "Qm9vawpsOA==", status: ACTIVE}) {
            data {
                id
            }
            errors {
                message
                code
            }
        }
    }


As alternative we can add the book as recommendation using the **addBookRecommendation** mutation. Optionally you can add a note for this relation.

.. code::

    mutation AddBookRec {
        addBookRecommendation(input: { sourceBookId: "Qm9vawpsMQ==", targetBookId: "Qm9vawpsOA==", note: "Test note" }) {
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

Finding data
------------

For authors and books a filter input type is defined. Usage example to filter the list of authors.

.. code::

    query {
        authors(where: { name: { contains: "h" } }) {
            name
        }
    }

    query {
        people(where: { name: { startsWith: "Ch" } }) {
            name
        }
    }

Change status of book
---------------------

First find the **id** of the book in the personal library (not the bookId).
Goal is to change the status of the book for this user, not for all users.

.. code::

    query {
        books {
            status,
            id
        }
    }

Change the status of the book.

.. code::

    mutation ChangeStatus {
        changeBookStatus(input: {personalBookId: "UGVyc29uYWxCb29rCmwx", newStatus: DONE}) {
            data {
                id
                status
            }
            errors {
                message
            }
        }
    }

Remove book from personal library
---------------------------------

First find the **id** of the book in the personal library (not the bookId).
Goal is to remove the book from the library for this user, not for all users.

**data** ist *true* when mutation is successfull.

.. code::

    mutation {
        remove (input: { personalBookId: "UGVyc29uYWxCb29rCmw2"}) {
            errors {
                code
                message
            }
            data
        }
    }

Sorting the personal book library results
-----------------------------------------

The **lastChange** date is automatically set on each database operation (add/modify).
e.g. to get the books sorted by change date use:

.. code::

    query {
        books(order: {lastChanged: DESC}) {
            lastChanged
            book {
                title
                identifier
            }
        }
    }
