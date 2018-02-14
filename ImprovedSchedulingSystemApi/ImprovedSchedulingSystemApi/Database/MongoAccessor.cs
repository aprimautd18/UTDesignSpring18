/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ImprovedSchedulingSystemApi.Models;
using UTDDesignMongoDemo.Models;

namespace ImprovedSchedulingSystemApi.Database
{


    public class MongoAccessor
    {
        private const string CONNECTIONSTRING = "mongodb://mongoUser:alexsplushpenguin123@35.226.179.59";
        private static MongoClient _mongoclient;
        private static IMongoDatabase db;
        public static IMongoCollection<AppointmentDemoModel> APPOINTMENTCOLLECTION;
        public static IMongoCollection<AppointmentDemoModel> CALENDERCOLLECTION;
        public MongoAccessor()
        {
            _mongoclient = new MongoClient(CONNECTIONSTRING);
            db = _mongoclient.GetDatabase("mongoDemo");
            APPOINTMENTCOLLECTION = db.GetCollection<AppointmentDemoModel>("appointments");
            
        }

        public void addRecord(AppointmentDemoModel model, IMongoCollection<AppointmentDemoModel> collection)
        {
            collection.InsertOne(model);
        }
  
        public async void addManyRecords(List<AppointmentDemoModel> modelList, IMongoCollection<AppointmentDemoModel> collection)
        {
            await collection.InsertManyAsync(modelList); //This says don't return the data until this task is completed(But becuase their is no return, it doesn't matter)
        }

        public List<AppointmentDemoModel> returnAll(IMongoCollection<AppointmentDemoModel> collection)
        {
            List<AppointmentDemoModel> list = collection.Find(_ => true).ToList();
            return list;
        }

        public void deleteAll(IMongoCollection<AppointmentDemoModel> collection)
        {
            collection.DeleteMany(_ => true); // Delete all values
        }

        public List<AppointmentDemoModel> search(IMongoCollection<AppointmentDemoModel> collection, string last_name,
            string first_name, string calender_name, DateTime start_datetime, DateTime end_datetime)
        {

            var builder = Builders<AppointmentDemoModel>.Filter; // Contructs a filter for the search.(Id normally use a lambda but in this case we need to incrumentally build the query)
            FilterDefinition<AppointmentDemoModel> filter = FilterDefinition<AppointmentDemoModel>.Empty; // Place to store the filter for the find query
            
            if (last_name != null)
            {
                filter = filter & builder.Eq(x => x.LastName, last_name);
            }

            if (first_name != null)
            {
                filter = filter & builder.Eq(x => x.FirstName, first_name);
            }
            if (calender_name != null)
            {
                filter = filter & builder.Eq(x => x.CalendarName, calender_name);
            }
            if (start_datetime != DateTime.MinValue)
            {
                filter = filter & builder.Eq(x => x.StartDateTime, start_datetime);
                
            }
            if (end_datetime != DateTime.MinValue)
            {
                filter = filter & builder.Eq(x => x.EndDateTime, end_datetime);
            }

            List<AppointmentDemoModel> returnedItems = collection.Find(filter).ToList();
            return returnedItems;

        }
        

    }
}
*/