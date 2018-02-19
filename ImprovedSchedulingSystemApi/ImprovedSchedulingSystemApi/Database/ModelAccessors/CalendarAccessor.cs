using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ImprovedSchedulingSystemApi.Database
{
    public class CalendarAccessor : MongoAbstract<CalendarModel>
    {
        public override IMongoCollection<CalendarModel> collectionSet()
        {
            return db.GetCollection<CalendarModel>("CalendarData");
        }

         public CalendarModel dateLookup(string calender_name, DateTime start_datetime)
        {

            var builder = Builders<CalendarModel>.Filter; // Contructs a filter for the search.(Id normally use a lambda but in this case we need to incrumentally build the query)
            FilterDefinition<CalendarModel> filter = FilterDefinition<CalendarModel>.Empty; // Place to store the filter for the find query

            ;

            filter = filter & builder.Eq(x => x.calName, calender_name);

            filter = filter & builder.Gte(x => x.startTime, start_datetime.Date);

            filter = filter & builder.Lte(x => x.startTime, start_datetime.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999));


            List<CalendarModel> returnedItems = collection.Find(filter).ToList();
            return returnedItems.FirstOrDefault();


        }

        public List<AppointmentModel> appointmentLookupById(ObjectId id)
        {
            return collection.AsQueryable().SelectMany(x => x.appointments).Where(x => x.CustomerId == id).ToList();

        }


    }
}
