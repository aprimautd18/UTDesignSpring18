using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Database.ModelAccessors;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.ViewModels.dateLookup
{
    public class dateLookup_CalendarViewModel
    {
        public dateLookup_CalendarViewModel(CalendarModel calendar)
        {
            CustomerAccessor db = new CustomerAccessor();
            id = calendar.id;
            calName = calendar.calName;
            startTime = calendar.startTime;
            endTime = calendar.endTime;
            appointments = new List<dateLookup_AppointmentViewModel>();
            foreach (var x in calendar.appointments)
            {
                appointments.Add(new dateLookup_AppointmentViewModel(x, db.searchByCustomerId(x.CustomerId)));
            }

        }


        public ObjectId id { get; set; }

      
        public string calName { get; set; }

    
        public DateTime startTime { get; set; }

       
        public DateTime endTime { get; set; }

        
        public List<dateLookup_AppointmentViewModel> appointments { get; set; }

        public DateTime getDate()
        {
            return startTime.Date;

        }
    }
}
