using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovedSchedulingSystemApi.Models.CalenderDTO
{
    public class AppointmentConflictModel
    {
        public AppointmentModel firstAppointment { get; set; }
        public AppointmentModel secondAppointment { get; set; }
    }
}
