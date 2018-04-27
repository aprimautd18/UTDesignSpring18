'use strict';
//Config for angular
angular.module('myApp.view2', ['ngRoute'])

    .config(['$routeProvider', function($routeProvider) {
        $routeProvider.when('/view2', {
            templateUrl: 'view2/view2.html',
            controller: 'View2Ctrl'
        });
    }])

    .controller('View2Ctrl', [function() {

    }]);

var app = angular.module('demo', []);
//These functions run on page load.
app.controller('onPageLoad', function($scope,$http) {
    $scope.firstname = "John";
    $scope.lastname = "Doe";
    $scope.data = {};
    $scope.data.calendarTimes = [];
    $scope.data.todayAppointment = [];
    $scope.data.calendarData = [];
    $scope.officeApptTimes = [];
    $scope.hasAppts = true;

    //manually setting the office hours until someone decides differently
    for(var i = 8; i< 12; i++) {
        $scope.officeApptTimes.push(i + " am");
    }
    $scope.officeApptTimes.push("12 pm")
    for(var i = 1; i< 6; i++) {
        $scope.officeApptTimes.push(i + " pm");
    }

    //AJAX call to populate the calender data
    $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/weekLookup?calName=Kelly%20441&startTime=2024-11-16%2000%3A00%3A00&range=7")
        .then(function(response) {
            var appointments = (response.data);
            $scope.data.todayAppointment = appointments[0].appointments;
            $scope.weeklyDate = {};
            $scope.weeklyDate.firstDayOfCalendar = appointments[0].startTime;
            $scope.weeklyDate.lastDayOfCalendar = appointments[appointments.length - 1].startTime;
            for(var daily in appointments) {
                $scope.data.calendarData.push(addBlankAppts(appointments[daily]));
            }
            $scope.pageLoaded = true;
        });

    if($scope.data.todayAppointment.length > 1) {
        $scope.hasAppts = true;
        $scope.hasNoAppts = false;
    }
    else {
        $scope.hasNoAppts = true;
        $scope.hasAppts = false;
    }
    //calling the time bar function
    timeBarHeight($scope);

});

//the status code function on the daily schedule
app.controller('statusCodeController', function($scope,$http){
    $scope.data= {
        model:null,
        statusOptions:[
            {statusID: '0', statusName: 'Scheduled', Color: "#f1f1f1"},
            {statusID: '1', statusName: 'Checked In', Color: "#eaf298"},
            {statusID: '2', statusName: 'In Process', Color: "lightblue"},
            {statusID: '3', statusName: 'Discharged', Color: "lightgreen"},
            {statusID: '4', statusName: 'Canceled',Color: "#ff596c"}
        ],

        selectedStatusCode: null
    };
    //persisting the data to the DB
    $scope.updatedStatus= function( appointmentId, indexOfWorkingStatus) {
        var data= {
            "id": ''+appointmentId,
            "newCode": $scope.data.selectedStatusCode
        };
        document.getElementsByTagName("TR")[indexOfWorkingStatus+1].style.backgroundColor=$scope.data.statusOptions[data.newCode].Color;
        $http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/updateAppointmentStatus",data);
    };
});

//this controller controls the left side of the view2Page.
app.controller('dateController', function($scope, $route, $http) {
    var todayDate = new Date("2024-11-11");
    $scope.selectedDate = todayDate;
    $scope.searchCalendar = "Kelly 441";
    $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/getCalendarNames")
        .then(function (response) {
            $scope.calendarNames = response.data;
        });
    //Function for the timebar height, will move the bar on every update.
    $scope.updateDate = function() {
        timeBarHeight($scope);
        var selectedDate = new Date($scope.selectedDate);
        //Hack because of USA date time
        selectedDate.setDate(selectedDate.getDate() - 1);
        selectedDate = selectedDate.toISOString();
        console.log("get your selected data here: "+selectedDate);
        console.log($scope.searchCalendar);
        //get 7 days of calender
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/weekLookup?calName=" + $scope.searchCalendar + "&startTime=" + selectedDate + "&range=7")
            .then(function (response) {
                renderCalendar(response, selectedDate, $scope);
            });
        if($scope.data.todayAppointment.length > 1) {
            $scope.hasAppts = true;
            $scope.hasNoAppts = false;
        }
        else {
            $scope.hasNoAppts = true;
            $scope.hasAppts = false;
        }
    }
//  On click, delete the appointment stuff
    $scope.clickAppointment = function (appointment) {
        var id = appointment.id;
        if(confirm('Are you sure you want to delete this appointment?') == false) {
            return;
        }
        console.log('Appointment Clicked!');
        console.log(appointment.id);
        $http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/deleteAppointment", {id: id})
            .then(onPost());
    }

    function onPost () {
        console.log("In here");
        console.log($scope.selectedDate);
        $scope.updateDate();
    }
});

//Creates the full calender object and displays to the user
function renderCalendar(response, selectedDate, $scope) {
    if(!selectedDate) {
        selectedDate = new Date($scope.selectedDate);
        selectedDate = selectedDate.toISOString();
    }
    var appointments = (response.data);
    $scope.weeklyDate.firstDayOfCalendar = appointments[0].startTime;
    $scope.weeklyDate.lastDayOfCalendar = appointments[appointments.length - 1].startTime;
    console.log("Refreshed Appts");
    console.log(appointments);

    var todayIndex = daysBetween(appointments[0].startTime,selectedDate);
    $scope.data.todayAppointment = appointments[todayIndex].appointments;
    $scope.data.calendarData = [];
    for (var daily in appointments) {
        var columnOfAppointments = addBlankAppts(appointments[daily]);
        for(var x = 0; x< columnOfAppointments.length; x++) {
            if(!columnOfAppointments[x].index) {
                columnOfAppointments[x].index = {};
            }
            columnOfAppointments[x].index.day = daily;
        }
        $scope.data.calendarData.push(columnOfAppointments);
    }
    $scope.data.calendarData[todayIndex].isCorrectDayToHighlight="correctHighlightedDay";
}
//Manual function to determine the timebar height math
function timeBarHeight($scope) {
    var currentTime = new Date();
    var dayStart = new Date(currentTime);
    dayStart.setHours(8);
    dayStart.setMinutes(0);
    var diff = Math.abs(currentTime - dayStart);
    $scope.scrollableTimeBarHeight = Math.min(50 + (diff/50000), 695);

}
//the pixel height between functions
function daysBetween(firstDate, secondDate) {
    var newDate1 = new Date(firstDate);
    newDate1.setHours(0);
    var newDate2 = new Date(secondDate);
    var diff = (newDate1 - newDate2);
    var daysApart = Math.abs(diff / 86400000);
    daysApart = Math.round(daysApart);
    return daysApart;
}
//the pixel height between functions

function calculatePixelHeights (appointment) {
    var pixelHeight, current, next;

    current = new Date(appointment.aptstartTime);
    next = new Date(appointment.aptendTime);

    pixelHeight = ((next - current) / 100000) * 2;

    return pixelHeight;
}
//add blakc appointments into the column of the day appointments
function addBlankAppts (input) {
    var output = [];
    var current, next, blank, blankDateTime;
    if(input.appointments.length < 1) {
        blank = {};
        blank.aptstartTime = input.startTime;
        blank.aptendTime = input.endTime;
        blank.firstName = " ";
        blank.isBlank = "blank";
        blank.heightInPixels = calculatePixelHeights(blank);
        output.push(blank);
        return output;
    }
    input = input.appointments;
    console.log("input[0]");
    var firstAptStartTime = new Date(input[0].aptstartTime);
    if(firstAptStartTime.getHours()>8){
        blank = {};
        blank.aptstartTime = new Date(firstAptStartTime);
        blank.aptstartTime.setHours(8);
        blank.aptstartTime.setMinutes(0);
        blank.aptendTime = new Date(firstAptStartTime);
        blank.firstName = " ";
        blank.isBlank = "blank";
        blank.heightInPixels = calculatePixelHeights(blank);
        output.push(blank);
    }
    input[0].heightInPixels = calculatePixelHeights(input[0]);
    input[0].index = {}
    input[0].index.row = 0;
    output.push(input[0]);
    for(var x = 1; x<input.length; x ++) {
        current = new Date(input[x-1].aptendTime);
        next = new Date(input[x].aptstartTime);
        var timeDiff = next - current;
        if(timeDiff > 0) {
            //using slice because it passes by reference and not by value which results in duplicates
            blank = input.slice(x-1,x);
            blank.aptstartTime = current;
            blank.aptendTime = next;
            blank.firstName = " ";
            blank.isBlank = "blank";
            blank.heightInPixels = calculatePixelHeights(blank);
            output.push(blank);
        }
        input[x].index = {}
        input[x].index.row = x;
        input[x].heightInPixels = calculatePixelHeights(input[x]);
        output.push(input[x]);
    }
    var lastAppt = new Date(output[output.length - 1].aptendTime);
    if(lastAppt.getHours() < 17) {
        blank = {};
        blank.aptstartTime = lastAppt;
        var endTime = new Date(lastAppt);
        endTime.setHours(17);
        endTime.setMinutes(0);
        blank.aptendTime = endTime;
        blank.firstName = " ";
        blank.isBlank = "blank";
        blank.heightInPixels = calculatePixelHeights(blank);
        output.push(blank);
    }
    console.log(output);
    return output;
}

