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
    //$scope.data = [{"id":1,"firstName":"Alex","lastName":"Khun","address":"123 street", "apptTime" : { "startTime" : "8:30am", "endTime" : "9:00am"}},
    //    {"id":2,"firstName":"K","lastName":"Felten","address":"123 street", "apptTime" : { "startTime" : "8:30am", "endTime" : "9:00am"}}];

    $http.get("https://seniordesign2018dev.azurewebsites.net/api/LookupDemo?startTime=2025-02-25T8%3A00%3A00Z&endTime=2025-02-25T20%3A00%3A00Z")
        .then(function(response) {
            console.log("im here bi");
            console.log(response.data);
            $scope.data = changeDateTime(response.data);
        });
});


function changeDateTime (input) {
    var date, prevDate;
    var output = [];

    //d current time + random time

    var firstAppt = input[0];
    date = new Date(firstAppt.startDateTime);
    date.setHours(8);
    date.setMinutes(0);
    firstAppt.startDateTime = date;

    date = new Date(firstAppt.startDateTime);
    date.setHours(date.getHours() + getRandomInt(2));
    date.setMinutes(date.getMinutes() + (getRandomInt(3) * 15) + 15);
    firstAppt.endDateTime = date;

    output.push(firstAppt);

    for(var x = 1; x<input.length; x++){

        var appt = input[x];
        date = new Date(appt.startDateTime);
        prevDate = new Date(output[x-1].endDateTime);
        date.setHours(prevDate.getHours());
        date.setMinutes(prevDate.getMinutes() + (getRandomInt(3) * 15) + 15);
        appt.startDateTime = date;

        if(date.getHours() >= 17) {
            return output;
        }

        date = new Date(appt.startDateTime);
        date.setHours(date.getHours() + getRandomInt(2));
        date.setMinutes(date.getMinutes() + (getRandomInt(3) * 15) + 15);
        appt.endDateTime = date;

        output.push(appt);
    }

    return output;
}

function getRandomInt(max) {
    return Math.floor(Math.random() * Math.floor(max));
}