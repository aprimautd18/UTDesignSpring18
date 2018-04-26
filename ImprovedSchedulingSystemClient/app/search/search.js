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

//This is the controller for the Search.html page

var app = angular.module('search', []);
app.controller('searchCtrl', function($scope,$http) {
    $scope.input = "searchHere";
    $scope.searchCalendar = "Kelly 441";
    $scope.modalCalendar = "";
    $scope.searchDate = new Date("2024-12-09");
    $scope.apptSearchResults = {};
    $scope.patientSearchResults = {};
    $scope.firstName = "Test96635";
    $scope.lastName = "Patient96635";
    $scope.customerID = "";
    $scope.deleteList = [];
    $scope.newApptDate = "";
    $scope.newApptStartTime = "";
    $scope.newApptEndTime = "";
    $scope.appointmentID = "";
    $scope.searchCalendarMerge = "";
    $scope.searchDateMerge = "";
    $scope.addApptDate = "";
    $scope.addApptStartTime = "";
    $scope.addApptEndTime = "";
    var realUpdateDateToSend = "";
    var patientTable = document.getElementById("patientSearchTable");
    var patientAppointmentTable = document.getElementById("patientAppointmentTable");
    patientAppointmentTable.style.display = "none";

    //get the list of calendar names
    $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/getCalendarNames")
        .then(function (response) {
            $scope.calendarNames = response.data;
        });

    //Searches the Database for appointments given the selected date and calendar name
    $scope.apptSearchEngine = function () {
        console.log("in here");
        var dummyDate = new Date($scope.searchDate);
        //the -1 is required to retrieve the desired date, otherwise the next day
        dummyDate.setDate(dummyDate.getDate() - 1);
        console.log(dummyDate);
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/dateLookup?calName=" + $scope.searchCalendar + "&startTime=" + dummyDate.toISOString() + "&range=1")
            .then(function (response) {
                var appointments = (response.data);
                $scope.apptSearchResults = appointments[0].appointments;
            });
    };

    //Searches the Database for a list of patients with matching names, either first, last, or both
    $scope.customerSearchEngine = function () {
        console.log("searching customers");
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Customer/customerLookup?firstName=" + $scope.firstName + "&lastName=" + $scope.lastName)
            .then(function (response) {
                $scope.patientSearchResults = response.data;
                $scope.customerID = response.data[0].id;
                patientTable.style.display = "inline";
                patientAppointmentTable.style.display = "none";
            });
    };
    //Adds the checked appointment to the list of the appointments to delete
    $scope.addToList = function (id) {
        var indexOfAppt = $scope.deleteList.indexOf(id);
        if (indexOfAppt > -1) {
            console.log("deleting from list");
            delete $scope.deleteList[indexOfAppt];
        }
        else {
            console.log("adding to list");
            $scope.deleteList.push(id);
        }
    }
    //When ana ppointment is unchecked it is removed from the list of appointments to delete.
    $scope.deleteFromSearchResults = function () {
        if (confirm('Are you sure you want to delete this appointment?') == false) {
            return;
        }
        var id = {
            id: $scope.deleteList
        }
        $http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/deleteMultipleAppointments", id)
            .then(function () {
                $scope.apptSearchEngine();
            }, function (response) {
                console.log(response);
                alert("Bad request\n" + response.statusText);
            });
    }
    // Get the modal
    var updateModal = document.getElementById('updateModal');
    var mergeModal = document.getElementById('mergeModal');
    var addModal = document.getElementById('addModal');

    // Get the button that opens the modal
    var btn = document.getElementById("updateButton");

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];
    var span1 = document.getElementsByClassName("close")[1];

    // When the user clicks the update button, open the  update appointment modal
    $scope.buttonPressed = function (clickedAppointment) {
        updateModal.style.display = "block";
        $scope.appointmentID = clickedAppointment.appointment.id;
        $scope.modalCalendar = clickedAppointment.calName;
        $scope.newApptDate = new Date(clickedAppointment.appointment.aptstartTime);
        $scope.newApptStartTime = new Date($scope.newApptDate);
        $scope.newApptEndTime = new Date(clickedAppointment.appointment.aptendTime);
        realUpdateDateToSend = new Date($scope.newApptDate);
    };
    //opens the add appointment modal
    $scope.addButtonPressed = function() {
        addModal.style.display = "block";
    };

    // When the user clicks the button, open the merge modal
    $scope.mergeButtonPressed = function () {
        mergeModal.style.display = "block";
    };
    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        updateModal.style.display = "none";
    };
    //hides the merge modal when clicked x of the modal
    span1.onclick = function() {
        mergeModal.style.display = "none";
    };

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == updateModal) {
            updateModal.style.display = "none";
        }
        if (event.target == mergeModal) {
            mergeModal.style.display = "none";
        }
        if (event.target == addModal) {
            addModal.style.display = "none";
        }
    };
    //when the patients last name is clicked, this function searches the database for all appointments associated with that patiend
    $scope.searchPatientAppointments = function (patient) {
        $scope.patient = patient;
        console.log("Searching for the patients appointments");
        var patientID = patient.id;
        $scope.firstName = patient.firstName;
        $scope.lastName = patient.lastName;
        $scope.patientPhoneNumber = patient.phoneNumber;
        $scope.customerID = patientID;
        $scope.patientAppointmentSearchResults = [];
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Appointment/appointmentLookupByCustomerId?id=" + patientID)
            .then(function (response) {
                patientTable.style.display = "none";
                patientAppointmentTable.style.display = "inline";
                console.log(response.data);
                $scope.patientAppointmentSearchResults = response.data;
            }, function (response) {
                alert(response.statusText);
            });
    };
    //This function sends the updated data to the database when the submit button on the modal is pressed
    //The endpoint currently only allows the times to be updated while the modal shows the opportunity to offer more functionality
    $scope.updateAppointment = function () {
        console.log("Adding the patient now");
        realUpdateDateToSend.setHours($scope.newApptStartTime.getHours());
        realUpdateDateToSend.setMinutes($scope.newApptStartTime.getMinutes());
        $scope.newApptEndDate = new Date(realUpdateDateToSend);
        $scope.newApptEndDate.setHours($scope.newApptEndTime.getHours());
        $scope.newApptEndDate.setMinutes($scope.newApptEndTime.getMinutes());
        var newAppt = {
            "id": $scope.appointmentID,
            "customerId": $scope.customerID,
            "aptstartTime": realUpdateDateToSend.toISOString(),
            "aptendTime": $scope.newApptEndDate.toISOString(),
        }
        console.log(newAppt);
        $http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/updateAppointment", newAppt)
            .then(function (response) {
                $scope.searchPatientAppointments($scope.patient);
                updateModal.style.display = "none";
            }, function (response) {
                alert(response.statusText);
            });
    };
    //When submit is pressed in the submit modal, this builds the object and sends it to the database.
    //Currently an entire day's appointment is sent to the DB
    $scope.mergeAppointment = function () {
        console.log("Merging the Appointments now");
        var mergeAppt = {
            "keepCalenderName": $scope.searchCalendarMerge,
            "keepCalenderTime": $scope.searchDateMerge.toISOString(),
            "deleteCalenderName": $scope.searchCalendar,
            "deleteCalenderTime": $scope.searchDate.toISOString()
        }
        $http.post("https://seniordesign2018dev.azurewebsites.net/api/Calendar/mergeCalendersByName", mergeAppt)
            .then(function (response) {
                mergeModal.style.display = "none";
            }, function (response) {
                alert(response.statusText);
            });
    };
    //This function is called when the submit button on the add modal is pressed.
    //It builds the object and sends the post request.
    //The alert is supposed to show the error if any is generated by the server.
    $scope.addNewAppointment = function () {
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Customer/customerLookup?firstName=" + $scope.firstName + "&lastName=" + $scope.lastName)
            .then(function (response) {
                console.log(response);
                $scope.searchedPatientID = response.data[0].id;
                console.log("Adding the patient now");
                $scope.addApptDate.setHours($scope.addApptStartTime.getHours());
                $scope.addApptDate.setMinutes($scope.addApptStartTime.getMinutes());
                $scope.addApptEndDate = new Date($scope.addApptDate);
                $scope.addApptEndDate.setHours($scope.addApptEndTime.getHours());
                $scope.addApptEndDate.setMinutes($scope.addApptEndTime.getMinutes());
                var newAppt = {
                    "calendarName": $scope.searchCalendar,
                    "appointment": {
                        "customerId": $scope.searchedPatientID,
                        "aptstartTime": $scope.addApptDate.toISOString(),
                        "aptendTime": $scope.addApptEndDate.toISOString(),
                    }

                };
                console.log(newAppt);
                $http.post("https://seniordesign2018dev.azurewebsites.net/api/Appointment/addAppointment",newAppt)
                    .then(function (response) {
                    }, function (response) {
                        alert(response.statusText);
                    });
            });
    };
});
