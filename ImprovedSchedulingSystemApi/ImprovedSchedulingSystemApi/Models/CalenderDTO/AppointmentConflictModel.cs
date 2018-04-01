using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovedSchedulingSystemApi.Models.CalenderDTO
{
    public class AppointmentConflictModel
    {

        public AppointmentConflictModel(AppointmentModel first, AppointmentModel second)
        {
            firstAppointment = first;
            secondAppointment = second;
        }
        public AppointmentModel firstAppointment { get; set; }
        public AppointmentModel secondAppointment { get; set; }
    }
}
