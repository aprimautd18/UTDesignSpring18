using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Models.CustomerApiDTO;

namespace ImprovedSchedulingSystemApi.Models
{
    public class Customer
    {
        public long Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public apptTime apptTime { get; set; }
    }
    
}
