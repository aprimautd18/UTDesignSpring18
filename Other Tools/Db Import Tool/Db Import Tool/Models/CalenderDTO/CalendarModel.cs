﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.WebEncoders.Testing;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImprovedSchedulingSystemApi.Models.CalenderDTO
{
    public class CalendarModel
    {
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
