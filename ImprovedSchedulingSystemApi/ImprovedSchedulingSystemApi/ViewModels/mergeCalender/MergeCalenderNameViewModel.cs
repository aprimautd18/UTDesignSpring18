using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovedSchedulingSystemApi.ViewModels.mergeCalender
{
    public class MergeCalenderNameViewModel
    {
        public string keepCalenderName { get; set; }
        public DateTime keepCalenderTime { get; set; }
        public string deleteCalenderName { get; set; }
        public DateTime deleteCalenderTime { get; set; }
    }
}
