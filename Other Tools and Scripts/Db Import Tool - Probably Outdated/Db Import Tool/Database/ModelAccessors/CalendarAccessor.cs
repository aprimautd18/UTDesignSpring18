using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using MongoDB.Driver;

namespace ImprovedSchedulingSystemApi.Database
{
    public class CalendarAccessor : MongoAbstract<CalendarModel>
    {
        public override IMongoCollection<CalendarModel> collectionSet()
        {
            return db.GetCollection<CalendarModel>("CalendarData");
        }


    }
}
