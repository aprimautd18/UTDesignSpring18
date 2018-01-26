using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
namespace Mango
{
    public class Class1
    {
        [BsonId]
        public ObjectId Id {
            get;
            set;
        }
    }
}
