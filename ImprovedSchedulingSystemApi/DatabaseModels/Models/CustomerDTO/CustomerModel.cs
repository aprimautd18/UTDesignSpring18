using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImprovedSchedulingSystemApi.Models.CustomerDTO
{

    /// <summary>
    /// The model for a single customer
    /// </summary>
    public class CustomerModel
    {
        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("firstName")]
        public string firstName { get; set; }

        [BsonElement("lastName")]
        public string lastName { get; set; }

        [BsonElement("phoneNumber")]
        public string phoneNumber { get; set; }
    }
}
