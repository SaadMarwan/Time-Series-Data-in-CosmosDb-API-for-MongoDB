using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbTest
{
    public class HeartRateModel
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        [BsonElement("DeviceId")]
        public int DeviceId { get; set; }

        [BsonElement("TimeStamp")]
        public DateTime TimeStamp { get; set; }


        [BsonElement("SensorTag")]
        public int SensorTag { get; set; }




        public Dictionary<string, string> Readings;

      //  public string [] reading { get; set; }






    }
}
