# Linkeeper
Project for storing and orginizing different links.

[Windows] To run simply type `dotnet run` into terminal.

_API Endpoints:_
|       URI      |    Verb    | Operation | Description           |  Success  |    Failure    |
|:---------------|:----------:|:---------:|:---------------------:|:---------:|--------------:|
| api/links      |  **GET**   |    READ   | Read all links        |   200 OK  | 404 Not found |
| api/links/{id} |  **GET**   |    READ   | Read a single link    |   200 OK  | 404 Not found |
| api/links/     |  **POST**  |   CREATE  | Create a new link     |   201 Created  | 400 Bad request, 405 Not allowed |
| api/links/{id} |  **PUT**   |   UPDATE  | Update an entire link |   204 No content  | - |
| api/links/{id} | **DELETE** |   DELETE  | Delete a single link  |   204 No content  | - |