﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Database;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using ImprovedSchedulingSystemApi.ViewModels;
using ImprovedSchedulingSystemApi.ViewModels.addAppointment;
using ImprovedSchedulingSystemApi.ViewModels.dateLookup;
using ImprovedSchedulingSystemApi.ViewModels.updateAppointmentStatus;
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
        /// Reterives all of the appointments associated with a specific ID 
        /// </summary>
        /// <param name="id">The id lookup appointments for </param>
        /// <returns>A list of Appointments </returns>
        /// <response code="200">Returns the appointments associated with the  ID</response>
        [Produces("application/json")]
        [HttpGet("appointmentLookupByCustomerId")]
        [ProducesResponseType(typeof(List<AppointmentModel>), 200)]
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


        /// <summary>
        /// Reterives all of the appointments associated with a specific customer ID 
        /// </summary>
        /// <param name="id">The id of the customer to lookup appointments for </param>
        /// <returns>A list of Appointments associated with the customer</returns>
        /// <response code="200">Returns the appointments associated with the customer ID</response>
        [Produces("application/json")]
        [HttpGet("appointmentLookupById")]
        [ProducesResponseType(typeof(List<AppointmentModel>), 200)]
        public IActionResult appointmentLookupByCustomerId([FromQuery] string id)
        {
            ObjectId parsedId = ObjectId.Parse(id);
            if (parsedId == ObjectId.Empty)
            {
                return BadRequest();
            }

            List<AppointmentModel> data = db.appointmentLookupByCustomerId(parsedId);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        /// <summary>
        /// Allows for the status field of the appointments to be updated
        /// </summary>
        /// <param name="model">contains the id and the newCode to update to</param>
        /// <returns>Nothing</returns>
        /// <response code="200">Status was successfully updated</response>
        [HttpPost("updateAppointmentStatus")]
        public IActionResult updateAppointmentStatus([FromBody]updateStatusViewModel model)
        {
            ObjectId parsedId = ObjectId.Parse(model.id);
            if (parsedId == ObjectId.Empty)
            {
                return BadRequest();
            }

            bool success = db.updateAppointmentStatus(parsedId, model.newCode);
            if (success == false)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Allows a new appointment to be added given the calendar anme and an appointment model. 
        /// </summary>
        /// <param name="model">The data the api needs. Look at the example for more info. Leave the appointment ID blank as we will generate it. If you need any clarification, send us a message</param>
        /// <returns>The newly added appointment containing the new id value generated for the appointment</returns>
        /// /// <response code="200">Appointment was sucessfully added</response>
        [HttpPost("addAppointmentWithCalId")]
        public IActionResult addAppointmentWithCalId( [FromBody]addAppointmentWithCalIdViewModel model)
        {
            if (model.Appointment == null || model.calendarId == ObjectId.Empty)
            {
                return BadRequest();
            }
            AppointmentModel returnedItem = db.addAppointment(model.calendarId, model.Appointment);
            return Ok(returnedItem);
        }

        /// <summary>
        /// Allows a new appointment to be added given the calendarId and an appointment model. 
        /// </summary>
        /// <param name="model">The data the api needs. Look at the example for more info. Leave the appointment ID blank as we will generate it. If you need any clarification, send us a message</param>
        /// <returns>The newly added appointment containing the new id value generated for the appointment</returns>
        /// /// <response code="200">Appointment was sucessfully added</response>
        [HttpPost("addAppointment")]
        public IActionResult addAppointment([FromBody]addAppointmentViewModel model)
        {
            if (model.Appointment == null || model.calendarName == null)
            {
                return BadRequest();
            }
            CalendarAccessor calDb = new CalendarAccessor();

            ObjectId calID = calDb.dateLookup(model.calendarName, model.Appointment.aptstartTime).id;

            AppointmentModel returnedItem = db.addAppointment(calID, model.Appointment);
            return Ok(returnedItem);
        }

        /// <summary>
        /// Allows an Appointment to be updated given the appointment model
        /// </summary>
        /// <param name="model">The model of the appointment to update. The id must match an existing appointment</param>
        /// <returns></returns>
        /// <response code="200">Appointment was sucessfully updated</response>
        /// <response code="404">Appointmentid was not found in the db</response>
        [HttpPost("updateAppointment")]
        public IActionResult updateAppointment([FromBody]AppointmentModel model)
        {
            if (model == null || model.id == ObjectId.Empty)
            {
                return BadRequest();
            }

            bool returnedItem = db.updateAppointment(model);
            if (returnedItem)
            {
                return Ok();
            }

            return NotFound();

        }

        /// <summary>
        /// Allows an Appointment to be delete given the appointment model
        /// </summary>
        /// <param name="id">The object Id for the appointment to delete</param>
        /// <returns></returns>
        /// <response code="200">Appointment was sucessfully delete</response>
        /// <response code="404">Appointment id was not found in the db</response>
        [HttpPost("deleteAppointment")]
        public IActionResult deleteAppointment([FromBody]deleteObjectViewModel model)
        {
            
            if (model.id == ObjectId.Empty)
            {
                return BadRequest();
            }

            bool returnedItem = db.deleteAppointment(model.id);
            if (returnedItem)
            {
                return Ok();
            }

            return NotFound();

        }
    }
}