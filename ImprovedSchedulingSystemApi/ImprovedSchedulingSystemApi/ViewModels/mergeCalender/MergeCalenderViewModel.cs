using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.ViewModels.mergeCalender
{
    public class MergeCalenderViewModel
    {
        public ObjectId calenderA { get; set; }
        public ObjectId calenderB { get; set; }
    }
}
