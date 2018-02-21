using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Database.ModelAccessors;
using Microsoft.AspNetCore.Mvc;
using ImprovedSchedulingSystemApi.Models;
using ImprovedSchedulingSystemApi.Models.CustomerApiDTO;
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
        [Produces("application/json")]
        [HttpGet("customerLookup")]
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
        [Produces("application/json")]
        [HttpGet("customerLookupByID")]
        public IActionResult customerLookupById([FromQuery]string _id)
        {
            ObjectId objectIdStorage = ObjectId.Parse(_id);
            if (objectIdStorage == ObjectId.Empty)
            {
                return BadRequest();
            }

            
            CustomerModel data = db.searchByCustomerId(objectIdStorage);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

/*
        public IActionResult AddCustomer([FromBody]Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            testCustomers.Add(customer);
            return CreatedAtRoute("GetCustomer", new {id = customer.Id}, customer);
        }
        */
    }
}