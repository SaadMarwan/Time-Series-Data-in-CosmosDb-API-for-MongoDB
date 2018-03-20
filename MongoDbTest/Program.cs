using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;


namespace MongoDbTest
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 60; i++)
            {
                try
                {
                    //Connect to Cosmos
                    MongoClient dbClient = new MongoClient("<your connection string>");


                    //TimeStamp (Truncate the Seconds part in the timestamp)
                    var timeStamp = DateTime.UtcNow;
                    timeStamp = new DateTime(timeStamp.Year, timeStamp.Month, timeStamp.Day, timeStamp.Hour, timeStamp.Minute, 00, timeStamp.Kind);


                    //Database List  
                    var dbList = dbClient.ListDatabases().ToList();

                    Console.WriteLine("The list of databases are :");
                    foreach (var item in dbList)
                    {
                        Console.WriteLine(item);
                    }

                    Console.WriteLine("\n\n");

                    //Get Database and Collection  
                    IMongoDatabase db = dbClient.GetDatabase("database");

                    var collList = db.ListCollections().ToList();
                    Console.WriteLine("The list of collections are :");
                    foreach (var item in collList)
                    {
                        Console.WriteLine(item);
                    }


                    // calling GetCollection with non-existing collection, it gets implicitly created with default options.
                    var collection = db.GetCollection<DataModel>("test");


                    //Generate Random Values
                    Random rnd = new Random();
                    int value = rnd.Next(60, 120); // creates a number between 1 and 12

                    //Data to store
                    DataModel document = new DataModel();
                    document.DeviceId = 100;
                    document.TimeStamp = timeStamp;
                    document.SensorTag = 1;
                    document.Readings = new Dictionary<string, string>() { { DateTime.UtcNow.Second.ToString(), value.ToString() } };




                    //Get the timestamp in millisecond to check against the documents in Cosmos (Timestamp in Cosmos is stored in Milliseconds format)
                    var timeStampInMilliseconds = timeStamp.ToUniversalTime().Subtract(
        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        ).TotalMilliseconds;







                    // Firt of all, try the update
                    var result = collection.UpdateOne(Builders<DataModel>.Filter.Eq("TimeStamp", timeStampInMilliseconds),
                     Builders<DataModel>.Update.Set("Readings." + DateTime.UtcNow.Second.ToString(), value.ToString()));



                    // If the update do not succeed, then try to insert the document
                    if (result.ModifiedCount == 0)
                    {
                        try
                        {
                            collection.InsertOne(document);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        // Here we are sure that the document exists.
                        // Retry to execute the update statement
                        // db.test.update(/* Same update as above */);
                    }







                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.ReadKey();
        }
    }
}
