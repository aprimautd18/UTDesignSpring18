using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImprovedSchedulingSystemApi.Models.CalenderDTO
{
    public class CalendarModel
    {

        public CalendarModel()
        {
            appointments = new List<AppointmentModel>();
        }

        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("calName")]
        public string calName { get; set; }

        [BsonElement("startTime")]
        public DateTime startTime { get; set; }

        [BsonElement("endTime")]
        public DateTime endTime { get; set; }

        [BsonElement("appointments")]
        public List<AppointmentModel> appointments { get; set; }

        public DateTime getDate()
        {
            return startTime.Date;

        }

       

    }
}
