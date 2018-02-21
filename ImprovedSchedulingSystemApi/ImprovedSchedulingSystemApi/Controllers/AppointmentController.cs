using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Database;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using ImprovedSchedulingSystemApi.ViewModels.dateLookup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointment")]
    public class AppointmentController : Controller
    {
        CalendarAccessor db = new CalendarAccessor();

       
        /// <summary>
        /// Reterives all of the appointments associated with a specific customer ID 
        /// </summary>
        /// <param name="id">The id of the customer to lookup appointments for </param>
        /// <returns>A list of Appointments associated with the customer</returns>
        [Produces("application/json")]
        [HttpGet("appointmentLookupById")]
        public IActionResult appointmentLookupById([FromQuery] string id)
        {
            ObjectId parsedId = ObjectId.Parse(id);
            if (parsedId == ObjectId.Empty)
            {
                return BadRequest();
            }

            List<AppointmentModel> data = db.appointmentLookupById(parsedId);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

    }
}