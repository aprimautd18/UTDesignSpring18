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
                console.log($scope.customerID);
                //nested them otherwise it would do the second call before saving the customer ID
                $http.get("https://seniordesign2018dev.azurewebsites.net/api/Appointment/appointmentLookupByCustomerId?id=" + $scope.customerID)
                    .then(function (response) {
                        $scope.searchResults = response.data;

                    });
            });
    }
});

