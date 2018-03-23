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
    $scope.searchResults = [];
    $scope.firstName = "Test96635";
    $scope.lastName = "Patient96635";
    $scope.customerID = "";

    //get the list of calendar names
    $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/getCalendarNames")
        .then(function (response) {
            $scope.calendarNames = response.data;
        });

    $scope.apptSearchEngine = function () {
        console.log("in here");
        $scope.apptSearch = true;
        $scope.customerSearch = false;
        var dummyDate = $scope.searchDate.toISOString();
        console.log(dummyDate);
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/dateLookup?calName=" + $scope.searchCalendar + "&startTime=" + dummyDate + "&range=1")
            .then(function (response) {
                console.log("im here biytg");
                var appointments = (response.data);
                $scope.searchResults = appointments[0].appointments;
            });
    }


    $scope.customerSearchEngine = function () {
        console.log("searching customers");
        $scope.apptSearch = false;
        $scope.customerSearch = true;
        //getting the customer ID back from the database to send it back again to get info about that customers appointments
        //2 DB calls for one piece of data. Totally best method possible
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Customer/customerLookup?firstName=" + $scope.firstName + "&lastName=" + $scope.lastName)
            .then(function (response) {
                $scope.customerID = response.data[0].id;
                //loop through and push all the customers onto an array
                console.log($scope.customerID);
                //nested them otherwise it would do the second call before saving the customer ID
                $http.get("https://seniordesign2018dev.azurewebsites.net/api/Appointment/appointmentLookupByCustomerId?id=" + $scope.customerID)
                    .then(function (response) {
                        $scope.searchResults = response.data;

                    });
            });
    }
});

app.controller('dialogService',function($scope, $http) {
    // Get the modal
    var modal = document.getElementById('addModal');

// Get the button that opens the modal
    var btn = document.getElementById("addButton");

// Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];

// When the user clicks the button, open the modal
    $scope.buttonPressed = function() {
        modal.style.display = "block";
    };

// When the user clicks on <span> (x), close the modal
    span.onclick = function() {
        modal.style.display = "none";
    };

// When the user clicks anywhere outside of the modal, close it
    window.onclick = function(event) {
        if (event.target == modal) {
            modal.style.display = "none";
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
        $http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/addAppointment",newAppt);
    };

});
