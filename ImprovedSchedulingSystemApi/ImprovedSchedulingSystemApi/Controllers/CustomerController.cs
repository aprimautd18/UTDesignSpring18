using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Database.ModelAccessors;
using Microsoft.AspNetCore.Mvc;
using ImprovedSchedulingSystemApi.Models;
using ImprovedSchedulingSystemApi.Models.CustomerDTO;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.Controllers
{


    [Route("api/Customer")]
    public class CustomerController : Controller
    {

        CustomerAccessor db = new CustomerAccessor();


        /// <summary>
        /// Search for a list of customers by their details
        /// </summary>
        /// <param name="firstName">First name of the customer</param>
        /// <param name="lastName">Last name of the customer</param>
        /// <param name="phoneNumber">Phone Number of the customer</param>
        /// <returns>A list of customer making the search parameters</returns>
        /// <response code="200">Returns the list of Customer Objects</response>
        [Produces("application/json")]
        [HttpGet("customerLookup")]
        [ProducesResponseType(typeof(List<CustomerModel>), 200)]
        public IActionResult customerLookup([FromQuery]string firstName, [FromQuery]string lastName, [FromQuery]string phoneNumber)
        {
            if (firstName == null && lastName == null && phoneNumber == null)
            {
                return BadRequest();
            }
            List<CustomerModel> data = db.searchByCustomerDetails(firstName, lastName, phoneNumber);
            if (data.Count == 0)
            {
                return NotFound();
            }
            return Ok(data);
        }

        /// <summary>
        /// Lookup a customer by their specific customer id
        /// </summary>
        /// <param name="_id">The customers ObjectiD</param>
        /// <returns>A single customer model element containing all of the details fo the customer</returns>
        /// <response code="200">Returns the Customer Object</response>
        [Produces("application/json")]
        [HttpGet("customerLookupByID")]
        [ProducesResponseType(typeof(CustomerModel), 200)]
        public IActionResult customerLookupById([FromQuery]ObjectId _id)
        {
            
            if (_id == ObjectId.Empty)
            {
                return BadRequest();
            }

            
            CustomerModel data = db.searchByCustomerId(_id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        /// <summary>
        /// Allows a new Customer to be added given the customer model
        /// </summary>
        /// <param name="model">The data the api needs. Look at the example for more info. Leave the customer ID blank as we will generate it. If you need any clarification, send us a message</param>
        /// <returns>The newly added customer containing the new id value generated for the customer</returns>
        /// <response code="200">customer was sucessfully added</response>
        [HttpPost("addCustomer")]
        public IActionResult addCustomer([FromBody]CustomerModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            model = db.addCustomer(model);
            return Ok(model);
        }

        /// <summary>
        /// Allows a Customer to be updated given the customer model
        /// </summary>
        /// <param name="model">The data the api needs. Look at the example for more info. Customer Id Must be an existing id in the db or update will fail. If you need any clarification, send us a message</param>
        /// <returns></returns>
        /// <response code="200">Customer was sucessfully updated</response>
        /// <response code="404">Customerid was not found in the db</response>
        [HttpPost("updateCustomer")]
        public IActionResult updateCustomer([FromBody]CustomerModel model)
        {
            if (model == null || model.id == ObjectId.Empty)
            {
                return BadRequest();
            }

            var result = db.updateCustomer(model);
            if (result)
            {
                return Ok();
            }

            return NotFound();
        }

    }
}