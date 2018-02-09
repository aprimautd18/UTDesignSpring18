using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImprovedSchedulingSystemApi.Database;
using ImprovedSchedulingSystemApi.Database.ModelAccessors;
using ImprovedSchedulingSystemApi.Models.CalenderDTO;
using ImprovedSchedulingSystemApi.Models.CustomerDTO;
using MongoDB.Bson;

namespace Db_Import_Tool
{
    class Program
    {

        static CustomerAccessor customerDB = new CustomerAccessor();
        static CalendarAccessor calendarDB = new CalendarAccessor();

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Need 2 args. File 1 (Calender.txt) / File 2 (Appointment.txt)");
                return;
            }



            var CalenderDataFile = args[0];
            var AppointmentDataFile = args[1];

            List<CalendarModel> calendarsToInsert = new List<CalendarModel>();
            Console.WriteLine("Generating Calendars...");
            try
            {
                using (StreamReader reader = new StreamReader(CalenderDataFile))
                {
                    String headerLine = reader.ReadLine();
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        CalendarModel newCalendar = new CalendarModel();
                        String[] seperatedValues = line.Split('|');

                        newCalendar.calName = seperatedValues[0];
                        newCalendar.startTime = DateTime.Parse(seperatedValues[1] + "T" + seperatedValues[2]);
                        newCalendar.endTime = DateTime.Parse(seperatedValues[1] + "T" + seperatedValues[3]);

                        calendarsToInsert.Add(newCalendar);
                    }

                }
                Console.WriteLine("Generating Customers...");
                List<CustomerModel> CustomersToInsert = new List<CustomerModel>();
                using (StreamReader reader = new StreamReader(AppointmentDataFile))
                {
                    String headerLine = reader.ReadLine();
                    String line;
                    int phoneNumberGen = 1111111111;
                    while ((line = reader.ReadLine()) != null)
                    {
                        CustomerModel newCustomer = new CustomerModel();
                        AppointmentModel newAppointment = new AppointmentModel();
                        String[] seperatedValues = line.Split('|');
                        newCustomer.firstName = seperatedValues[1];
                        newCustomer.lastName = seperatedValues[0];
                        newCustomer.phoneNumber = phoneNumberGen++.ToString();
                        newCustomer.id = new ObjectId();
                        CustomersToInsert.Add(newCustomer);


                        
                    }


                }

                Console.WriteLine("Writing Customer Data to DB...");
                customerDB.addManyRecords(CustomersToInsert);

                Console.WriteLine("Generating Appointments...");
                using (StreamReader reader = new StreamReader(AppointmentDataFile))
                {
                    String headerLine = reader.ReadLine();
                    String line;
                    int lineNumber = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] seperatedValues = line.Split('|');
                        AppointmentModel newAppointment = new AppointmentModel();
                        newAppointment.CustomerId = CustomersToInsert[lineNumber].id;
                        newAppointment.aptstartTime = DateTime.ParseExact(seperatedValues[3],
                            "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        newAppointment.aptendTime = DateTime.ParseExact(seperatedValues[4], "yyyy-MM-dd HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture);
                        newAppointment.reason = seperatedValues[5];


                        foreach (var x in calendarsToInsert)
                        {
                            if (newAppointment.aptstartTime >= x.startTime && newAppointment.aptstartTime <= x.endTime && seperatedValues[2].Equals(x.calName))
                            {
                                x.appointments.Add(newAppointment);
                                continue;
                            }
                        }

                        lineNumber++;

                    }


                }

               



               
                Console.WriteLine("Inserting Calendar/Appointment Data into DB...");
                calendarDB.addManyRecords(calendarsToInsert);

            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }


        }
    }
}
