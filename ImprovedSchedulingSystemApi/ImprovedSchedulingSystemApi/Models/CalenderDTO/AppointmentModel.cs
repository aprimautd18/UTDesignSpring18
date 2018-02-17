using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImprovedSchedulingSystemApi.Models.CalenderDTO
{

    public enum StatusCodes
    {
        CheckedIn=1, Scheduled=2, Canceled=3, Discharged=4, InProcess=5
    }

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
        
        [BsonElement("status")]
        public StatusCodes status { get; set; }

    }
}
