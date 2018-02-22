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
    $scope.calendarNames = ["Kelly 441", "Ablasaeu 376"];

    $scope.searchEngine = function () {
        console.log("in here");
        var dummyDate = $scope.searchDate.toISOString();
        console.log(dummyDate);
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/dateLookup?calName=" + $scope.searchCalendar + "&startTime=" + dummyDate + "&range=1")
            .then(function (response) {
                console.log("im here biytg");
                var appointments = (response.data);
                console.log("Appointments");
                console.log(appointments);
                $scope.searchResults = appointments[0].appointments;
            });
    }
});

