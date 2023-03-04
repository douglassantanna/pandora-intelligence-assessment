# Pandora intelligence assessment
This project was developed as an assessment for Pandora Intelligence.
It integrates with an Open Data API and an Entity Framework Core InMemory Database. Whenever a call is made to retrieve car information,
the information is persisted in the database if the car is found. If the car is not found, the user will receive a BadRequest.
This allows users to search for previously searched cars in the local database.
The project includes both unit and integration tests to ensure quality.

### Prerequisites
Before you can run the project, you must have the following software installed on your machine:
- .NET 7 SDK
- Docker

### Running the project
To run the project, follow these steps

1.Clone the repository to your machine
```
git clone https://github.com/douglassantanna/pandora-intelligence-assessment
```
2.Navigate to the project directory
```
cd pandora-intelligence-assessment
```
3.Build the Docker image:
```
docker build -t pandora-api .
```
4.Run the Docker container
```
docker run -p 8080:80 my-api-image
```

### Making HTTP calls
To make an HTTP call to the API, you must include the following key in the header:
```
"Pandora-Api-Key": "pandoraassessment"
```
- To make an HTTP call to the API in this project, you can:
copy and paste the following URL in your browser
```
http://localhost:8080/swagger/index.html
```
- use Postman, or any API platform, to make GET calls to the following URLs:
```
http://localhost:8080/api/pandora/place-here-the-car-plate
```

```
http://localhost:8080/api/pandora/addedcars?pageSize=10&pageIndex=0&sort=asc
```
> Note that the response is paginated, which means you can adjust the number of vehicles displayed per page, as well as change the page index.
