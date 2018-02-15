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
app.controller('demoCtrl', function($scope,$http) {
    $scope.firstname = "John";
    $scope.lastname = "Doe";
    $scope.data = {};
    $scope.data.calendarTimes = [];
    $scope.data.todayAppointment = [];
    $scope.data.calendarData = [];
   
    $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/dateLookup?calName=Ablaseau%20376&startTime=2023-10-09%2000%3A00%3A00&range=5")
        .then(function(response) {
            console.log("im here bi");
            //console.log(response.data);
            var appointments = (response.data);
            console.log("Appointments" + appointments);
            $scope.data.todayAppointment = appointments[0].appointments;

            for(var daily in appointments) {
                $scope.data.calendarData.push(addBlankAppts(appointments[daily]));
            }
            console.log("Data?");
            console.log($scope.data.calendarData);
            fillTimeSlots($scope);
        });
});

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

function dateSelector() {

}
