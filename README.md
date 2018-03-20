# Time-Series-Data-in-CosmosDb-API-for-MongoDB

Implementing Time Series data model with Azure Cosmosdb using Mongodb API

## Schema Design

To Store time series data in a relational database is straightforward, we would have a table and each row with a different timestamp.

```
TimeStamp                 EventValue
2018-01-01T10:01:00Z      100
2018-01-01T20:01:00Z      200
```

If we map this to MongoDB, we would end up with a document per event 

```
{
  TimeStamp: 2018-01-01T10:01:00Z
  Value: 100

},
{
  TimeStamp: 2018-01-02T10:01:00Z
  Value: 200

}

```

This model is 100% Valid but how can we refine it to provide better performance and storage efficiency.


### Document-Oriented Design
If we read the events every second, in a relational database we will have one row every second and in Mongo it's one document every second. But we can store multiple readings in a single document, i.e. One document per minute (60 readings in one document) 

```
{
  TimeStamp: 2018-01-01T10:01:00Z
  Values: {
    0:100,
    1:200,
    
    ...
    59:6000
  }
}
```
Because we are storing one value per second, we can simply represent each minute as fields 0 - 59



## Running the tests

1. Clone the project and change the connection string
2. Install MongoDB.Driver
Install-Package mongocsharpdriver -Version 2.5.0





## Built With

* .net core
* Azure CosmosDb
* Mongodb C# Driver


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* [Schema Design for Time Series Data in MongoDB](https://www.mongodb.com/blog/post/schema-design-for-time-series-data-in-mongodb)

