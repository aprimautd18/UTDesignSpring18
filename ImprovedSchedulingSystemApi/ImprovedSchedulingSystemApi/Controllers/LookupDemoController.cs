/*
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ImprovedSchedulingSystemApi.Database;
using Microsoft.Extensions.WebEncoders.Testing;
using UTDDesignMongoDemo.Models;

namespace ImprovedSchedulingSystemApi.Controllers
{
    [Produces("application/json")]
    [Route("api/LookupDemo")]
    public class LookupDemoController : Controller
    {

        MongoAccessor db = new MongoAccessor();

        public LookupDemoController()
        {         

        }


        public IActionResult search([FromQuery] string firstName,[FromQuery] string lastName, [FromQuery] string calName, [FromQuery] string startTime, [FromQuery] string endTime)
        {


            DateTime starttimeDateTime;
            DateTime endtimeDateTime;

            DateTime.TryParse(startTime, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out starttimeDateTime);

            DateTime.TryParse(endTime, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out endtimeDateTime);


            if (firstName == null && lastName == null && calName == null && starttimeDateTime == DateTime.MinValue && endtimeDateTime == DateTime.MinValue)
            {
                return BadRequest();
            }

            List<AppointmentDemoModel> data = db.search(MongoAccessor.APPOINTMENTCOLLECTION, lastName, firstName,
                calName, starttimeDateTime, endtimeDateTime);

            if (data.Count == 0)
            {
                return NotFound();
            }

            return Ok(data);
        } 


    }
}
*/