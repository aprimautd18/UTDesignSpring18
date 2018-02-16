using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CustomerDTO;
using MongoDB.Bson;

namespace ImprovedSchedulingSystemApi.ViewModels.dateLookup
{
    public class dateLookup_CustomerViewModel
    {

        public dateLookup_CustomerViewModel(CustomerModel customer)
        {
            id = customer.id;
            firstName = customer.firstName;
            lastName = customer.lastName;
        }

        public ObjectId id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

    }
}
