'use strict';

angular.module('myApp.search', ['ngRoute'])

    .config(['$routeProvider', function($routeProvider) {
        $routeProvider.when('/search', {
            templateUrl: 'search/search.html',
            controller: 'SearchCtrl'
        });
    }])
    .controller('SearchCtrl', [function() {

}]);


var app = angular.module('search', []);
app.controller('searchCtrl', function($scope,$http) {
    $scope.input = "searchHere";
    $scope.searchCalendar = "Kelly 441";
    $scope.searchDate = new Date("2024-12-09");
    $scope.apptSearchResults = {};
    $scope.patientSearchResults = {};
    $scope.firstName = "Test96635";
    $scope.lastName = "Patient96635";
    $scope.customerID = "";
    $scope.deleteList = [];

    //get the list of calendar names
    $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/getCalendarNames")
        .then(function (response) {
            $scope.calendarNames = response.data;
        });

    $scope.apptSearchEngine = function () {
        console.log("in here");
        var dummyDate = new Date($scope.searchDate);
        dummyDate.setDate(dummyDate.getDate() - 1);
        console.log(dummyDate);
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/dateLookup?calName=" + $scope.searchCalendar + "&startTime=" + dummyDate.toISOString() + "&range=1")
            .then(function (response) {
                var appointments = (response.data);
                $scope.apptSearchResults = appointments[0].appointments;
            });
    }

    $scope.customerSearchEngine = function () {
        console.log("searching customers");
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Customer/customerLookup?firstName=" + $scope.firstName + "&lastName=" + $scope.lastName)
            .then(function (response) {
                $scope.patientSearchResults = response.data;
                $scope.customerID = response.data[0].id;
            });
    }

    $scope.addToList = function (id) {
        var indexOfAppt = $scope.deleteList.indexOf(id);
        if(indexOfAppt > -1) {
            console.log("deleting from list");
            delete $scope.deleteList[indexOfAppt];
        }
        else
        {
            console.log("adding to list");
            $scope.deleteList.push(id);
        }
    }

    $scope.deleteFromSearchResults = function () {
        if(confirm('Are you sure you want to delete this appointment?') == false) {
            return;
        }
        var id = {
            id : $scope.deleteList
        }
        $http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/deleteMultipleAppointments", id)
            .then(function (response) {$scope.apptSearchEngine();});
    }
});

app.controller('dialogService',function($scope, $http) {
    // Get the modal
    var addModal = document.getElementById('addModal');

// Get the button that opens the modal
    var btn = document.getElementById("addButton");

// Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];

// When the user clicks the button, open the modal
    $scope.buttonPressed = function() {
        addModal.style.display = "block";
    };

// When the user clicks on <span> (x), close the modal
    span.onclick = function() {
        addModal.style.display = "none";
    };

// When the user clicks anywhere outside of the modal, close it
    window.onclick = function(event) {
        if (event.target == addModal) {
            addModal.style.display = "none";
        }
    };
    $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/getCalendarNames")
        .then(function (response) {
            $scope.calendarNames = response.data;
        });
    $scope.searchCalendar = "Kelly 441";
    $scope.patientFirstName = " ";
    $scope.patientLastName = " ";
    $scope.patientPhoneNumber = " ";
    $scope.newApptDate = "";
    $scope.newApptStartTime = "";
    $scope.newApptEndTime = "";
    $scope.searchPatient = function () {
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Customer/customerLookup?firstName=" + $scope.patientFirstName + "&lastName=" + $scope.patientLastName + "&phoneNumber=" + $scope.patientPhoneNumber)
            .then(function (response) {
                console.log(response);
                $scope.searchedPatientID = response.data[0].id;
                $scope.patientFirstName = response.data[0].firstName;
                $scope.patientLastName = response.data[0].lastName;
                $scope.patientPhoneNumber = response.data[0].phoneNumber;
            });
    };



    $scope.addNewPatient = function () {
        console.log("Adding the patient now");
        $scope.newApptDate.setHours($scope.newApptStartTime.getHours());
        $scope.newApptDate.setMinutes($scope.newApptStartTime.getMinutes());
        $scope.newApptEndDate = new Date($scope.newApptDate);
        $scope.newApptEndDate.setHours($scope.newApptEndTime.getHours());
        $scope.newApptEndDate.setMinutes($scope.newApptEndTime.getMinutes());
        var newAppt = {
            "calendarName": $scope.searchCalendar,
            "appointment": {
                "customerId": $scope.searchedPatientID,
                "aptstartTime": $scope.newApptDate,
                "aptendTime": $scope.newApptEndDate,
            }

        };
        console.log(newAppt);
        $http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/addAppointment",newAppt)
            .then(function (response) {
            });
    };

});
