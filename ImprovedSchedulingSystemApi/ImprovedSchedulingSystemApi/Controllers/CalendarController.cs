using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Database;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImprovedSchedulingSystemApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Calendar")]
    public class CalendarController : Controller
    {

        CalendarAccessor db = new CalendarAccessor();

    
        [Produces("application/json")]
        [HttpGet("dateLookup")]
        public IActionResult dateLookup( [FromQuery] string calName, [FromQuery] string startTime, [FromQuery] int range)
        {


            DateTime starttimeDateTime;
           
        
            DateTime.TryParse(startTime, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out starttimeDateTime);


            if (calName == null || starttimeDateTime == DateTime.MinValue)
            {
                return BadRequest();
            }

            if(range <= 0)
            {
                range = 1;
            }

            //list to store the returned values
            List<CalendarModel> data = new List<CalendarModel>();

            //in the case of large range 
            for (int i = 0; i < range; i++)
            {
               data.Add ( db.dateLookup(calName, starttimeDateTime.AddDays(i)));
               
            }

            return Ok(data);
        }
    }
}