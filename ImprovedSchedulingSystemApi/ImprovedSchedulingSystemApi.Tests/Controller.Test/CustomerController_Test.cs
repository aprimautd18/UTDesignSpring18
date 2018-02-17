using System;
using System.Collections.Generic;
using ImprovedSchedulingSystemApi.Controllers;
using ImprovedSchedulingSystemApi.Models;
using ImprovedSchedulingSystemApi.Models.CustomerApiDTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImprovedSchedulingSystemApi.Tests.Controller.Test
{
    [TestClass]
    public class CustomerController_Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new CustomerController();
            var result = controller.GetAll() as List<Customer>;
            Assert.AreEqual(getTestCustomers().Count, result.Count);

        }


        public List<Customer> getTestCustomers()
        {
            List<Customer> testCustomers = new List<Customer>();
            Customer testCustomerone = new Customer();
            testCustomerone.Id = 1;
            testCustomerone.firstName = "Alex";
            testCustomerone.lastName = "Kuhn";
            testCustomerone.address = "123 Street";
            testCustomerone.apptTime = new apptTime();
            testCustomerone.apptTime.startTime = new DateTime(2018, 02, 01, 04, 00, 00);
            testCustomerone.apptTime.endTime = new DateTime(2018, 02, 01, 05, 00, 00);
            Customer testCustomertwo = new Customer();
            testCustomertwo.Id = 2;
            testCustomertwo.firstName = "K";
            testCustomertwo.lastName = "Felten";
            testCustomertwo.address = "123 Street";
            testCustomertwo.apptTime = new apptTime();
            testCustomertwo.apptTime.startTime = new DateTime(2018, 02, 01, 13, 00, 00);
            testCustomertwo.apptTime.endTime = new DateTime(2018, 02, 01, 14, 00, 00);
            Customer testCustomerthree = new Customer();
            testCustomerthree.Id = 3;
            testCustomerthree.firstName = "Mario";
            testCustomerthree.lastName = "Peach";
            testCustomerthree.address = "324 Burbon Street";
            testCustomerthree.apptTime = new apptTime();
            testCustomerthree.apptTime.startTime = new DateTime(2018, 02, 01, 22, 00, 00);
            testCustomerthree.apptTime.endTime = new DateTime(2018, 02, 01, 23, 30, 00);
            Customer testCustomerfour = new Customer();
            testCustomerfour.Id = 4;
            testCustomerfour.firstName = "Pit";
            testCustomerfour.lastName = "Arm";
            testCustomerfour.address = "4400 Shoulder Alley";
            testCustomerfour.apptTime = new apptTime();
            testCustomerfour.apptTime.startTime = new DateTime(2018, 02, 28, 08, 00, 00);
            testCustomerfour.apptTime.endTime = new DateTime(2018, 02, 28, 09, 00, 00);
            Customer testCustomerfive = new Customer();
            testCustomerfive.Id = 5;
            testCustomerfive.firstName = "Sigh";
            testCustomerfive.lastName = "Tigress";
            testCustomerfive.address = "2600 Neer Drive";
            testCustomerfive.apptTime = new apptTime();
            testCustomerfive.apptTime.startTime = new DateTime(2018, 03, 26, 14, 00, 00);
            testCustomerfive.apptTime.endTime = new DateTime(2018, 03, 26, 15, 00, 00);
            testCustomers.Add(testCustomerone);
            testCustomers.Add(testCustomertwo);
            testCustomers.Add(testCustomerthree);
            testCustomers.Add(testCustomerfour);
            testCustomers.Add(testCustomerfive);
            return testCustomers;
        }
    }
}
