using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using ImprovedSchedulingSystemApi.Models.CustomerDTO;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.ViewModels.dateLookup
{
    public class dateLookup_AppointmentViewModel
    {

        public dateLookup_AppointmentViewModel(AppointmentModel appointment, CustomerModel customerData)
        {
            id = appointment.id;
            customer = new dateLookup_CustomerViewModel(customerData);
            aptstartTime = appointment.aptstartTime;
            aptendTime = appointment.aptendTime;
            status = appointment.status;
        }

        public ObjectId id { get; set; }
        public dateLookup_CustomerViewModel customer { get; set; }
        public DateTime aptstartTime { get; set; }
        public DateTime aptendTime { get; set; }

    
       // public string reason { get; set; }

        public int status { get; set; }

    }
}
