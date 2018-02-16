using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ImprovedSchedulingSystemApi.Database
{
    public abstract class MongoAbstract<T>
    {
        protected const string CONNECTIONSTRING = "mongodb://mongoUser:alexsplushpenguin123@35.226.179.59";
        protected static MongoClient _mongoclient;
        protected static IMongoDatabase db;
        protected static IMongoCollection<T> collection;

        public MongoAbstract()
        {
            _mongoclient = new MongoClient(CONNECTIONSTRING);
            db = _mongoclient.GetDatabase("applicationDatabase");
            collection = collectionSet();
        }

        public abstract IMongoCollection<T> collectionSet();

        public void addRecord(T model)
        {
            collection.InsertOne(model);
        }

        public async void addManyRecords(List<T> modelList)
        {
            await collection.InsertManyAsync(modelList); //This says don't return the data until this task is completed(But becuase their is no return, it doesn't matter)
        }

        public List<T> returnAll()
        {
            List<T> list = collection.Find(_ => true).ToList();
            return list;
        }

    }
}
