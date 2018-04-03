using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Database.ModelAccessors;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ImprovedSchedulingSystemApi.Database.ModelAccessors
{
    public class CalendarAccessor : MongoAbstract<CalendarModel>
    {
        public override IMongoCollection<CalendarModel> collectionSet()
        {
            return db.GetCollection<CalendarModel>("CalendarData"); //Sets the collection to the mongo CalenderData collection
        }

        /// <summary>
        /// Searches the database for a specific calender date
        /// </summary>
        /// <param name="calender_name">The string name of the calender</param>
        /// <param name="start_datetime">A date for the calender(Time component doesn't matter, only the Data component</param>
        /// <returns>A CalenderModel retrieved from the DB</returns>
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

        
        /// <summary>
        /// Retrieves a list of appointments from the db from a given customer ID
        /// </summary>
        /// <param name="id">The id of the customer</param>
        /// <returns>A list of appointments associated with the customer id</returns>
        public List<AppointmentModel> appointmentLookupByCustomerId(ObjectId id)
        {
            return collection.AsQueryable().SelectMany(x => x.appointments).Where(x => x.CustomerId == id).ToList();

        }

        /// <summary>
        /// Retrieves an appointment by the appointment ID
        /// </summary>
        /// <param name="id">The appointment ID</param>
        /// <returns>The list of appointments associated with the id(should be one but just to be safe)</returns>
        public List<AppointmentModel> appointmentLookupById(ObjectId id)
        {
            return collection.AsQueryable().SelectMany(x => x.appointments).Where(x => x.id == id).ToList();

        }

        /// <summary>
        /// Updates the status of a single appointment
        /// </summary>
        /// <param name="_id">The id of the appointment to update</param>
        /// <param name="newCode">The new status code to associate wit the appointment</param>
        /// <returns>A bool on whether the update was accepted</returns>
        public bool updateAppointmentStatus(ObjectId _id, StatusCodes newCode)
        {

            var findAppointmentFilter = Builders<CalendarModel>.Filter.Where(x => x.appointments.Any(y => y.id == _id));
            var updateAppointmentFilter = Builders<CalendarModel>.Update.Set(x => x.appointments[-1].status, newCode);
            UpdateResult updateResult = collection.UpdateOne(findAppointmentFilter, updateAppointmentFilter);
            return updateResult.IsAcknowledged;
        }

        /// <summary>
        /// Retrieves a list of all of the calender name strings
        /// </summary>
        /// <returns>The list of calender name strings</returns>
        public List<string> retreiveCalendarNames()
        {
            return collection.AsQueryable().Select(x => x.calName).Distinct().ToList();
        }

        /// <summary>
        /// Adds a new appointment to the db    
        /// </summary>
        /// <param name="calendarId">The id of the calender </param>
        /// <param name="newAppointment">The new appointment to add to the calender</param>
        /// <returns>The appointment object added to the db(now contains the id object if needed by frontend</returns>
        public AppointmentModel addAppointment(ObjectId calendarId, AppointmentModel newAppointment)
        {
            newAppointment.id = ObjectId.GenerateNewId();
            var findCalendarFilter = Builders<CalendarModel>.Filter.Where(x => x.id == calendarId);
            CalendarModel calender = collection.Find(findCalendarFilter).FirstOrDefault();
            calender.appointments.RemoveAll(x => x == null); //Remove null values
            helperClasses.fastSortAdd(calender.appointments, newAppointment);
            if (helperClasses.appointmentListConflict(calender.appointments))
            {
                return null;
            }
            collection.ReplaceOne(x => x.id == calendarId, calender);
            return newAppointment;
        }

        /// <summary>
        /// Update an appointment in the db
        /// </summary>
        /// <param name="newAppointment">The appointment to update(updates the apponinemnt with the id given in the model, rest is updated)</param>
        /// <returns>A bool stating whether or not the update is a success</returns>
        public bool updateAppointment(AppointmentModel newAppointment)
        {
            var findAppointmentFilter = Builders<CalendarModel>.Filter.ElemMatch(x => x.appointments, x => x.id == newAppointment.id);

            CalendarModel result = collection.Find(findAppointmentFilter).FirstOrDefault();
            int index = result.appointments.FindIndex(x => x.id == newAppointment.id);
            result.appointments.RemoveAt(index);
            result.appointments.RemoveAll(x => x == null);
            helperClasses.fastSortAdd(result.appointments, newAppointment);
            var updateAppointmentFilter = Builders<CalendarModel>.Update.Set(x => x.appointments, result.appointments);

            var updateResult = collection.UpdateOne(findAppointmentFilter, updateAppointmentFilter);
            return updateResult.IsAcknowledged;
        }

        /// <summary>
        /// Delete an appointment from the db
        /// </summary>
        /// <param name="Appointment">The id of the appointment to delete</param>
        /// <returns>A bool stating whether it is successful</returns>
        public bool deleteAppointment(ObjectId Appointment)
        {
            var findAppointmentFilter = Builders<CalendarModel>.Filter.ElemMatch(x => x.appointments, x => x.id == Appointment);
            var updateAppointmentFilter = Builders<CalendarModel>.Update.Unset(x => x.appointments[-1]);

            var result = collection.UpdateOne(findAppointmentFilter, updateAppointmentFilter);

            return result.IsAcknowledged;
        }

        /// <summary>
        /// Merges two calenders given their id
        /// </summary>
        /// <param name="calenderA">The calender to merge into</param>
        /// <param name="calenderB">The calender to put into calenderA</param>
        /// <returns>A list of appointment Conflict models that contains conflict info</returns>
        public List<AppointmentConflictModel> mergeCalenders(ObjectId calenderA, ObjectId calenderB)
        {
            List<AppointmentConflictModel> conflictList = new List<AppointmentConflictModel>();

            CalendarModel keepCalender = collection.Find(x => x.id == calenderA).SingleOrDefault();
            CalendarModel deleteCalender = collection.Find(x => x.id == calenderB).SingleOrDefault();
            List<AppointmentModel> newAppointmentList = new List<AppointmentModel>();

            //Generates the new Appointment list will all appointments in sorted order
            newAppointmentList.AddRange(keepCalender.appointments);
            newAppointmentList.AddRange(deleteCalender.appointments);
            newAppointmentList.Sort();


            //Finds conflicts to return
            for (int i = 1; i < newAppointmentList.Count; i++)
            {
                if (AppointmentModel.conflict(newAppointmentList[i - 1], newAppointmentList[i])) //Check every possible conflict
                {
                    conflictList.Add(new AppointmentConflictModel(newAppointmentList[i - 1], newAppointmentList[i])); // Add a conflict to the list
                }
            }


            //Handle conflicts
            if (conflictList.Count > 0) // If there is at least on conflict
            {
                return conflictList; //Return the list so the api can return it.
            }


            //Database update code
            var updateAppointmentFilter = Builders<CalendarModel>.Update.Set(x => x.appointments, newAppointmentList);
            var result = collection.UpdateOne(x => x.id == calenderA, updateAppointmentFilter);


            //Delete old calender
            updateAppointmentFilter = Builders<CalendarModel>.Update.Unset(x => x.appointments);
            collection.UpdateOne(x => x.id == calenderB, updateAppointmentFilter);


            return conflictList; // Return the list of conflicts(which will be empty to signal the update happened with no conflicts. 
        }

    }
}
