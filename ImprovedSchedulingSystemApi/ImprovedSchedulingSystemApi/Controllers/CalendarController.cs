﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Controllers.Constants;
using ImprovedSchedulingSystemApi.Database;
using ImprovedSchedulingSystemApi.Database.ModelAccessors;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using ImprovedSchedulingSystemApi.ViewModels.dateLookup;
using ImprovedSchedulingSystemApi.ViewModels.mergeCalender;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Calendar")]
    public class CalendarController : Controller
    {

        CalendarAccessor db = new CalendarAccessor();



        /// <summary>
        /// Returns the calendar id of the calender you lookup
        /// </summary>
        /// <param name="calName">The name of the calendar to retreive</param>
        /// <param name="startTime">The date to retreive</param>
        /// <returns></returns>
        /// <response code="200">The calender id</response>
        /// <response code="404">The calender was not found</response>
        [Produces("application/json")]
        [HttpGet("calenderIdLookup")]
        [ProducesResponseType(typeof(List<dateLookup_CalendarViewModel>), 200)]
        public IActionResult calenderIdLookup([FromQuery] string calName, [FromQuery] string startTime)
        {


            DateTime starttimeDateTime;


            DateTime.TryParse(startTime, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out starttimeDateTime);


            if (calName == null || starttimeDateTime == DateTime.MinValue)
            {
                return BadRequest();
            }


            CalendarModel test = db.dateLookup(calName, starttimeDateTime);
            if (test == null)
            {
                return NotFound();
            }


            return Ok(test.id);
        }


        /// <summary>
        /// Returns a list of calendar objects based on the date given to lookup
        /// </summary>
        /// <param name="calName">The name of the calendar to retreive the dates from</param>
        /// <param name="startTime">The Date to do the lookup with</param>
        /// <param name="range">The number of days to return(Defaults to 1)</param>
        /// <returns></returns>
        /// <response code="200">Returns the List of Calendar Objects</response>
        [Produces("application/json")]
        [HttpGet("dateLookup")]
        [ProducesResponseType(typeof(List<dateLookup_CalendarViewModel>), 200)]
        public IActionResult dateLookup([FromQuery] string calName, [FromQuery] string startTime,
            [FromQuery] int range = 1)
        {


            DateTime starttimeDateTime;


            DateTime.TryParse(startTime, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out starttimeDateTime);


            if (calName == null || starttimeDateTime == DateTime.MinValue)
            {
                return BadRequest();
            }

            if (range <= 0)
            {
                range = 1;
            }

            //list to store the returned values
            List<dateLookup_CalendarViewModel> data = new List<dateLookup_CalendarViewModel>();

            //in the case of large range 
            for (int i = 0; i < range; i++)
            {
                CalendarModel test = db.dateLookup(calName, starttimeDateTime.AddDays(i));
                if (test != null)
                {
                    data.Add(new dateLookup_CalendarViewModel(test));
                }


            }

            return Ok(data);
        }

        /// <summary>
        /// Retuns a list of Calendar objects from Sunday-Saturday. The given date will be one of the days within the week of dates to return
        /// </summary>
        /// <param name="calName">The name of the calendar to retreive the dates from</param>
        /// <param name="startTime">The Date to do the lookup with</param>
        /// <returns>A list of 7 Calendar objects for the week Sunday-Saturday</returns>
        /// <response code="200">Returns the List of Calendar Objects</response>
        [Produces("application/json")]
        [HttpGet("weekLookup")]
        [ProducesResponseType(typeof(List<dateLookup_CalendarViewModel>), 200)]
        public IActionResult dateLookupByContainedDay([FromQuery] string calName, [FromQuery] string startTime)
        {
            DateTime starttimeDateTime;


            DateTime.TryParse(startTime, CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out starttimeDateTime);


            if (calName == null || starttimeDateTime == DateTime.MinValue)
            {
                return BadRequest();
            }

            //list to store the returned values
            List<dateLookup_CalendarViewModel> data = new List<dateLookup_CalendarViewModel>();

            switch (starttimeDateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    break;
                case DayOfWeek.Monday:
                    starttimeDateTime = starttimeDateTime.AddDays(-1);
                    break;
                case DayOfWeek.Tuesday:
                    starttimeDateTime = starttimeDateTime.AddDays(-2);
                    break;
                case DayOfWeek.Wednesday:
                    starttimeDateTime = starttimeDateTime.AddDays(-3);
                    break;
                case DayOfWeek.Thursday:
                    starttimeDateTime = starttimeDateTime.AddDays(-4);
                    break;
                case DayOfWeek.Friday:
                    starttimeDateTime = starttimeDateTime.AddDays(-5);
                    break;
                case DayOfWeek.Saturday:
                    starttimeDateTime = starttimeDateTime.AddDays(-6);
                    break;
                default:
                    return BadRequest();
            }

            for (int i = 0; i < 7; i++)
            {
                CalendarModel test = db.dateLookup(calName, starttimeDateTime.AddDays(i));
                if (test != null) // Check if the date exists in the calender
                {
                    data.Add(new dateLookup_CalendarViewModel(test));
                }

            }

            return Ok(data);
        }

        /// <summary>
        /// Gets a list of all of the potiental calendar names in the calender collection
        /// </summary>
        /// <returns>String list of calendar names</returns>
        /// <response code="200">Returns the list of string objects</response>
        [Produces("application/json")]
        [HttpGet("getCalendarNames")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public IActionResult getCalenderNames()
        {
            List<string> calNames = db.retreiveCalendarNames();
            calNames.Sort();
            return Ok(calNames);
        }


        /// <summary>
        /// Merges two calender objext
        /// </summary>
        /// <param name="model">CalenderA is merged into. CalenderB will be deleted</param>
        /// <returns></returns>
        /// <response code="200">Success. Returns nothing</response>
        /// <response code="400">Error. Check return data</response>
        /// <response code="409">Conflict during the merge. Check the returned list of conflicting objects</response>
        [HttpPost("mergeCalendersByID")]
        public IActionResult mergeCalendersByID([FromBody] MergeCalenderViewModel model)
        {
            if (model.calenderA == ObjectId.Empty || model.calenderB == ObjectId.Empty)
            {
                return BadRequest(ErrorMessageConstants.OBJECT_ID_INVALID);
            }

            List<AppointmentConflictModel> conflicts = db.mergeCalenders(model.calenderA, model.calenderB);
            if (conflicts.Count > 0) // If there is a conflict
            {
                return StatusCode(409, conflicts);
            }

            return Ok();//Otherwise merge was successful so indicate this
        }

        /// <summary>
        /// Calender keep will be merged into, Calender delete will be removed
        /// </summary>
        /// <param name="model">The date to use for the merge. Give a name and the date and we will take all of the appointments from one and put it in the other. The time component of the DateTime object is ignored.</param>
        /// <returns></returns>
        /// <response code="200">Success. Returns nothing</response>
        /// <response code="404">The calenders were not found given the names and date you have given</response>
        /// <response code="409">Conflict during the merge. Check the returned list of conflicting objects</response>
        [HttpPost("mergeCalendersByName")]
        public IActionResult mergeCalendersByName([FromBody] MergeCalenderNameViewModel model)
        {

            CalendarModel keepCalender = db.dateLookup(model.keepCalenderName, model.keepCalenderTime);
            CalendarModel deleteCalender = db.dateLookup(model.deleteCalenderName, model.deleteCalenderTime);
            if (keepCalender == null || deleteCalender == null)
            {
                return NotFound();
            }

            ObjectId keepCalenderId = keepCalender.id;
            ObjectId deleteCalenderId = deleteCalender.id;

            if (keepCalenderId == ObjectId.Empty || deleteCalenderId == ObjectId.Empty)
            {
                return NotFound();
            }

            List<AppointmentConflictModel> conflicts = db.mergeCalenders(keepCalenderId, deleteCalenderId);
            if (conflicts.Count > 0) // If there is a conflict
            {
                return StatusCode(409, conflicts);
            }

            return Ok();//Otherwise merge was successful so indicate this
        }
    }
}