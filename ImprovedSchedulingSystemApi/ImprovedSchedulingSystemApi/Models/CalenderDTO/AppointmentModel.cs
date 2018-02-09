using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImprovedSchedulingSystemApi.Models.CalenderDTO
{
    public class AppointmentModel
    {
        [BsonElement("appointmentID")]
        public ObjectId id { get; set; }

        [BsonElement("customerId")]
        public ObjectId CustomerId { get; set; }

        [BsonElement("aptstartTime")]
        public DateTime aptstartTime { get; set; }

        [BsonElement("aptendTime")]
        public DateTime aptendTime { get; set; }

        [BsonElement("reason")]
        public string reason { get; set; }

    }
}
