using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.ViewModels.lookupAppiontmentsWithCustomerID
{
    public class lookupAppointmentWithCustomerIdViewModel
    {
        public lookupAppointmentWithCustomerIdViewModel(ObjectId calId, string calName, AppointmentModel model)
        {
            calenderId = calId;
            this.calName = calName;
            Appointment = model;
        }

        public ObjectId calenderId { get; set; }
        public string calName { get; set; }
        public AppointmentModel Appointment { get; set; }
    }
}
