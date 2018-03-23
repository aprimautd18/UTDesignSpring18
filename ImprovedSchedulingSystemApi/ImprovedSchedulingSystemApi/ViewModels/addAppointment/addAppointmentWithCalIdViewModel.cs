using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.ViewModels.addAppointment
{
    public class addAppointmentWithCalIdViewModel
    {

        public ObjectId calendarId { get; set; }

        public AppointmentModel Appointment { get; set; }

    }
}
