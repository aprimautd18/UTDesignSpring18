using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ImprovedSchedulingSystemApi.Models.CalenderDTO
{

    public enum StatusCodes
    {
        Scheduled = 0, CheckedIn = 1, InProcess = 2, Discharged = 4, Canceled = 5
    }

    /// <summary>
    /// The model for a single appointment
    /// </summary>
    public class AppointmentModel : IComparable<AppointmentModel>
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


        public static bool operator<=(AppointmentModel a, AppointmentModel b)
        {
            return (a.aptstartTime <= b.aptstartTime);
        }

        public static bool operator>=(AppointmentModel a, AppointmentModel b)
        {
            return (a.aptstartTime >= b.aptstartTime);
        }
        public static bool operator <(AppointmentModel a, AppointmentModel b)
        {
            return (a.aptstartTime < b.aptstartTime);
        }

        public static bool operator >(AppointmentModel a, AppointmentModel b)
        {
            return (a.aptstartTime > b.aptstartTime);
        }
        public static bool conflict(AppointmentModel a, AppointmentModel b)
        {
            return a.aptendTime > b.aptstartTime || a.aptstartTime > b.aptendTime;
        }


        public int CompareTo(AppointmentModel other)
        {
            if (this < other)
                return -1;
            
            if (this == other)
                return 0;

            return 1;
        }
    }

        

}
