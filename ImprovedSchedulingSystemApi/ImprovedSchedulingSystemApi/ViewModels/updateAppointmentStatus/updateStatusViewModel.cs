using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;

namespace ImprovedSchedulingSystemApi.ViewModels.updateAppointmentStatus
{
    public class updateStatusViewModel
    {
        [Required]
        public string id { get; set; }

        [Required]
        public StatusCodes newCode { get; set; }
    }
}
