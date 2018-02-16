using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CustomerDTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ImprovedSchedulingSystemApi.Database.ModelAccessors
{
    public class CustomerAccessor : MongoAbstract<CustomerModel>
    {
        public override IMongoCollection<CustomerModel> collectionSet()
        {
            return db.GetCollection<CustomerModel>("Customer");
        }

        public CustomerModel searchByCustomerId(ObjectId _id)
        {
            return collection.Find(x => x.id == _id).FirstOrDefault();
        }
    }
}
