'use strict';

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

    $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/dateLookup?calName=Ablaseau%20376&startTime=2024-12-06%2000%3A00%3A00&range=7")
        .then(function(response) {
            var appointments = (response.data);
            $scope.data.todayAppointment = appointments[0].appointments;
            $scope.weeklyDate = {};
            $scope.weeklyDate.firstDayOfCalendar = appointments[0].startTime;
            $scope.weeklyDate.lastDayOfCalendar = appointments[appointments.length - 1].startTime;
            for(var daily in appointments) {
                $scope.data.calendarData.push(addBlankAppts(appointments[daily]));
            }
            fillTimeSlots($scope);
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
    timeBarHeight($scope);

});
app.controller('statusCodeController', function($scope,$http){
$scope.data= {
    model:null,
    statusOptions:[
        {statusID: '0', statusName: 'Scheduled', Color: "gray"},
        {statusID: '1', statusName: 'Checked In', Color: "yellow"},
        {statusID: '2', statusName: 'In Process', Color: "blue"},
        {statusID: '3', statusName: 'Discharged', Color: "green"},
        {statusID: '4', statusName: 'Canceled',Color: "red"}
],

    selectedStatusCode: null
};

$scope.updatedStatus= function( a) {
    var data= {
        "id": ''+a,
        "newCode": $scope.data.selectedStatusCode
    };
    $http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/updateAppointmentStatus",data);
};
});


app.controller('dateController', function($scope, $route, $http) {
    var todayDate = new Date("2024-11-16");
    $scope.selectedDate = todayDate;
    $scope.updateDate = function() {
        timeBarHeight($scope);
        var selectedDate = new Date($scope.selectedDate);
        selectedDate.setDate(selectedDate.getDate() - 1);
        selectedDate = selectedDate.toISOString();
        console.log("get your selected data here: "+selectedDate);
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/weekLookup?calName=Kelly%20441&startTime=" + selectedDate + "&range=7")
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

    $scope.clickAppointment = function (appointment) {
        console.log('Appointment Clicked!');
        console.log(appointment.id);
        var id = appointment.id;
        $http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/deleteAppointment", {id: id})
            .then(onPost());
        //delete allAppointments[appointment.index.day][appointment.index.row];
        // renderCalendar({data: allAppointments}, null, $scope);
        // appointment.isBlank = true;
        // console.log(appointment);
        // console.log(appointment.id);
        //$scope.$apply();
        //$http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/deleteAppointment",appointmentId);
    }

    function onPost () {
        console.log("In here");
        console.log($scope.selectedDate);
        $scope.updateDate();
    }
});


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
   // $scope.selectedDate = new Date(selectedDate);
    $scope.data.calendarData[todayIndex].isCorrectDayToHighlight="correctHighlightedDay";
    fillTimeSlots($scope);
}

function timeBarHeight($scope) {
    var currentTime = new Date();
    var dayStart = new Date(currentTime);
    dayStart.setHours(8);
    dayStart.setMinutes(0);
    var diff = Math.abs(currentTime - dayStart);
    $scope.scrollableTimeBarHeight = Math.min(50 + (diff/50000), 695);

}

function daysBetween(firstDate, secondDate) {
    var newDate1 = new Date(firstDate);
    newDate1.setHours(0);
    var newDate2 = new Date(secondDate);
    var diff = (newDate1 - newDate2);
    var daysApart = Math.abs(diff / 86400000);
    daysApart = Math.round(daysApart);
    return daysApart;
}

function fillTimeSlots($scope) {
    var time;

    for(var hour = 8; hour <= 12; hour++) {
        for(var minute = 0; minute <= 45; minute += 15) {
            time = {
                time : " " + hour + ":" + minute
            };
            $scope.data.calendarTimes.push(time);
        }
    }
    for(var hour = 13; hour < 17; hour++) {
        for (var minute = 0; minute <= 45; minute += 15) {
            time = {
                time: " " + (hour-12) + ":" + minute
            };
            $scope.data.calendarTimes.push(time);
        }
    }
}

function calculatePixelHeights (appointment) {
    var pixelHeight, current, next;

    current = new Date(appointment.aptstartTime);
    next = new Date(appointment.aptendTime);

    pixelHeight = ((next - current) / 100000) * 2;

    return pixelHeight;
}

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

