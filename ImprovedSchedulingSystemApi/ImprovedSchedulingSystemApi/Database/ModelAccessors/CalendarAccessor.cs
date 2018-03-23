using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        

        public List<AppointmentModel> appointmentLookupByCustomerId(ObjectId id)
        {
            return collection.AsQueryable().SelectMany(x => x.appointments).Where(x => x.CustomerId == id).ToList();

        }

        public bool updateAppointmentStatus(ObjectId _id, StatusCodes newCode)
        {
            var findAppointmentFilter = Builders<CalendarModel>.Filter.Where(x => x.appointments.Any(y => y.id == _id));
            var updateAppointmentFilter = Builders<CalendarModel>.Update.Set(x => x.appointments[-1].status, newCode);
            UpdateResult updateResult = collection.UpdateOne(findAppointmentFilter, updateAppointmentFilter);
            return updateResult.IsAcknowledged;
        }

        public List<string> retreiveCalendarNames()
        {
            return collection.AsQueryable().Select(x => x.calName).Distinct().ToList();
        }

        public AppointmentModel addAppointment(ObjectId calendarId, AppointmentModel newAppointment)
        {
            newAppointment.id = ObjectId.GenerateNewId();
            var findCalendarFilter = Builders<CalendarModel>.Filter.Where(x => x.id == calendarId);
            var addAppointmentToListFilter = Builders<CalendarModel>.Update.Push(x => x.appointments, newAppointment);
            UpdateResult updateResult = collection.UpdateOne(findCalendarFilter, addAppointmentToListFilter);
            if (!updateResult.IsAcknowledged)
            {
                return null;
            }

            return newAppointment;
        }

        public bool updateAppointment(AppointmentModel newAppointment)
        {
            var findAppointmentFilter = Builders<CalendarModel>.Filter.ElemMatch(x => x.appointments, x => x.id == newAppointment.id);
            var updateAppointmentFilter = Builders<CalendarModel>.Update.Set(x => x.appointments[-1], newAppointment);

            var result = collection.UpdateOne(findAppointmentFilter, updateAppointmentFilter);
            
            return result.IsAcknowledged;
        }

        public bool deleteAppointment(ObjectId Appointment)
        {
            var findAppointmentFilter = Builders<CalendarModel>.Filter.ElemMatch(x => x.appointments, x => x.id == Appointment);
            var updateAppointmentFilter = Builders<CalendarModel>.Update.Unset(x => x.appointments[-1]);

            var result = collection.UpdateOne(findAppointmentFilter, updateAppointmentFilter);

            return result.IsAcknowledged;
        }

    }
}
