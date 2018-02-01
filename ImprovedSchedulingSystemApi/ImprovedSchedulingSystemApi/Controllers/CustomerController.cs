using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImprovedSchedulingSystemApi.Models;

namespace ImprovedSchedulingSystemApi.Controllers
{

    [Route("api/[controller]")]
    public class CustomerController : Controller
    {

        private List<Customer> testCustomers = new List<Customer>();

        public CustomerController()
        {
            Customer testCustomerone = new Customer();
            testCustomerone.Id = 1;
            testCustomerone.firstName = "Alex";
            testCustomerone.lastName = "Khun";
            testCustomerone.address = "123 street";
            Customer testCustomertwo = new Customer();
            testCustomertwo.Id = 2;
            testCustomertwo.firstName = "K";
            testCustomertwo.lastName = "Felten";
            testCustomertwo.address = "123 street";

            testCustomers.Add(testCustomerone);
            testCustomers.Add(testCustomertwo);
        }

        /// <summary>
        /// Retreives all of the Customer objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<Customer> GetAll()
        {
            return testCustomers.ToList();
        }

        /// <summary>
        /// Retrieves a specific object
        /// </summary>
        /// <param name="id">Id of the customer to retrieve</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetCustomer")]
        [Produces("application/json")]
        public IActionResult GetById(long id)
        {
            var customer = testCustomers.FirstOrDefault(t => t.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return new ObjectResult(customer);
        }

        /// <summary>
        /// Adds a new customer object(becuase their is no databse yet it will not save)
        /// </summary>
        /// <param name="customer">Customer to save</param>
        /// <returns>A newly-created customer</returns>
        [HttpPost]
        public IActionResult AddCustomer([FromBody]Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            testCustomers.Add(customer);
            return CreatedAtRoute("GetCustomer", new {id = customer.Id}, customer);
        }

    }
}