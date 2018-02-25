using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.ViewModels.addAppiontment
{
    public class addAppointmentViewModel
    {

        [Required]
        public string calendarId { get; set; }

        [Required]
        public AppointmentModel appointment { get; set; }

    }
}
