using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.ViewModels.deleteModel
{
    public class deleteMultipleObjectViewModel
    {

        public List<ObjectId> id { get; set; }
    }
}
