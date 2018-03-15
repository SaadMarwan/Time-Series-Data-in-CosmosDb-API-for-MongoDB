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
            try
            {
                //Connect to Cosmos
                MongoClient dbClient = new MongoClient("mongodb://wearablestest:gGn9MPBanbGRRFaDYmC01ymcI8xzsnzJJjm7NpD8Caf52rmX04PwGeEbSDeyBeFj6noLobPtXPdbDhIpgnWECg==@wearablestest.documents.azure.com:10255/?ssl=true&replicaSet=globaldb");


                for (int i = 1; i < 1000; i++)
                {
                    //TimeStamp 
                    var truncDate = TimeSpan.Parse(string.Format("{0:HH:mm}", DateTime.UtcNow));
                    var timeStamp = DateTime.UtcNow;
                    timeStamp = new DateTime(timeStamp.Year, timeStamp.Month, timeStamp.Day, timeStamp.Hour, timeStamp.Minute, 00, timeStamp.Kind);

                    var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse("1521076380000"));

                    //Database List  
                    var dbList = dbClient.ListDatabases().ToList();

                    Console.WriteLine("The list of databases are :");
                    foreach (var item in dbList)
                    {
                        Console.WriteLine(item);
                    }

                    Console.WriteLine("\n\n");

                    //Get Database and Collection  
                    IMongoDatabase db = dbClient.GetDatabase("Biodata");
                    
                    var collList = db.ListCollections().ToList();
                    Console.WriteLine("The list of collections are :");
                    foreach (var item in collList)
                    {
                        Console.WriteLine(item);
                    }



                    var shardCollectionCommand = new CommandDocument();

                    var shardcollection = new BsonElement("shardcollection", "Biodata.Sensors3");
                    var key = new BsonElement("key", new BsonDocument("SensorTag", "hashed"));

                    shardCollectionCommand.Add(shardcollection);
                    shardCollectionCommand.Add(key);

                    db.RunCommand<string>(shardCollectionCommand);


                    var Sensors = db.GetCollection<HeartRateModel>("Sensors3");

                


                    ////CREATE  
                    //BsonElement personFirstNameElement = new BsonElement("PersonFirstName", "Saad");

                    //BsonDocument personDoc = new BsonDocument();
                    //personDoc.Add(personFirstNameElement);
                    //personDoc.Add(new BsonElement("PersonAge", 24));

                    Random rnd = new Random();
                    int value = rnd.Next(60, 120); // creates a number between 1 and 12

                    HeartRateModel heartRate = new HeartRateModel();
                    heartRate.DeviceId = 100;
                    heartRate.TimeStamp = timeStamp;
                    heartRate.SensorTag = 1;
                    heartRate.Readings = new Dictionary<string, string>() { { DateTime.UtcNow.Millisecond.ToString(), value.ToString() } };


                    // Sensors.InsertOne(heartRate);



                    var timeStampInMilliseconds = timeStamp.ToUniversalTime().Subtract(
        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        ).TotalMilliseconds;





                    //    //UPDATE  
                    //    BsonElement updatePersonFirstNameElement = new BsonElement("PersonFirstName", "Souvik");

                    //    BsonDocument updatePersonDoc = new BsonDocument();
                    //    updatePersonDoc.Add(updatePersonFirstNameElement);
                    //    updatePersonDoc.Add(new BsonElement("PersonAge", 24));

                    //    BsonDocument findPersonDoc = new BsonDocument(new BsonElement("PersonFirstName", "Saad"));

                    //    var updateDoc = Sensors.FindOneAndReplace(findPersonDoc, updatePersonDoc);

                    //    Console.WriteLine(updateDoc);

                    //    //DELETE  
                    ////    BsonDocument findAnotherPersonDoc = new BsonDocument(new BsonElement("PersonFirstName", "Sourav"));

                    //  //  things.FindOneAndDelete(findAnotherPersonDoc);



                    // Firt of all, try the update
                    //var result = Sensors.UpdateOne(Builders<HeartRateModel>.Filter.Eq("DeviceId", 10),
                    //      Builders<HeartRateModel>.Update.Set("DeviceId",11));


                    //var result2 = Sensors.UpdateOne(Builders<HeartRateModel>.Filter.Eq("DeviceId", 10),
                    // Builders<HeartRateModel>.Update.Set("Posts."+DateTime.Now.Millisecond ,"607"));


                    var result3 = Sensors.UpdateOne(Builders<HeartRateModel>.Filter.Eq("TimeStamp", timeStampInMilliseconds),
                     Builders<HeartRateModel>.Update.Set("Readings." + DateTime.UtcNow.Millisecond.ToString(), value.ToString()));



                    // If the update do not succeed, then try to insert the document
                    if (result3.ModifiedCount == 0)
                    {
                        try
                        {
                            Sensors.InsertOne(heartRate);
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
            





            //    //READ  
            //    var resultDoc = Sensors.Find(new BsonDocument()).ToList();
            //    foreach (var item in resultDoc)
            //    {
            //        Console.WriteLine(item.ToString());
            //    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}
