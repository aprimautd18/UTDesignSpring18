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

        public List<CustomerModel> searchByCustomerDetails(string firstName, string lastName, string phoneNumber)
        {
            var builder = Builders<CustomerModel>.Filter; // Contructs a filter for the search.(Id normally use a lambda but in this case we need to incrumentally build the query)
            FilterDefinition<CustomerModel> filter = FilterDefinition<CustomerModel>.Empty; // Place to store the filter for the find query


            if (firstName != null)
            {
                filter = filter & builder.Eq(x => x.firstName, firstName);
            }

            if (lastName != null)
            {
                filter = filter & builder.Eq(x => x.lastName, lastName);
            }

            if (phoneNumber != null)
            {
                filter = filter & builder.Eq(x => x.phoneNumber, phoneNumber);
            }
           

            List<CustomerModel> returnedItems = collection.Find(filter).ToList();
            return returnedItems;
        }


        public CustomerModel addCustomer(CustomerModel model)
        {
            model.id = ObjectId.GenerateNewId();
            collection.InsertOne(model);
            return model;
        }

        public bool updateCustomer(CustomerModel model)
        {
            var result = collection.ReplaceOne(x => x.id == model.id, model);
            return result.IsAcknowledged;
        }

        public bool deleteCustomer(ObjectId id)
        {
            var result = collection.DeleteOne(x => x.id == id);
            return result.IsAcknowledged;
        }
    }
}
