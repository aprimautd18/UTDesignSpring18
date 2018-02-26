using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.ViewModels.addAppiontment
{
    public class addAppointmentSubViewModel
    {
        

        public string id { get; set; }
        public string CustomerId { get; set; }
        public string aptstartTime { get; set; }
        public string aptendTime { get; set; }
        public string reason { get; set; }
        public int status { get; set; }

        public AppointmentModel toAppointmentModel()
        {
            AppointmentModel newAppointment = new AppointmentModel();
            newAppointment.id = ObjectId.Empty;
            newAppointment.CustomerId = ObjectId.Parse(CustomerId);
            newAppointment.aptendTime = DateTime.Parse(aptendTime);
            newAppointment.aptstartTime = DateTime.Parse(aptstartTime);
            newAppointment.reason = reason;
            newAppointment.status = (StatusCodes) status;
            return newAppointment;
        }

    }
}
