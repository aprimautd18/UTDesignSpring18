using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using System.Globalization;

namespace Mango
{


    class Account
    {
        public ObjectId _id { get; set; }
        public String AccountUid { get; set; }
        public int ExternalID { get; set; }
        public String Name { get; set; }
        public String PersonUid { get; set; }
        public String AccountResponsiblePartyUid { get; set; }
        public String ResponsiblePartyRelationshipUid { get; set; }
        public String FormularyList { get; set; }
        public String PatienteRxEligibilityUid { get; set; }
        public String EligibilityProviderUid { get; set; }
        public String EligibilityLastCheckedDate { get; set; }
        public String EligibilityLastResponse { get; set; }
        public int IsMain { get; set; }
        public int Inactive { get; set; }
        public String LastStatementDate { get; set; }
        public int CycleTrigger { get; set; }
        public int ExcludeFromAutomatedCollections { get; set; }
        public String LastModifiedDate { get; set; }
        public String LastModifiedByUid { get; set; }
        public String CoverageTypeUid { get; set; }

    }

    public class AppointmentClean
    {
        public ObjectId _id { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String CalendarName { get; set; }
        public String StartDateTime { get; set; }
        public String EndDateTime { get; set; }
        public String Reason { get; set; }

    }

    public class CalendarTemplateClean
    {
        public ObjectId _id { get; set; }
        public String CalendarName { get; set; }
        public String CalendarTemplateYear { get; set; }
        public String CalendarTemplateStartTime { get; set; }
        public String CalendarTemplateEndTime { get; set; }

    }
    public class AppCleanPractice
    {
        public ObjectId _id { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String CalendarName { get; set; }
        public String StartDateTime { get; set; }
        public String EndDateTime { get; set; }
        public String Reason { get; set; }
    }
    public class CalCleanPractice
    {
        public ObjectId _id { get; set; }
        public String CalendarName { get; set; }
        public String CalendarTemplateYear { get; set; }
        public String CalendarTemplateStartTime { get; set; }
        public String CalendarTemplateEndTime { get; set; }
    }
    class Program

    {

        public static void MassMigration(IMongoCollection<CalendarTemplateClean> calendarCollection, IMongoCollection<AppointmentClean> collection)
        {
            //   var collection = database.GetCollection<AppointmentClean>("AppointmentTest");
            //   var calendarCollection = database.GetCollection<CalendarTemplateClean>("CalendarTemplateTest");

            // Console.WriteLine("Start");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var cC = calendarCollection.AsQueryable();






           // Console.WriteLine("OK");

            String k = "Orner C Calendar";

            var poop = collection.AsQueryable().Select(pop => pop.CalendarName);
            //poopy is a distinct list of calender names (without duplicates)
            var poopy = poop.Distinct();

            //iterating through each calendername
            foreach (var ug in poopy)
            {

                k = ug;

                //accounts is a list of all the appointments that have the givem calendarname
                var accounts = collection.AsQueryable().Where(kk => kk.CalendarName.Equals(k));

                //iteraing each appointment that has the same calender name iteration
                foreach (var ll in accounts)
                {
                    //get the year of appointment
                    String time = ll.StartDateTime;
                    String time2 = ll.EndDateTime;
                    String[] date = time.Split(' ');
                    String[] date2 = time2.Split(' ');

                    //  Console.WriteLine();
                    // Console.WriteLine("Acount name " + ll.CalendarName + " StartDate tiem: " + ll.StartDateTime + " EndDate time " + ll.EndDateTime);

                    //convert the year of appoinent clean
                    DateTime year = DateTime.ParseExact(date[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime year2 = DateTime.ParseExact(date2[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);


                    //convert the hours of appointment clean
                    if (date[1].Contains('.'))
                        date[1] = date[1].Substring(0, date[1].LastIndexOf('.'));
                    if (date2[1].Contains('.'))
                        date2[1] = date2[1].Substring(0, date2[1].LastIndexOf('.'));
                    DateTime hour = DateTime.ParseExact(date[1], "HH:mm:ss", CultureInfo.InvariantCulture);
                    DateTime hour2 = DateTime.ParseExact(date2[1], "HH:mm:ss", CultureInfo.InvariantCulture);

                    //get the actual string with time
                    string hourCompare = hour.ToString("HH:mm:ss");
                    string hour2Compare = hour2.ToString("HH:mm:ss");

                    //split the string into time so we can converted to TimeSpan
                    char deliter = ':';
                    String[] startTime1 = hourCompare.Split(deliter);
                    String[] startTime2 = hour2Compare.Split(deliter);

                    //the hours and the minute of appoitment clean
                    TimeSpan ApCleanStartTime = new TimeSpan(Int32.Parse(startTime1[0]), Int32.Parse(startTime1[1]), Int32.Parse(startTime1[2]));
                    TimeSpan ApCleanEndTime = new TimeSpan(Int32.Parse(startTime2[0]), Int32.Parse(startTime2[1]), Int32.Parse(startTime2[2]));

                    //calenders is a list of all the calender slots with the same year as the given appointment
                    var calendars = calendarCollection.AsQueryable().Where(kkk => kkk.CalendarTemplateYear.Equals(date[0]));

                    //foreach (var a in calendars)
                    //{
                    //    Console.WriteLine("Calendar where the date are equal\n Start time: " + a.CalendarTemplateStartTime + " Endtime " + a.CalendarTemplateEndTime);
                    //}
                    //calendersNew is a list of calender slots with the same year, and with the same caleder name
                    var calendarsNew = calendars.AsQueryable().Where(pp => pp.CalendarName.Equals(k));

                    foreach (var lll in calendarsNew)
                    {

                        //get the ID of calener spot
                        var obj = lll._id;
                        //create new cal slot with same calenrdar name
                        CalendarTemplateClean calendarNew = new CalendarTemplateClean();

                        //the  new calener slot with same calenrdar name and year

                        String time3 = lll.CalendarTemplateYear;
                        String time4 = lll.CalendarTemplateStartTime;
                        String time5 = lll.CalendarTemplateEndTime;
                        // Console.WriteLine("\nCal name " + lll.CalendarName + " Cal start: " + lll.CalendarTemplateStartTime + " Cal end: " + lll.CalendarTemplateEndTime);
                        //Console.WriteLine("App name " + ll.CalendarName + " App start: " + ll.StartDateTime + " Cal end: " + ll.EndDateTime);
                        // Console.WriteLine();

                        //convert the year of Cal clean
                        DateTime year3 = DateTime.ParseExact(time3, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        //convert the time and hour of cal clean
                        if (time4.Contains('.'))
                            time4 = time4.Substring(0, time4.LastIndexOf('.'));
                        if (time5.Contains('.'))
                            time5 = time5.Substring(0, time5.LastIndexOf('.'));

                        //this is just get the time so we can convert it into the timespan
                        DateTime hour3 = DateTime.ParseExact(time4, "HH:mm:ss", CultureInfo.InvariantCulture);
                        DateTime hour4 = DateTime.ParseExact(time5, "HH:mm:ss", CultureInfo.InvariantCulture);
                        //split the string into time so we can converted to TimeSpan
                        string hourCompare2 = hour3.ToString("HH:mm:ss");
                        string hourCompare3 = hour4.ToString("HH:mm:ss");

                        String[] startTime3 = hourCompare2.Split(deliter);
                        String[] startTime4 = hourCompare3.Split(deliter);

                        //timespan for cal clean
                        TimeSpan CalTempStart = new TimeSpan(Int32.Parse(startTime3[0]), Int32.Parse(startTime3[1]), Int32.Parse(startTime3[2]));
                        TimeSpan CalTempEnd = new TimeSpan(Int32.Parse(startTime4[0]), Int32.Parse(startTime4[1]), Int32.Parse(startTime4[2]));
                        //to tell if the appointment is valid in the time slots
                        Boolean flag = true;
                        //check if want start time is early than calCleanStart time
                        if (ApCleanStartTime < CalTempStart)
                        {
                            //   Console.WriteLine("Appointment not open\n");
                            flag = false;
                            //Console.WriteLine(ApCleanStartTime + " SS " + CalTempStart);
                        }
                        //check if want end time is later than calCleanEnd time
                        if (ApCleanEndTime > CalTempEnd)
                        {
                            //    Console.WriteLine("Appointment not open\n");
                            // Console.WriteLine(ApCleanEndTime + " EE " + CalTempEnd);
                            flag = false;
                        }






                        //  if (hourCompare == hourCompare2||hourCompare3==hour2Compare) { flag = false; }


                        //if flag is true then you insert the 2 documents
                        if (flag)
                        {

                            Boolean sameTimeCheck1 = false;
                            Boolean sameTimeCheck2 = false;
                            //check if the time want is avaiable
                            //set the 2 new docuemnts with the new start/end times
                            //1st new document
                            calendarNew.CalendarName = lll.CalendarName;
                            calendarNew.CalendarTemplateYear = lll.CalendarTemplateYear;
                            calendarNew.CalendarTemplateStartTime = hourCompare2;
                            calendarNew.CalendarTemplateEndTime = hourCompare;

                            //2nd new doucument
                            CalendarTemplateClean calendarNew2 = new CalendarTemplateClean(); //create new
                            calendarNew2.CalendarName = lll.CalendarName;   //set calender name
                            calendarNew2.CalendarTemplateYear = lll.CalendarTemplateYear;   //set calender year
                            calendarNew2.CalendarTemplateStartTime = hour2Compare;
                            calendarNew2.CalendarTemplateEndTime = hourCompare3;
                            calendarCollection.DeleteOne(aaa => aaa._id == obj);    //delete the old


                            //need 2, not one

                            if (calendarNew.CalendarTemplateStartTime == calendarNew.CalendarTemplateEndTime)
                            {
                                sameTimeCheck1 = true;
                            }
                            if (calendarNew2.CalendarTemplateStartTime == calendarNew2.CalendarTemplateEndTime)
                            {
                                sameTimeCheck2 = true;
                            }

                            if (sameTimeCheck1 == false)
                            {

                                calendarCollection.InsertOne(calendarNew);  //insert the new
                              //  calendarCollection.InsertOne(calendarNew2);
                            }
                            if (sameTimeCheck2 == false)
                            {

                                //calendarCollection.InsertOne(calendarNew);  //insert the new
                                calendarCollection.InsertOne(calendarNew2);
                            }

                        }
                        //display error if could not add
                        if (flag == false)
                        {
                            //    Console.WriteLine("Could not add appointment due to scheduling conflict objectID: " + ll._id);
                            //    Console.WriteLine("");
                        }

                    }


                }
            }
            //stop the timer

            //  List iop = new List<CalendarTemplateClean>();
            var obje = new ObjectId();
            var fixit = calendarCollection.AsQueryable();
            //foreach (var ttt in fixit)
            //{
            //    if (ttt.CalendarTemplateStartTime == ttt.CalendarTemplateEndTime)
            //    {
            //        obje = ttt._id;
            //    }
            //    calendarCollection.DeleteOne(a => a._id == obje);
            //}


            //stoptimer
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00},{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Runtime: " + elapsedTime);



        }

        public static void MassMigrationNew(IMongoCollection<CalendarTemplateClean> calendarCollection, IMongoCollection<AppointmentClean> collection)
        {
            //   var collection = database.GetCollection<AppointmentClean>("AppointmentTest");
            //   var calendarCollection = database.GetCollection<CalendarTemplateClean>("CalendarTemplateTest");

            Console.WriteLine("Start");

            var cC = calendarCollection.AsQueryable();


            List<string> errorList = new List<string>();



            Console.WriteLine("OK");

            String k = "Orner C Calendar";

            var poop = collection.AsQueryable().Select(pop => pop.CalendarName);
            //poopy is a distinct list of calender names (without duplicates)
            var poopy = poop.Distinct();

            //iterating through each calendername
            foreach (var ug in poopy)
            {

                k = ug;

                //accounts is a list of all the appointments that have the givem calendarname
                var accounts = collection.AsQueryable().Where(kk => kk.CalendarName.Equals(k));

                //iteraing each appointment that has the same calender name iteration
                foreach (var ll in accounts)
                {
                    //get the year of appointment
                    String time = ll.StartDateTime;
                    String time2 = ll.EndDateTime;
                    String[] date = time.Split(' ');
                    String[] date2 = time2.Split(' ');

                    //  Console.WriteLine();
                    // Console.WriteLine("Acount name " + ll.CalendarName + " StartDate tiem: " + ll.StartDateTime + " EndDate time " + ll.EndDateTime);

                    //convert the year of appoinent clean
                    DateTime year = DateTime.ParseExact(date[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime year2 = DateTime.ParseExact(date2[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);


                    //convert the hours of appointment clean
                    if (date[1].Contains('.'))
                        date[1] = date[1].Substring(0, date[1].LastIndexOf('.'));
                    if (date2[1].Contains('.'))
                        date2[1] = date2[1].Substring(0, date2[1].LastIndexOf('.'));
                    DateTime hour = DateTime.ParseExact(date[1], "HH:mm:ss", CultureInfo.InvariantCulture);
                    DateTime hour2 = DateTime.ParseExact(date2[1], "HH:mm:ss", CultureInfo.InvariantCulture);

                    //get the actual string with time
                    string hourCompare = hour.ToString("HH:mm:ss");
                    string hour2Compare = hour2.ToString("HH:mm:ss");

                    //split the string into time so we can converted to TimeSpan
                    char deliter = ':';
                    String[] startTime1 = hourCompare.Split(deliter);
                    String[] startTime2 = hour2Compare.Split(deliter);

                    //the hours and the minute of appoitment clean
                    TimeSpan ApCleanStartTime = new TimeSpan(Int32.Parse(startTime1[0]), Int32.Parse(startTime1[1]), Int32.Parse(startTime1[2]));
                    TimeSpan ApCleanEndTime = new TimeSpan(Int32.Parse(startTime2[0]), Int32.Parse(startTime2[1]), Int32.Parse(startTime2[2]));

                    //calenders is a list of all the calender slots with the same year as the given appointment
                    var calendars = calendarCollection.AsQueryable().Where(kkk => kkk.CalendarTemplateYear.Equals(date[0]));

                    //foreach (var a in calendars)
                    //{
                    //    Console.WriteLine("Calendar where the date are equal\n Start time: " + a.CalendarTemplateStartTime + " Endtime " + a.CalendarTemplateEndTime);
                    //}
                    //calendersNew is a list of calender slots with the same year, and with the same caleder name
                    var calendarsNew = calendars.AsQueryable().Where(pp => pp.CalendarName.Equals(k));

                    foreach (var lll in calendarsNew)
                    {

                        //get the ID of calener spot
                        var obj = lll._id;
                        //create new cal slot with same calenrdar name
                        CalendarTemplateClean calendarNew = new CalendarTemplateClean();

                        //the  new calener slot with same calenrdar name and year

                        String time3 = lll.CalendarTemplateYear;
                        String time4 = lll.CalendarTemplateStartTime;
                        String time5 = lll.CalendarTemplateEndTime;
                        // Console.WriteLine("\nCal name " + lll.CalendarName + " Cal start: " + lll.CalendarTemplateStartTime + " Cal end: " + lll.CalendarTemplateEndTime);
                        //Console.WriteLine("App name " + ll.CalendarName + " App start: " + ll.StartDateTime + " Cal end: " + ll.EndDateTime);
                        // Console.WriteLine();

                        //convert the year of Cal clean
                        DateTime year3 = DateTime.ParseExact(time3, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        //convert the time and hour of cal clean
                        if (time4.Contains('.'))
                            time4 = time4.Substring(0, time4.LastIndexOf('.'));
                        if (time5.Contains('.'))
                            time5 = time5.Substring(0, time5.LastIndexOf('.'));

                        //this is just get the time so we can convert it into the timespan
                        DateTime hour3 = DateTime.ParseExact(time4, "HH:mm:ss", CultureInfo.InvariantCulture);
                        DateTime hour4 = DateTime.ParseExact(time5, "HH:mm:ss", CultureInfo.InvariantCulture);
                        //split the string into time so we can converted to TimeSpan
                        string hourCompare2 = hour3.ToString("HH:mm:ss");
                        string hourCompare3 = hour4.ToString("HH:mm:ss");

                        String[] startTime3 = hourCompare2.Split(deliter);
                        String[] startTime4 = hourCompare3.Split(deliter);

                        //timespan for cal clean
                        TimeSpan CalTempStart = new TimeSpan(Int32.Parse(startTime3[0]), Int32.Parse(startTime3[1]), Int32.Parse(startTime3[2]));
                        TimeSpan CalTempEnd = new TimeSpan(Int32.Parse(startTime4[0]), Int32.Parse(startTime4[1]), Int32.Parse(startTime4[2]));
                        //to tell if the appointment is valid in the time slots
                        Boolean flag = true;
                        //check if want start time is early than calCleanStart time
                        if (ApCleanStartTime < CalTempStart)
                        {
                            //   Console.WriteLine("Appointment not open\n");
                            flag = false;
                            //Console.WriteLine(ApCleanStartTime + " SS " + CalTempStart);
                        }
                        //check if want end time is later than calCleanEnd time
                        if (ApCleanEndTime > CalTempEnd)
                        {
                            //    Console.WriteLine("Appointment not open\n");
                            // Console.WriteLine(ApCleanEndTime + " EE " + CalTempEnd);
                            flag = false;
                        }






                        //  if (hourCompare == hourCompare2||hourCompare3==hour2Compare) { flag = false; }


                        //if flag is true then you insert the 2 documents
                        if (flag)
                        {
                            Boolean sameTimeCheck1 = false;
                            Boolean sameTimeCheck2 = false;

                            //check if the time want is avaiable
                            //set the 2 new docuemnts with the new start/end times
                            //1st new document
                            calendarNew.CalendarName = lll.CalendarName;
                            calendarNew.CalendarTemplateYear = lll.CalendarTemplateYear;
                            calendarNew.CalendarTemplateStartTime = hourCompare2;
                            calendarNew.CalendarTemplateEndTime = hourCompare;

                            //2nd new doucument
                            CalendarTemplateClean calendarNew2 = new CalendarTemplateClean(); //create new
                            calendarNew2.CalendarName = lll.CalendarName;   //set calender name
                            calendarNew2.CalendarTemplateYear = lll.CalendarTemplateYear;   //set calender year
                            calendarNew2.CalendarTemplateStartTime = hour2Compare;
                            calendarNew2.CalendarTemplateEndTime = hourCompare3;
                            calendarCollection.DeleteOne(aaa => aaa._id == obj);    //delete the old
                            if (calendarNew.CalendarTemplateStartTime == calendarNew.CalendarTemplateEndTime)
                            {
                                sameTimeCheck1 = true;
                            }
                            if (calendarNew2.CalendarTemplateStartTime == calendarNew2.CalendarTemplateEndTime)
                            {
                                sameTimeCheck2 = true;
                            }

                            if (sameTimeCheck1 == false)
                            {

                                calendarCollection.InsertOne(calendarNew);  //insert the new
                                                                            // calendarCollection.InsertOne(calendarNew2);
                            }
                            if (sameTimeCheck2 == false)
                            {

                                //  calendarCollection.InsertOne(calendarNew);  //insert the new
                                calendarCollection.InsertOne(calendarNew2);
                            }

                        }
                        //display error if could not add
                        if (flag == false)
                        {
                            errorList.Add("Could not merge appointment due to scheduling conflict "+ll.FirstName + " " + ll.LastName + " " +ll.StartDateTime + " " +ll.EndDateTime);
                            //    Console.WriteLine("Could not add appointment due to scheduling conflict objectID: " + ll._id);
                            //    Console.WriteLine("");
                        }

                    }


                }
            }
            //stop the timer

            //  List iop = new List<CalendarTemplateClean>();
            var obje = new ObjectId();
            //var fixit = calendarCollection.AsQueryable();
            //foreach (var ttt in fixit)
            //{
            //    if (ttt.CalendarTemplateStartTime == ttt.CalendarTemplateEndTime)
            //    {
            //        obje = ttt._id;
            //    }
            //    calendarCollection.DeleteOne(a => a._id == obje);
            //}
            //foreach (var i in errorList) {
            //    Console.WriteLine(i);
            //}


        }

        public static void Delete(IMongoCollection<CalendarTemplateClean> calendarCollection, IMongoCollection<AppointmentClean> collection)
        {
            //deleting start comment

            //did not delete from appointments yet



            var calendarCollection2 = calendarCollection.AsQueryable();
            string dCN, dY, dST, dET;
            Console.WriteLine("Enter the calendername, year, and start time, end time of cancled appointment");
            dCN = Console.ReadLine();

            string newStart = null;
            string newEnd = null;

            dY = Console.ReadLine();
            dST = Console.ReadLine();
            dET = Console.ReadLine();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //get calendars where same year and calendar name
            var step1 = calendarCollection2.AsQueryable().Where(kkk => kkk.CalendarTemplateYear.Equals(dY));
            var step2 = step1.AsQueryable().Where(kkk => kkk.CalendarName.Equals(dCN));

            //foreach (var niner in calendarCollection2)
            //{
            //    Console.WriteLine("mmmmmhmmm");
            //}
            //foreach (var niner in step2)
            //{
            //    Console.WriteLine("hmmm");
            //}

            //get appointments where calendar name is same
            var applist = collection.AsQueryable().Where(i => i.CalendarName == dCN);
            var appList = new List<AppointmentClean>();

            //for every appointment
            foreach (var tol in applist)
            {
                //if year is the same add to list
                if (tol.StartDateTime.Substring(0, 10) == dY)
                {

                    AppointmentClean temp = new AppointmentClean();
                    temp.CalendarName = tol.CalendarName;
                    temp.StartDateTime = tol.StartDateTime;
                    temp.EndDateTime = tol.EndDateTime;
                    temp.Reason = tol.Reason;
                    temp.FirstName = tol.FirstName;
                    temp.LastName = tol.LastName;
                    appList.Add(temp);
                }


            }
            var delid = new ObjectId();

            var appcol = collection.AsQueryable();
            foreach (var fe in appcol)
            {
                if ((dCN == fe.CalendarName) && ((dY + " " + dST) == fe.StartDateTime) && ((dY + " " + dET) == fe.EndDateTime))
                {
                    delid = fe._id;
                }
            }
            collection.DeleteOne(aaa => aaa._id == delid);    //delete the old


            //Console.WriteLine(appList);
            Boolean fg1 = false;
            Boolean fg2 = false;
            Boolean fg3 = false;

            foreach (var lp in appList)
            {
               // Console.WriteLine(lp.CalendarName + " " + lp.FirstName + " " + lp.LastName + " " + lp.StartDateTime + " " + lp.EndDateTime + " " + lp.Reason);
                if (lp.EndDateTime == (dY + " " + dST))
                {
                    // Console.WriteLine("Before");
                    fg1 = true;
                }
                if (lp.StartDateTime == (dY + " " + dET))
                {
                    // Console.WriteLine("After");
                    fg2 = true;
                }

                if (fg1 == true && fg2 == true)
                {
                    fg3 = true;
                }
            }
           // Console.WriteLine(fg1 + " " + fg2 + " " + fg3);

           // Console.ReadLine();






            //  Console.WriteLine(dCN + " " + dY + " " + dST);
            var id1 = new ObjectId();
            var id2 = new ObjectId();

            //no conflict case

            //ands to ors


            if (fg1 == false && fg2 == false && fg3 == false)
            {

                foreach (var llll in step2)
                {
                    //var id1 = llll._id;
                    //var id2 = llll._id;

                    //Console.WriteLine("1:");
                    //Console.WriteLine(llll.CalendarName + " " + llll.CalendarTemplateStartTime + " " + llll.CalendarTemplateEndTime + " " + llll.CalendarTemplateYear);
                    //Console.ReadLine();
                    //Console.WriteLine("2:");

                    //Console.WriteLine(llll.CalendarTemplateStartTime + " " + dST + " " + llll.CalendarTemplateEndTime + " " + dET);
                    //Console.ReadLine();


                    if (fg1 == false && fg2 == false && fg3 == false)
                    {

                        //where no conflict goes
                    }

                    if (llll.CalendarTemplateEndTime.ToString() == (dST))
                    {
                        id1 = llll._id;
                        newStart = llll.CalendarTemplateStartTime.ToString();
                     //   Console.WriteLine("testing1");
                        // Console.ReadLine();

                    }
                    if (llll.CalendarTemplateStartTime.ToString() == (dET))
                    {
                        id2 = llll._id;
                        newEnd = llll.CalendarTemplateEndTime.ToString();
                     //   Console.WriteLine("testing2");
                        // Console.ReadLine();

                    }


                }
                CalendarTemplateClean calendarNewDelete = new CalendarTemplateClean();
                calendarNewDelete.CalendarName = dCN;
                calendarNewDelete.CalendarTemplateStartTime = newStart;
                calendarNewDelete.CalendarTemplateEndTime = newEnd;
                calendarNewDelete.CalendarTemplateYear = dY;

                calendarCollection.DeleteOne(aaa => aaa._id == id1);
                calendarCollection.DeleteOne(aaa => aaa._id == id2);
                calendarCollection.InsertOne(calendarNewDelete);

            }






            if (fg1 == false && fg2 == true && fg3 == false)
            {
                string no = " ";
                string net = " ";

                foreach (var llll in step2)
                {

                    //var id1 = llll._id;
                    //var id2 = llll._id;

                    //Console.WriteLine("1:");
                    //Console.WriteLine(llll.CalendarName + " " + llll.CalendarTemplateStartTime + " " + llll.CalendarTemplateEndTime + " " + llll.CalendarTemplateYear);
                    //Console.ReadLine();
                    //Console.WriteLine("2:");

                    //Console.WriteLine(llll.CalendarTemplateStartTime + " " + dST + " " + llll.CalendarTemplateEndTime + " " + dET);
                    //Console.ReadLine();


                    if (fg1 == false && fg2 == false && fg3 == false)
                    {

                        //where no conflict goes
                    }

                    if (llll.CalendarTemplateEndTime.ToString() == (dST))
                    {
                        id1 = llll._id;
                        newStart = llll.CalendarTemplateStartTime.ToString();
                       // Console.WriteLine("testing1");
                        // Console.ReadLine();

                    }

                    foreach (var ttt in applist)
                    {
                        if (dY + " " + llll.CalendarTemplateStartTime == (ttt.EndDateTime))
                        {
                            string up = ttt.StartDateTime.Substring(11).ToString();
                            newEnd = up;
                            id2 = llll._id;
                            no = llll.CalendarTemplateEndTime;


                        }
                    }
                    if (llll.CalendarTemplateEndTime == no) { net = llll.CalendarTemplateStartTime; }



                }
                CalendarTemplateClean calendarNewDelete = new CalendarTemplateClean();
                calendarNewDelete.CalendarName = dCN;
                calendarNewDelete.CalendarTemplateStartTime = newStart;
                calendarNewDelete.CalendarTemplateEndTime = newEnd;
                calendarNewDelete.CalendarTemplateYear = dY;

                calendarCollection.DeleteOne(aaa => aaa._id == id1);
                calendarCollection.DeleteOne(aaa => aaa._id == id2);
                calendarCollection.InsertOne(calendarNewDelete);

                CalendarTemplateClean calendarNewDelete2 = new CalendarTemplateClean();
                calendarNewDelete2.CalendarName = dCN;
                calendarNewDelete2.CalendarTemplateStartTime = net;
                calendarNewDelete2.CalendarTemplateEndTime = no;
                calendarNewDelete2.CalendarTemplateYear = dY;
                calendarCollection.InsertOne(calendarNewDelete2);







            }
            if (fg1 == true && fg2 == false && fg3 == false)
            {
                string no = " ";
                string net = " ";
                string up = " ";
                string ender = " ";
                string starter = " ";

                foreach (var llll in step2)
                {

                    //var id1 = llll._id;
                    //var id2 = llll._id;

                    //Console.WriteLine("1:");
                    //Console.WriteLine(llll.CalendarName + " " + llll.CalendarTemplateStartTime + " " + llll.CalendarTemplateEndTime + " " + llll.CalendarTemplateYear);
                    //Console.ReadLine();
                    //Console.WriteLine("2:");

                    //Console.WriteLine(llll.CalendarTemplateStartTime + " " + dST + " " + llll.CalendarTemplateEndTime + " " + dET);
                    //Console.ReadLine();



                    foreach (var ttt in applist)
                    {
                        if (dY + " " + llll.CalendarTemplateEndTime == (ttt.StartDateTime))
                        {
                            up = ttt.EndDateTime.Substring(11).ToString();
                            newEnd = up;
                            string x = up;
                            id2 = llll._id;
                            no = llll.CalendarTemplateStartTime;
                            starter = ttt.StartDateTime.Substring(11).ToString();


                        }
                    }




                    if (llll.CalendarTemplateStartTime.ToString() == (dET))
                    {
                        id1 = llll._id;
                        newStart = llll.CalendarTemplateStartTime.ToString();
                        ender = llll.CalendarTemplateEndTime.ToString();
                       // Console.WriteLine("testing1");
                        // Console.ReadLine();

                    }


                }
                CalendarTemplateClean calendarNewDelete = new CalendarTemplateClean();
                calendarNewDelete.CalendarName = dCN;
                calendarNewDelete.CalendarTemplateStartTime = no;
                calendarNewDelete.CalendarTemplateEndTime = starter;
                calendarNewDelete.CalendarTemplateYear = dY;

                calendarCollection.DeleteOne(aaa => aaa._id == id1);
                calendarCollection.DeleteOne(aaa => aaa._id == id2);
                calendarCollection.InsertOne(calendarNewDelete);

                CalendarTemplateClean calendarNewDelete2 = new CalendarTemplateClean();
                calendarNewDelete2.CalendarName = dCN;
                calendarNewDelete2.CalendarTemplateStartTime = up;
                calendarNewDelete2.CalendarTemplateEndTime = ender;
                calendarNewDelete2.CalendarTemplateYear = dY;
                calendarCollection.InsertOne(calendarNewDelete2);






            }




            //both

            if (fg1 == true && fg2 == true && fg3 == true)
            {
                string no = " ";
                string net = " ";
                string up = " ";
                string ender = " ";
                string starter = " ";
                string x = " ";
                string z = " ";
                string y = " ";

                foreach (var llll in step2)
                {

                    //var id1 = llll._id;
                    //var id2 = llll._id;

                    //Console.WriteLine("1:");
                    //Console.WriteLine(llll.CalendarName + " " + llll.CalendarTemplateStartTime + " " + llll.CalendarTemplateEndTime + " " + llll.CalendarTemplateYear);
                    //Console.ReadLine();
                    //Console.WriteLine("2:");

                    //Console.WriteLine(llll.CalendarTemplateStartTime + " " + dST + " " + llll.CalendarTemplateEndTime + " " + dET);
                    //Console.ReadLine();



                    foreach (var ttt in applist)
                    {
                        if (dY + " " + llll.CalendarTemplateEndTime == (ttt.StartDateTime))
                        {
                            up = ttt.EndDateTime.Substring(11).ToString();
                            newEnd = up;
                            x = up;
                            id2 = llll._id;
                            no = llll.CalendarTemplateStartTime;
                            starter = ttt.StartDateTime.Substring(11).ToString();


                        }
                        if (dY + " " + llll.CalendarTemplateStartTime == (ttt.EndDateTime))
                        {
                            y = ttt.StartDateTime.Substring(11).ToString();

                            id2 = llll._id;
                            no = llll.CalendarTemplateStartTime;
                            starter = ttt.StartDateTime.Substring(11).ToString();


                        }
                        if (dY + " " + x == ttt.EndDateTime)
                        {
                            z = ttt.StartDateTime.Substring(11).ToString();
                        }
                        //  if(dY + " " + llll.CalendarTemplateStartTime==(ttt.EndDateTime))

                    }




                    if (llll.CalendarTemplateStartTime == no) { net = llll.CalendarTemplateEndTime; }



                }
                CalendarTemplateClean calendarNewDelete = new CalendarTemplateClean();
                calendarNewDelete.CalendarName = dCN;
                calendarNewDelete.CalendarTemplateStartTime = no;
                calendarNewDelete.CalendarTemplateEndTime = (z);
                calendarNewDelete.CalendarTemplateYear = dY;

                calendarCollection.DeleteOne(aaa => aaa._id == id1);
                calendarCollection.DeleteOne(aaa => aaa._id == id2);
                calendarCollection.InsertOne(calendarNewDelete);

                CalendarTemplateClean calendarNewDelete2 = new CalendarTemplateClean();
                calendarNewDelete2.CalendarName = dCN;
                calendarNewDelete2.CalendarTemplateStartTime = dST;
                calendarNewDelete2.CalendarTemplateEndTime = dET;
                calendarNewDelete2.CalendarTemplateYear = dY;
                calendarCollection.InsertOne(calendarNewDelete2);






            }


            //end comment




            //stoptime
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00},{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Runtime: " + elapsedTime);



            //Console.WriteLine("Enter");
            //Console.ReadLine();

        }


        //Merge
        public static void Merge(IMongoCollection<CalendarTemplateClean> calendarCollection, IMongoCollection<AppointmentClean> collection)
        {

            Console.WriteLine("Merge calendar A ---> B");
            Console.WriteLine("Name of Calendar A: ");
            String calA = Console.ReadLine();
            Console.WriteLine("Name of Calendar B: ");
            String calB = Console.ReadLine();
            Console.WriteLine();
            //Console.WriteLine(calA + " " + calB);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //get every appointment with the same name as calA from Appooinement A
            var appointmentA = collection.AsQueryable().Where(pp => pp.CalendarName.Equals(calA));

            //get every appointmnet with the same name as calB from Appooinement B
            var appointmentB = collection.AsQueryable().Where(pp => pp.CalendarName.Equals(calB));

            //get every calender with the same name as calB from Calendar B
            var calendarB = calendarCollection.AsQueryable().Where(pp => pp.CalendarName.Equals(calB));

            bool calNameNotExit = true;
            //for each in calA, go through it
            foreach (var ll in appointmentA)
            {

                bool delete = true;
                // Console.WriteLine(ll.CalendarName + " " + ll.StartDateTime + ll.EndDateTime);

                //get the time
                String time = ll.StartDateTime;
                String time2 = ll.EndDateTime;
                String[] date = time.Split(' ');
                String[] date2 = time2.Split(' ');

                //convert the year of appoinent clean
                DateTime year = DateTime.ParseExact(date[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime year2 = DateTime.ParseExact(date2[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);

                //convert the hours of appointment clean
                if (date[1].Contains('.'))
                    date[1] = date[1].Substring(0, date[1].LastIndexOf('.'));
                if (date2[1].Contains('.'))
                    date2[1] = date2[1].Substring(0, date2[1].LastIndexOf('.'));
                DateTime hour = DateTime.ParseExact(date[1], "HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime hour2 = DateTime.ParseExact(date2[1], "HH:mm:ss", CultureInfo.InvariantCulture);

                //get the actual string with time
                string hourCompare = hour.ToString("HH:mm:ss");
                string hour2Compare = hour2.ToString("HH:mm:ss");

                //split the string into time so we can converted to TimeSpan
                char deliter = ':';
                String[] startTime1 = hourCompare.Split(deliter);
                String[] startTime2 = hour2Compare.Split(deliter);

                //the hours and the minute of appoitment clean
                TimeSpan ApCleanStartTime = new TimeSpan(Int32.Parse(startTime1[0]), Int32.Parse(startTime1[1]), Int32.Parse(startTime1[2]));
                TimeSpan ApCleanEndTime = new TimeSpan(Int32.Parse(startTime2[0]), Int32.Parse(startTime2[1]), Int32.Parse(startTime2[2]));
                //end getting time from Calendar A appointmnet

                String k = date[0];
                //if same Date of A and B, get it
                var sameDate = appointmentB.AsQueryable().Where(kk => kk.StartDateTime.Contains(date[0]));

                //if same date in appointment
                if (sameDate.Count() > 0)
                {

                    calNameNotExit = false;
                    foreach (var kk in sameDate)
                    {
                        //  Console.WriteLine("Same");
                        // Console.WriteLine(kk.CalendarName + " " + kk.StartDateTime + " " + kk.EndDateTime);


                        //if same start time || same end time
                       // Console.WriteLine();
                        if (kk.StartDateTime.Equals(ll.StartDateTime) || kk.EndDateTime.Equals(ll.EndDateTime))
                        {
                            //Console.WriteLine("Conflict: " + ll.StartDateTime + " " + ll.EndDateTime);
                            // Console.WriteLine();
                        }
                        else
                        {
                            //get Calhoun Calendar entries where it has the same date
                            var sameCalDate = calendarB.AsQueryable().Where(ee => ee.CalendarTemplateYear.Equals(date[0]));
                            //for each calender time spot that is valid for the given appointment
                            bool avaiable = false;
                            foreach (var lll in sameCalDate)
                            {

                                //get the ID of calener spot
                                var obj = lll._id;
                                //create new cal slot with same calenrdar name
                                CalendarTemplateClean calendarNew = new CalendarTemplateClean();

                                //the  new calener slot with same calenrdar name and year

                                String time3 = lll.CalendarTemplateYear;
                                String time4 = lll.CalendarTemplateStartTime;
                                String time5 = lll.CalendarTemplateEndTime;
                                //Console.WriteLine("\nCal name " + lll.CalendarName + " Cal start: " + lll.CalendarTemplateStartTime + " Cal end: " + lll.CalendarTemplateEndTime);
                                //Console.WriteLine("App name " + ll.CalendarName + " App start: " + ll.StartDateTime + " Cal end: " + ll.EndDateTime);
                                // Console.WriteLine();
                                //convert the year of Cal clean
                                DateTime year3 = DateTime.ParseExact(time3, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                //convert the time and hour of cal clean
                                if (time4.Contains('.'))
                                    time4 = time4.Substring(0, time4.LastIndexOf('.'));
                                if (time5.Contains('.'))
                                    time5 = time5.Substring(0, time5.LastIndexOf('.'));

                                //this is just get the time so we can convert it into the timespan
                                DateTime hour3 = DateTime.ParseExact(time4, "HH:mm:ss", CultureInfo.InvariantCulture);
                                DateTime hour4 = DateTime.ParseExact(time5, "HH:mm:ss", CultureInfo.InvariantCulture);
                                //split the string into time so we can converted to TimeSpan
                                string hourCompare2 = hour3.ToString("HH:mm:ss");
                                string hourCompare3 = hour4.ToString("HH:mm:ss");

                                String[] startTime3 = hourCompare2.Split(deliter);
                                String[] startTime4 = hourCompare3.Split(deliter);

                                //timespan for cal clean
                                TimeSpan CalTempStart = new TimeSpan(Int32.Parse(startTime3[0]), Int32.Parse(startTime3[1]), Int32.Parse(startTime3[2]));
                                TimeSpan CalTempEnd = new TimeSpan(Int32.Parse(startTime4[0]), Int32.Parse(startTime4[1]), Int32.Parse(startTime4[2]));
                                //to tell if the appointment is valid in the time slots
                                Boolean flag = true;
                                //check if want start time is early than calCleanStart time
                                if (ApCleanStartTime < CalTempStart)
                                {
                                    //Console.WriteLine("Appointment not open\n");
                                    flag = false;
                                    //Console.WriteLine(ApCleanStartTime + " SS " + CalTempStart);
                                }
                                //check if want end time is later than calCleanEnd time
                                if (ApCleanEndTime > CalTempEnd)
                                {
                                    // Console.WriteLine("Appointment not open\n");
                                    // Console.WriteLine(ApCleanEndTime + " EE " + CalTempEnd);
                                    flag = false;
                                }

                                //if flag is true then you insert the 2 documents
                                if (flag)
                                {
                                    Boolean sameTimeCheck1 = false;
                                    Boolean sameTimeCheck2 = false;

                                    delete = true;
                                    avaiable = true;
                                    //check if the time want is avaiable
                                    //set the 2 new docuemnts with the new start/end times
                                    //1st new document
                                    calendarNew.CalendarName = lll.CalendarName;
                                    calendarNew.CalendarTemplateYear = lll.CalendarTemplateYear;
                                    calendarNew.CalendarTemplateStartTime = hourCompare2;
                                    calendarNew.CalendarTemplateEndTime = hourCompare;

                                    //2nd new doucument
                                    CalendarTemplateClean calendarNew2 = new CalendarTemplateClean(); //create new
                                    calendarNew2.CalendarName = lll.CalendarName;   //set calender name
                                    calendarNew2.CalendarTemplateYear = lll.CalendarTemplateYear;   //set calender year
                                    calendarNew2.CalendarTemplateStartTime = hour2Compare;
                                    calendarNew2.CalendarTemplateEndTime = hourCompare3;
                                    calendarCollection.DeleteOne(aaa => aaa._id == obj);


                                    //insert new error check here

                                    //delete the old
                                    if (calendarNew.CalendarTemplateStartTime == calendarNew.CalendarTemplateEndTime)
                                    {
                                        sameTimeCheck1 = true;
                                    }
                                    if (calendarNew2.CalendarTemplateStartTime == calendarNew2.CalendarTemplateEndTime)
                                    {
                                        sameTimeCheck2 = true;
                                    }

                                    if (sameTimeCheck1 == false)
                                    {

                                        calendarCollection.InsertOne(calendarNew);  //insert the new
                                                                                    // calendarCollection.InsertOne(calendarNew2);
                                    }
                                    if (sameTimeCheck2 == false)
                                    {

                                        //  calendarCollection.InsertOne(calendarNew);  //insert the new
                                        calendarCollection.InsertOne(calendarNew2);
                                    }

                                    //insert in Appoint of B
                                    AppointmentClean appointmentNew = new AppointmentClean();

                                    appointmentNew.CalendarName = lll.CalendarName;

                                    appointmentNew.FirstName = ll.FirstName;
                                    appointmentNew.LastName = ll.LastName;
                                    appointmentNew.Reason = ll.Reason;
                                    appointmentNew.StartDateTime = ll.StartDateTime;
                                    appointmentNew.EndDateTime = ll.EndDateTime;
                                    collection.InsertOne(appointmentNew);
                                    collection.DeleteOne(lkk => lkk._id.Equals(ll._id));
                                }
                                //display error if could not add
                                if (flag == false)
                                {
                                    //Console.WriteLine("Could not add appointment due to scheduling conflict objectID: " + ll._id);
                                    //Console.WriteLine("");
                                }


                            }
                            if (!avaiable)
                            {
                                //Console.WriteLine("Slot not avaiable " + ll.StartDateTime + " " + ll.EndDateTime);
                                //Console.WriteLine();
                            }

                        }
                    }
                    //var obje = new ObjectId();
                    //var fixit = calendarCollection.AsQueryable();
                    //foreach (var ttt in fixit)
                    //{
                    //    if (ttt.CalendarTemplateStartTime == ttt.CalendarTemplateEndTime)
                    //    {
                    //        obje = ttt._id;
                    //    }
                    //    calendarCollection.DeleteOne(a => a._id == obje);
                    //}


                }
                //if not same date
                else
                {
                    calNameNotExit = false;
                    //get Calhoun Calendar entries where it has the same date
                    var sameCalDate = calendarB.AsQueryable().Where(ee => ee.CalendarTemplateYear.Equals(date[0]));
                    //for each calender time spot that is valid for the given appointment
                    bool avaiable = false;
                    foreach (var lll in sameCalDate)
                    {

                        //get the ID of calener spot
                        var obj = lll._id;
                        //create new cal slot with same calenrdar name
                        CalendarTemplateClean calendarNew = new CalendarTemplateClean();

                        //the  new calener slot with same calenrdar name and year

                        String time3 = lll.CalendarTemplateYear;
                        String time4 = lll.CalendarTemplateStartTime;
                        String time5 = lll.CalendarTemplateEndTime;
                        //Console.WriteLine("\nCal name " + lll.CalendarName + " Cal start: " + lll.CalendarTemplateStartTime + " Cal end: " + lll.CalendarTemplateEndTime);
                        //Console.WriteLine("App name " + ll.CalendarName + " App start: " + ll.StartDateTime + " Cal end: " + ll.EndDateTime);
                        // Console.WriteLine();
                        //convert the year of Cal clean
                        DateTime year3 = DateTime.ParseExact(time3, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        //convert the time and hour of cal clean
                        if (time4.Contains('.'))
                            time4 = time4.Substring(0, time4.LastIndexOf('.'));
                        if (time5.Contains('.'))
                            time5 = time5.Substring(0, time5.LastIndexOf('.'));

                        //this is just get the time so we can convert it into the timespan
                        DateTime hour3 = DateTime.ParseExact(time4, "HH:mm:ss", CultureInfo.InvariantCulture);
                        DateTime hour4 = DateTime.ParseExact(time5, "HH:mm:ss", CultureInfo.InvariantCulture);
                        //split the string into time so we can converted to TimeSpan
                        string hourCompare2 = hour3.ToString("HH:mm:ss");
                        string hourCompare3 = hour4.ToString("HH:mm:ss");

                        String[] startTime3 = hourCompare2.Split(deliter);
                        String[] startTime4 = hourCompare3.Split(deliter);

                        //timespan for cal clean
                        TimeSpan CalTempStart = new TimeSpan(Int32.Parse(startTime3[0]), Int32.Parse(startTime3[1]), Int32.Parse(startTime3[2]));
                        TimeSpan CalTempEnd = new TimeSpan(Int32.Parse(startTime4[0]), Int32.Parse(startTime4[1]), Int32.Parse(startTime4[2]));
                        //to tell if the appointment is valid in the time slots
                        Boolean flag = true;
                        //check if want start time is early than calCleanStart time
                        if (ApCleanStartTime < CalTempStart)
                        {
                            //Console.WriteLine("Appointment not open\n");
                            flag = false;
                            //Console.WriteLine(ApCleanStartTime + " SS " + CalTempStart);
                        }
                        //check if want end time is later than calCleanEnd time
                        if (ApCleanEndTime > CalTempEnd)
                        {
                            // Console.WriteLine("Appointment not open\n");
                            // Console.WriteLine(ApCleanEndTime + " EE " + CalTempEnd);
                            flag = false;
                        }

                        //if flag is true then you insert the 2 documents
                        if (flag)
                        {
                            Boolean sameTimeCheck1 = false;
                            Boolean sameTimeCheck2 = false;
                            delete = true;
                            avaiable = true;
                            //check if the time want is avaiable
                            //set the 2 new docuemnts with the new start/end times
                            //1st new document
                            calendarNew.CalendarName = lll.CalendarName;
                            calendarNew.CalendarTemplateYear = lll.CalendarTemplateYear;
                            calendarNew.CalendarTemplateStartTime = hourCompare2;
                            calendarNew.CalendarTemplateEndTime = hourCompare;

                            //2nd new doucument
                            CalendarTemplateClean calendarNew2 = new CalendarTemplateClean(); //create new
                            calendarNew2.CalendarName = lll.CalendarName;   //set calender name
                            calendarNew2.CalendarTemplateYear = lll.CalendarTemplateYear;   //set calender year
                            calendarNew2.CalendarTemplateStartTime = hour2Compare;
                            calendarNew2.CalendarTemplateEndTime = hourCompare3;
                            calendarCollection.DeleteOne(aaa => aaa._id == obj);    //delete the old
                            if (calendarNew.CalendarTemplateStartTime == calendarNew.CalendarTemplateEndTime)
                            {
                                sameTimeCheck1 = true;
                            }
                            if (calendarNew2.CalendarTemplateStartTime == calendarNew2.CalendarTemplateEndTime)
                            {
                                sameTimeCheck2 = true;
                            }

                            if (sameTimeCheck1 == false)
                            {

                                calendarCollection.InsertOne(calendarNew);  //insert the new
                                                                            // calendarCollection.InsertOne(calendarNew2);
                            }
                            if (sameTimeCheck2 == false)
                            {

                                //  calendarCollection.InsertOne(calendarNew);  //insert the new
                                calendarCollection.InsertOne(calendarNew2);
                            }

                            //insert in Appoint of B
                            AppointmentClean appointmentNew = new AppointmentClean();

                            appointmentNew.CalendarName = lll.CalendarName;

                            appointmentNew.FirstName = ll.FirstName;
                            appointmentNew.LastName = ll.LastName;
                            appointmentNew.Reason = ll.Reason;
                            appointmentNew.StartDateTime = ll.StartDateTime;
                            appointmentNew.EndDateTime = ll.EndDateTime;
                            collection.InsertOne(appointmentNew);
                            collection.DeleteOne(lkk => lkk._id.Equals(ll._id));
                        }
                        //display error if could not add
                        if (flag == false)
                        {
                            //Console.WriteLine("Could not add appointment due to scheduling conflict objectID: " + ll._id);
                            //Console.WriteLine("");
                        }

                    }
                    if (!avaiable)
                    {
                        //Console.WriteLine("Slot not avaiable " + ll.StartDateTime + " " + ll.EndDateTime);
                    }

                }


                //delete A
                Console.WriteLine();
            }

            if (calNameNotExit)
            {
                Console.WriteLine("Both calendar name not existed");
            }
            else
            {
                //get every appointment with the same name as calA from Appooinement A
                var appointmentAAfter = collection.AsQueryable().Where(pp => pp.CalendarName.Equals(calA));
                foreach (var ll in appointmentAAfter)
                {
                    Console.WriteLine("Could not merge " + ll.StartDateTime + " " + ll.EndDateTime);
                }
            }
            calendarCollection.DeleteMany(a => a.CalendarName == calA);


            //stoptimer
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00},{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Runtime: " + elapsedTime);



        }


        //Insert
        public static void Insert(IMongoCollection<CalendarTemplateClean> calendarCollection, IMongoCollection<AppointmentClean> collection)
        {
            /*
            User input: name of the calendar
           Date
           Start and End time
           Create in the appointment collection
           Then update in the Calendar collection
            */

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();

            Console.WriteLine("Patient LastName: ");
            String lastName = Console.ReadLine();
            Console.WriteLine("Patient FirstName: ");
            String firstName = Console.ReadLine();
            Console.WriteLine("CalendarName: ");
            String cname = Console.ReadLine();
            Console.WriteLine("StartDateTime (yyyy-mm-dd hh:mm:ss) ");
            String startTime = Console.ReadLine();
            Console.WriteLine("EndDateTime (yyyy-mm-dd hh:mm:ss) ");
            String endTime = Console.ReadLine();
            Console.WriteLine("Reason: ");
            String reason = Console.ReadLine();

            Console.WriteLine(lastName + " " + firstName + " " + cname + " " +
                startTime + " " + endTime + " " + reason);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //error checking
            //create new appointment clean
            AppointmentClean newAppointment = new AppointmentClean();
            newAppointment.LastName = lastName;
            newAppointment.FirstName = firstName;
            newAppointment.CalendarName = cname;
            newAppointment.StartDateTime = startTime;
            newAppointment.EndDateTime = endTime;
            newAppointment.Reason = reason;


            //update it in the calendar clean
            String time = startTime;
            String time2 = endTime;
            String[] date = time.Split(' ');
            String[] date2 = time2.Split(' ');

            //convert the year of appoinent clean
            DateTime year = DateTime.ParseExact(date[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime year2 = DateTime.ParseExact(date2[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);

            //convert the hours of appointment clean
            if (date[1].Contains('.'))
                date[1] = date[1].Substring(0, date[1].LastIndexOf('.'));
            if (date2[1].Contains('.'))
                date2[1] = date2[1].Substring(0, date2[1].LastIndexOf('.'));
            DateTime hour = DateTime.ParseExact(date[1], "HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime hour2 = DateTime.ParseExact(date2[1], "HH:mm:ss", CultureInfo.InvariantCulture);

            //get the actual string with time
            string hourCompare = hour.ToString("HH:mm:ss");
            string hour2Compare = hour2.ToString("HH:mm:ss");

            //split the string into time so we can converted to TimeSpan
            char deliter = ':';
            String[] startTime1 = hourCompare.Split(deliter);
            String[] startTime2 = hour2Compare.Split(deliter);

            //the hours and the minute of appoitment clean
            TimeSpan ApCleanStartTime = new TimeSpan(Int32.Parse(startTime1[0]), Int32.Parse(startTime1[1]), Int32.Parse(startTime1[2]));
            TimeSpan ApCleanEndTime = new TimeSpan(Int32.Parse(startTime2[0]), Int32.Parse(startTime2[1]), Int32.Parse(startTime2[2]));


            //calenders is a list of all the calender slots with the same year as the given appointment
             var calendars = calendarCollection.AsQueryable().Where(kkk => kkk.CalendarTemplateYear.Equals(date[0]));
            //calendersNew is a list of calender slots with the same year, and with the same caleder name
            var calendarsNew = calendars.AsQueryable().Where(pp => pp.CalendarName.Equals(cname));

            //if there is no same calendar, create a new one after this for loop
            Boolean empty = true;
            Boolean exit = true;
            //for each calender time spot that is valid for the given appointment
            foreach (var lll in calendarsNew)
            {

                empty = false;

                //get the ID of calener spot
                //create new cal slot with same calenrdar name
                //the  new calener slot with same calenrdar name and year
                String time3 = lll.CalendarTemplateYear;
                String time4 = lll.CalendarTemplateStartTime;
                String time5 = lll.CalendarTemplateEndTime;

                //Console.WriteLine("Cal name " + lll.CalendarName + " Cal tiem: " + lll.CalendarTemplateStartTime);

                //convert the year of Cal clean
                DateTime year3 = DateTime.ParseExact(time3, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                //convert the time and hour of cal clean
                if (time4.Contains('.'))
                    time4 = time4.Substring(0, time4.LastIndexOf('.'));
                if (time5.Contains('.'))
                    time5 = time5.Substring(0, time5.LastIndexOf('.'));

                //this is just get the time so we can convert it into the timespan
                DateTime hour3 = DateTime.ParseExact(time4, "HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime hour4 = DateTime.ParseExact(time5, "HH:mm:ss", CultureInfo.InvariantCulture);
                //split the string into time so we can converted to TimeSpan
                string hourCompare2 = hour3.ToString("HH:mm:ss");
                string hourCompare3 = hour4.ToString("HH:mm:ss");

                String[] startTime3 = hourCompare2.Split(deliter);
                String[] startTime4 = hourCompare3.Split(deliter);

                //timespan for cal clean
                TimeSpan CalTempStart = new TimeSpan(Int32.Parse(startTime3[0]), Int32.Parse(startTime3[1]), Int32.Parse(startTime3[2]));
                TimeSpan CalTempEnd = new TimeSpan(Int32.Parse(startTime4[0]), Int32.Parse(startTime4[1]), Int32.Parse(startTime4[2]));
                //to tell if the appointment is valid in the time slots
                Boolean flag = true;

                //check if they have the same year
                if (year3 == year)
                {

                    //check if want start time is early than calCleanStart time
                    if (ApCleanStartTime < CalTempStart)
                    {
                        //Console.WriteLine("Appointment not open\n");
                        flag = false;
                        //Console.WriteLine(ApCleanStartTime + " SS " + CalTempStart);
                    }
                    //check if want end time is later than calCleanEnd time
                    if (ApCleanEndTime > CalTempEnd)
                    {
                        //Console.WriteLine("Appointment not open\n");
                        // Console.WriteLine(ApCleanEndTime + " EE " + CalTempEnd);
                        flag = false;
                    }
                    

                    //if flag is true then you insert the 2 documents
                    if (flag)
                    {
                        exit = false;
                        Boolean sameTimeCheck1 = false;
                        Boolean sameTimeCheck2 = false;

                        //check if the time want is avaiable
                        //set the 2 new docuemnts with the new start/end times
                        //1st new document
                        CalendarTemplateClean calendarNew = new CalendarTemplateClean();
                        calendarNew.CalendarName = lll.CalendarName;
                        calendarNew.CalendarTemplateYear = lll.CalendarTemplateYear;
                        calendarNew.CalendarTemplateStartTime = hourCompare2; //new start time
                        calendarNew.CalendarTemplateEndTime = hourCompare; //new end time 

                        //2nd new doucument
                        CalendarTemplateClean calendarNew2 = new CalendarTemplateClean(); //create new
                        calendarNew2.CalendarName = lll.CalendarName;   //set calender name
                        calendarNew2.CalendarTemplateYear = lll.CalendarTemplateYear;   //set calender year
                        calendarNew2.CalendarTemplateStartTime = hour2Compare; //new start time
                        calendarNew2.CalendarTemplateEndTime = hourCompare3; //new end time

                        var obj = lll._id;
                        calendarCollection.DeleteOne(aaa => aaa._id == obj);    //delete the old

                        if (calendarNew.CalendarTemplateStartTime == calendarNew.CalendarTemplateEndTime) {
                            sameTimeCheck1 = true;
                        }
                        if (calendarNew2.CalendarTemplateStartTime == calendarNew2.CalendarTemplateEndTime)
                        {
                            sameTimeCheck2 = true;
                        }

                        if (sameTimeCheck1 == false)
                        {

                            calendarCollection.InsertOne(calendarNew);  //insert the new
                           // calendarCollection.InsertOne(calendarNew2);
                        }
                        if (sameTimeCheck2 == false)
                        {

                          //  calendarCollection.InsertOne(calendarNew);  //insert the new
                            calendarCollection.InsertOne(calendarNew2);
                        }
                    }
                    //display error if could not add
                    if (flag == false)
                    {
                        // Console.WriteLine("Could not add appointment due to scheduling conflict objectID: " + lll.CalendarName + " " + lll.CalendarTemplateStartTime);
                        // Console.WriteLine("");
                    }

                }

            }//end for loop
            var obje = new ObjectId();
            Console.WriteLine("\n\nstart cleanup\n\n");
          //  var fixit = calendarCollection.AsQueryable();
            //foreach (var ttt in fixit)
            //{
            //    if (ttt.CalendarTemplateStartTime == ttt.CalendarTemplateEndTime)
            //    {
            //        obje = ttt._id;
            //    }
            //    calendarCollection.DeleteOne(a => a._id == obje);
            //}
            //if the appoint is existed or no appointment name is found
            if (empty || exit)
            {
                Console.WriteLine("No appointment Name is found or appointment is already existed");
            }
            else
            {
                //added into the collection
                collection.InsertOne(newAppointment);
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00},{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Runtime: " + elapsedTime);
        }


        //Search
        public static void Search(IMongoCollection<CalendarTemplateClean> calendarCollection)
        {
            Console.WriteLine("Calendar Name: ");
            String calName = Console.ReadLine();
            Console.WriteLine("Date (yyyy-mm-dd): ");
            String date = Console.ReadLine();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var avaiableCal = calendarCollection.AsQueryable().Where(pp => pp.CalendarName.Equals(calName) && pp.CalendarTemplateYear.Equals(date));
            foreach (var kk in avaiableCal)
            {
                Console.WriteLine("Start time: " + kk.CalendarTemplateStartTime + " End Time: " + kk.CalendarTemplateEndTime);
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00},{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Runtime: " + elapsedTime);



        }




        static void Main(string[] args)
        {
            var connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("Demo2");

            //Check if it connect or not
            if (database.RunCommandAsync((Command<BsonDocument>)"{ping:1}")
                    .Wait(1000))
            {
                Console.WriteLine("Connected");
            }
            else
            {
                Console.WriteLine("Not Connected");
                System.Environment.Exit(1);
            }

            //start timer 
            //Stopwatch stopWatch = new Stopwatch();
            //Stopwatch stopWatch2 = new Stopwatch();
            //Stopwatch stopWatch3 = new Stopwatch();
            //Stopwatch stopWatch4 = new Stopwatch();
            //Stopwatch stopWatch5 = new Stopwatch();
            //stopWatch.Start();

            //create new Objects
            Account account = new Account();
            AppointmentClean appointment = new AppointmentClean();
            CalendarTemplateClean calendar = new CalendarTemplateClean();



            //get the collections

            //clean to test
            var collection = database.GetCollection<AppointmentClean>("AppointmentTest");
            var calendarCollection = database.GetCollection<CalendarTemplateClean>("CalendarTemplateTest");



            //stopWatch.Stop();

            //TimeSpan ts = stopWatch.Elapsed;
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00},{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            //Console.WriteLine("Runtime: " + elapsedTime);




            //Console.WriteLine("End");

            //Console.WriteLine("Enter");
            //Console.ReadLine();
            Boolean flagger = true;
            do
            {



                Console.WriteLine("Program Start ");
                Console.WriteLine("Enter 1 for Search");
                Console.WriteLine("2 for add appt");
                Console.WriteLine("3 for add all appts");
                Console.WriteLine("4 for delete appt");
                Console.WriteLine("5 for merge appts");
                Console.WriteLine("6 to exit");

                String a = Console.ReadLine();

                if (a == "1")
                {
                    Search(calendarCollection);
                }
                if (a == "2")
                {
                    // stopWatch2.Start();
                    Insert(calendarCollection, collection);
                    //    stopWatch.Stop();
                    //    TimeSpan ts2 = stopWatch2.Elapsed;
                    //    string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00},{3:00}", ts2.Hours, ts2.Minutes, ts2.Seconds, ts2.Milliseconds / 10);
                    //    Console.WriteLine("Runtime: " + elapsedTime2);
                }
                if (a == "3")
                {
                    //  stopWatch3.Start();

                    MassMigration(calendarCollection, collection);
                    //TimeSpan ts3 = stopWatch3.Elapsed;
                    //string elapsedTime3 = String.Format("{0:00}:{1:00}:{2:00},{3:00}", ts3.Hours, ts3.Minutes, ts3.Seconds, ts3.Milliseconds / 10);
                    //Console.WriteLine("Runtime: " + elapsedTime3);
                }
                if (a == "4")
                {
                    //stopWatch4.Start();
                    Delete(calendarCollection, collection);
                    //TimeSpan ts4 = stopWatch4.Elapsed;
                    //string elapsedTime4 = String.Format("{0:00}:{1:00}:{2:00},{3:00}", ts4.Hours, ts4.Minutes, ts4.Seconds, ts4.Milliseconds / 10);
                    //Console.WriteLine("Runtime: " + elapsedTime4);
                }
                if (a == "5")
                {
                    // stopWatch5.Start();
                    Merge(calendarCollection, collection);
                    //TimeSpan ts5 = stopWatch5.Elapsed;
                    //string elapsedTime5 = String.Format("{0:00}:{1:00}:{2:00},{3:00}", ts5.Hours, ts5.Minutes, ts5.Seconds, ts5.Milliseconds / 10);
                    //Console.WriteLine("Runtime: " + elapsedTime5);

                }
                if (a == "6") {
                    flagger = false;
                }

            } while (flagger == true);




            //Console.WriteLine("MassMigration");
            //MassMigration(calendarCollection, collection);


            //   Console.WriteLine("Merge");
            //  Merge(calendarCollection, collection);

            //Console.WriteLine("insert");
            //Insert(calendarCollection, collection);

            //Console.WriteLine("delete");
            //Delete(calendarCollection, collection);



            //Console.WriteLine("end of program, enter any key to exit");
            //Console.ReadLine();







        }
    }
}







