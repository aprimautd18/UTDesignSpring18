using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;

namespace ImprovedSchedulingSystemApi.ViewModels.updateAppointmentStatus
{
    public class updateStatusViewModel
    {
        public string _id { get; set; }
        public StatusCodes newCode { get; set; }
    }
}
